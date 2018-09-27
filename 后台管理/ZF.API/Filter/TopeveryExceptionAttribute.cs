using System;
using System.Web.Mvc;
using log4net;
using ZF.Infrastructure.Entity;

namespace ZF.API.Filter
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TopeveryExceptionAttribute:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                string msgTemplate = "在执行 controller[{0}] 的 action[{1}] 时产生异常";
                LogManager.GetLogger("TopeveryExceptionAttribute").Error(string.Format(msgTemplate, controllerName, actionName), filterContext.Exception);
            }

            //如果是ajax访问
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new WrapEntity()
                    {
                        Success = false,
                        Error = filterContext.Exception.Message
                    },
                     JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                //当结果为json时，设置异常已处理
                filterContext.ExceptionHandled = true;
            }
            else
            {
                base.OnException(filterContext);
            }

        }

    }
}