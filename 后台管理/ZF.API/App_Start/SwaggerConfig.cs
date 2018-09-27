using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using Swashbuckle.Application;
using WebActivatorEx;
using ZF.API;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace ZF.API
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "ZF.API");
                        c.ResolveConflictingActions(x => x.First());
                        var baseDirectiory = AppDomain.CurrentDomain.BaseDirectory;

                        var commentsFileName = "bin//ZF.API.XML";
                        var commentsFile = Path.Combine(baseDirectiory, commentsFileName);
                        c.IncludeXmlComments(commentsFile);


                        var commentsFileName1 = "bin//ZF.Application.XML";
                        var commentsFile1 = Path.Combine(baseDirectiory, commentsFileName1);
                        c.IncludeXmlComments(commentsFile1);

                    })
                .EnableSwaggerUi(c =>
                    {
                        
                    });
        }
    }
}
