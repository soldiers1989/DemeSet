using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using log4net;
using ZF.Infrastructure.Entity;

namespace ZF.API.Filter
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TopeveryExceptionApiAttribute: ExceptionFilterAttribute

    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            string controllerName = (string)actionExecutedContext.ActionContext.ControllerContext.RouteData.Values["controller"];
            string actionName = (string)actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            string msgTemplate = "在执行 apicontroller[{0}] 的 action[{1}] 时产生异常";
            LogManager.GetLogger("TopeveryExceptionApiAttribute").Error(string.Format(msgTemplate, controllerName, actionName), actionExecutedContext.Exception);

            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.NotImplemented,new WrapEntity()
                {
                    Success = false,
                    Error = "服务器不支持此请求"
                }) ;
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.RequestTimeout,new WrapEntity()
                {
                    Success = false,
                    Error = "请求超时"
                });
            }
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError,new WrapEntity()
                {
                    Success = false,
                    Error = actionExecutedContext.Exception.Message
                });
            }
        }
    }
}