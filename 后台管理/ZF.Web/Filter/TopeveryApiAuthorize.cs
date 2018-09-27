using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using ZF.Infrastructure.Entity;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;


namespace ZF.Web.Filter
{
    /// <summary>
    /// 验证过滤器
    /// </summary>
    public class TopeveryApiAuthorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            //从Cookie中读取出Json串并反序列化成实体
            //取出Cookie对象
            HttpCookie userInfoCookie = HttpContext.Current.Request.Cookies.Get("UserInfo");
            // return true;
            return userInfoCookie != null;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
          filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, WrapEntity.CreateForbiddenEntity());
        }
    }
}