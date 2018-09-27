using System.Web.Mvc;
using ZF.API.Filter;

namespace ZF.API
{
    /// <summary>
    /// 注册配置
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //错误过滤器
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TopeveryExceptionAttribute());
            //包装过滤器
        }
    }
}
