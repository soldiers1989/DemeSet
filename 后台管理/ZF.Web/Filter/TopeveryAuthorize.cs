using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace ZF.Web.Filter
{
    /// <summary>
    /// 验证过滤器
    /// </summary>
    public class TopeveryAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //从Cookie中读取出Json串并反序列化成实体
            //取出Cookie对象
            HttpCookie userInfoCookie = HttpContext.Current.Request.Cookies.Get("UserInfo");
           // return true;
            return userInfoCookie != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["loginUrl"]);
        }
    }
}