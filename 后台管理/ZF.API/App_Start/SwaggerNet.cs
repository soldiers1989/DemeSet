using System;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Routing;
using Swagger.Net;

namespace ZF.API 
{
    /// <summary>
    /// 
    /// </summary>
    public static class SwaggerNet 
    {
        /// <summary>
        /// 
        /// </summary>
        public static void PreStart() 
        {
            RouteTable.Routes.MapHttpRoute(
                name: "SwaggerApi",
                routeTemplate: "api/docs/{controller}",
                defaults: new { swagger = true }
            );            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void PostStart() 
        {
            var config = GlobalConfiguration.Configuration;

            config.Filters.Add(new SwaggerActionFilter());
            
            try
            {
                config.Services.Replace(typeof(IDocumentationProvider),
                    new XmlCommentDocumentationProvider(HttpContext.Current.Server.MapPath("~/bin/ZF.API.XML")));
            }
            catch (FileNotFoundException)
            {
                throw new Exception("Please enable \"XML documentation file\" in project properties with default (bin\\ZF.API.XML) value or edit value in App_Start\\SwaggerNet.cs");
            }
        }
    }
}