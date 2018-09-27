using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite.Areas.jjs
{
    public class jjsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "jjs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
          
            context.MapRoute(
                "jjs_default",
                "jjs/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}