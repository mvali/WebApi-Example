using ApiServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiServer.Logic
{
/*    // another way of adding versioning by extending the Startup method "ConfigureServices"
    public static class ApiVersioning
    {
        public static void AddApiVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(apiVersioningOptions =>
            {
                apiVersioningOptions.ApiVersionReader = new HeaderApiVersionReader(new string[] { "api-version" }); // It means version will be define in header.and header name would be "api-version".
                apiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;
                var apiVersion = new Version(Convert.ToString(configuration["DefaultApiVersion"]));
                apiVersioningOptions.DefaultApiVersion = new ApiVersion(apiVersion.Major, apiVersion.Minor);
                apiVersioningOptions.ReportApiVersions = true;
                apiVersioningOptions.UseApiBehavior = true; // It means include only api controller not mvc controller.
                apiVersioningOptions.Conventions.Controller<CartController>().HasApiVersion(apiVersioningOptions.DefaultApiVersion);
                apiVersioningOptions.Conventions.Controller<AlimentController>().HasApiVersion(apiVersioningOptions.DefaultApiVersion);
                apiVersioningOptions.ApiVersionSelector = new CurrentImplementationApiVersionSelector(apiVersioningOptions);
            });
            services.AddVersionedApiExplorer(); // It will be used to explorer api versioning and add custom text box in swagger to take version number.
        }
    }*/
}
