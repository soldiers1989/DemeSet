using System;
using System.Net.Http;
using System.Web.Http.Filters;
using ZF.Infrastructure.Entity;

namespace ZF.Web.Filter
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TopeveryWrapperApiAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                throw actionExecutedContext.Exception;
            }
            if (actionExecutedContext.ActionContext.Response.Content != null)
            {
                var result = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;
                var statusCode = actionExecutedContext.ActionContext.Response.StatusCode;
                var wrapResult = new WrapEntity(result);
                actionExecutedContext.Response = actionExecutedContext.Response.RequestMessage.CreateResponse(
                    statusCode, wrapResult);
            }
        }
    }
}