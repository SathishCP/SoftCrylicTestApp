using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SoftCrylicTestApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Clear();
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented };
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = true;
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            config.Formatters.OfType<System.Net.Http.Formatting.JsonMediaTypeFormatter>().FirstOrDefault();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.SuppressHostPrincipal();
        }
    }
}
