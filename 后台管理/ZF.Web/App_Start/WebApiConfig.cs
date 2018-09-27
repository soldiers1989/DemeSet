﻿using System.Web.Http;
using Newtonsoft.Json.Serialization;
using ZF.Web.Filter;

namespace ZF.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );


            config.Filters.Add(new TopeveryApiAuthorize());
            config.Filters.Add(new TopeveryExceptionApiAttribute());
            config.Filters.Add(new TopeveryWrapperApiAttribute());


            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //// 对 JSON 数据使用混合大小写。驼峰式,但是是javascript 首字母小写形式.
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new  CamelCasePropertyNamesContractResolver();

            // 对 JSON 数据使用混合大小写。跟属性名同样的大小.输出
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver();

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Insert(
                0, new JsonDateTimeConverter());

        }
    }
}