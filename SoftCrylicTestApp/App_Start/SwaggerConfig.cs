using System.Web.Http;
using WebActivatorEx;
using SoftCrylicTestApp;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.ComponentModel;
using System.Linq;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace SoftCrylicTestApp
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.Schemes(new[] { "http", "https" });
                        c.SingleApiVersion("v1", "SoftCrylicTestApp");
                        c.BasicAuth("basic").Description("Basic HTTP Authentication");
                        c.ApiKey("Authorization").Description("API Key Authentication").Name("Authorization").In("header");
                        c.IgnoreObsoleteActions();
                        c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();
                        c.GroupActionsBy(apiDesc => {
                            DisplayNameAttribute attr = apiDesc
                                .GetControllerAndActionAttributes<DisplayNameAttribute>()
                                .FirstOrDefault();
                            return attr?.DisplayName ?? apiDesc.ActionDescriptor.ControllerDescriptor.ControllerName;
                        });
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("SoftCrylic Assessment");
                        c.EnableApiKeySupport("Authorization", "org1_abcd123");
                    });
        }
    }

    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters != null)
            {
                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "API Key Authentication",
                    required = false,
                    type = "string"
                });
            }
        }
    }
}