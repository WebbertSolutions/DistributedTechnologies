using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.SwaggerGen;

//
//  Enable XML  project -> Properties -> Build
//      -> Errors and warnings
//          -> Suppress specific warnings
//              Add 1591 to ignore warning -> "Missing XML comment for publicly visible type or member"
//      -> Output
//          -> Documentation file -> [X] Generate a file containing API documentation
//          -> XML documentation file path -> optional
//

namespace SwaggerShare
{
    static public class SwaggerExtensions
    {
        static private readonly string? AssemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "unknown";

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="services">The collection of services to configure the application with.</param>
        static public void ConfigureServices(IServiceCollection services, bool isCustom = false)
        {
            if (isCustom)
            {
                services.AddTransient<IConfigureOptions<SwaggerGenOptions>, CustomConfigureSwaggerOptions>();
                services.AddSingleton<IApiVersionDescriptionProvider, MyCustomApiVersionDescriptionProvider>();
            }
            else
                services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    var xmlFile = XmlCommentsFilePath;
                    if (File.Exists(xmlFile))
                        options.IncludeXmlComments(xmlFile);
                });
        }

        /// <summary>
        /// Configures the application using the provided builder, hosting environment, and API version description provider.
        /// </summary>
        /// <param name="app">The current application builder.</param>
        /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
        static public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            // TODO: Swagger - 03

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = AssemblyName + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
