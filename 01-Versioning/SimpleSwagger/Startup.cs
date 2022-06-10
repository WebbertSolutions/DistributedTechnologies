using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SwaggerShare;

namespace SimpleSwagger
{
	public class Startup
	{
        public IConfigurationRoot Configuration { get; }


        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //
            // Dependency Injection
            //


            //
            // Services
            //

            services.AddControllers();

            // TODO: API Version - 01 - Simple Swagger
            var defaultApiVersion = new ApiVersion(1, 0);
            ApiVersioning.ConfigureServices(services, defaultApiVersion);


            // Project Generated Code  - Refactored into Extension
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();

            // TODO: API Version - 02 - Simple Swagger
            SwaggerExtensions.ConfigureServices(services);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="apiProvider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiProvider)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                // Project Generated Code - Refactored into Extension
                //app.UseSwagger();
                //app.UseSwaggerUI();

                SwaggerExtensions.Configure(app, apiProvider);  // TODO: API Version - 03 - Simple Swagger
            }

            app.UseHttpsRedirection();

            app.UseApiVersioning(); // TODO: API Version - 04 - Simple Swagger

            app.UseRouting();

            app.UseAuthorization();

            // Project Generated Code - Replaced with UseEndpoints
            //app.MapControllers();

            app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
    }

    //
    //  Helpers
    //

    static public class StartupExtension
    {
        private static Startup? startup;

        static public WebApplicationBuilder CreateStartup(this WebApplicationBuilder builder)
        {
            startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            return builder;
        }

        static public WebApplication ConfigureStartup(this WebApplication app, WebApplicationBuilder builder)
        {
            var apiProvider = app.Services.GetService<IApiVersionDescriptionProvider>()!;

            startup!.Configure(app, builder.Environment, apiProvider);

            return app;
        }
    }
}
