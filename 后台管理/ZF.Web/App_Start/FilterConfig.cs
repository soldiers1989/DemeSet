using System.Web.Mvc;
using ZF.Web.Filter;

namespace ZF.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //添加验证过滤器
            filters.Add(new TopeveryAuthorize());
            //错误过滤器
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TopeveryExceptionAttribute());
            //包装过滤器
        }
    }
}