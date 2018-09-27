using System;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using ZF.Core.Entity;
using ZF.Dapper.Db.Repository;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.Json;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    [RequestAuthorize]
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// 用户类
        /// </summary>
        protected RegisterUserOutputDto UserObject
        {
            get
            {
                if (Request.Headers.Authorization?.Parameter != null)
                {
                    var authorization = Request.Headers.Authorization.Parameter;
                    try
                    {
                        //解密Ticket
                        var formsAuthenticationTicket = Des3Cryption.Decrypt3DES(authorization);
                        if (!string.IsNullOrEmpty(formsAuthenticationTicket))
                        {
                            var strTicket = formsAuthenticationTicket;
                            //从Ticket里面获取用户名和密码
                            string telphoneNum = "";
                            if (!string.IsNullOrEmpty(strTicket))
                            {
                                var index = strTicket.Split('&');
                                telphoneNum = index[0];
                            }
                            var model = new RegisterUserRepository().GetLogin(telphoneNum);
                            return model;
                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    return null;
                }
                return null;
            }
        }
    }
}
