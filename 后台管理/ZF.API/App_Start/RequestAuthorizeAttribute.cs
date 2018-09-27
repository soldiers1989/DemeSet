using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using ZF.Dapper.Db.Repository;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.RedisCache;

namespace ZF.API
{
    /// <summary>
    /// 自定义此特性用于接口的身份验证
    /// </summary>
    public class RequestAuthorizeAttribute : AuthorizeAttribute
    {

        //重写基类的验证方式，加入我们自定义的Ticket验证
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //从http请求的头里面获取身份验证信息，验证是否是请求发起方的ticket
            var authorization = actionContext.Request.Headers.Authorization;
            if (authorization?.Parameter != null)
            {
                //解密用户ticket,并校验用户名密码是否匹配
                var encryptTicket = authorization.Parameter;
                if (encryptTicket != "null" && ValidateTicket(encryptTicket))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                var attributes =
                    actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()
                        .OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string encryptTicket)
        {
            if (string.IsNullOrEmpty("undeencryptTicket") || encryptTicket == "undefined")
            {
                return false;
            }
            else
            {
                //解密Ticket
                var formsAuthenticationTicket = Des3Cryption.Decrypt3DES(encryptTicket);
                if (!string.IsNullOrEmpty(formsAuthenticationTicket))
                {
                    var strTicket = formsAuthenticationTicket;
                    //从Ticket里面获取用户名和密码
                    string telphoneNum = "";
                    string loginPwd = "";
                    string guid = "";
                    if (!string.IsNullOrEmpty(strTicket))
                    {
                        var index = strTicket.Split('&');
                        switch (index.Length)
                        {
                            case 0:
                                break;
                            case 1:
                                telphoneNum = index[0];
                                break;
                            case 2:
                                telphoneNum = index[0];
                                loginPwd = index[1];
                                break;
                            case 3:
                                telphoneNum = index[0];
                                loginPwd = index[1];
                                guid = index[2];
                                break;
                        }
                    }
                    var model = new RegisterUserRepository().GetLogin(telphoneNum);
                    if (model == null)
                    {
                        return false;
                    }
                    if (!string.IsNullOrEmpty(RedisCacheHelper.Get<string>(model.Id))&& !string.IsNullOrWhiteSpace(guid) && guid != RedisCacheHelper.Get<string>(model.Id))
                    {
                        return false;
                    }
                    if (model.LoginPwd == loginPwd)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }
    }
}