using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace ZF.WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //添加验证过滤器
            //filters.Add(new TopeveryAuthorize());

            HttpContext.Current.Application["DefuleDomain"] = ConfigurationManager.AppSettings["DefuleDomain"];
            HttpContext.Current.Application["TitleName"] = ConfigurationManager.AppSettings["TitleName"];

        }
    }
}
