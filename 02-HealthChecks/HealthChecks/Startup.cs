using HealthCheckShare;

namespace HealthChecks
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

			services
				.ConfigureServicesHealthChecks(Configuration)		// TODO: Health Check - 01
				;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="apiProvider"></param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.ConfigureHealthChecks();							// TODO: Health Check - 02

			// Project Generated Code - Replaced with UseEndpoints
			//app.MapControllers();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.CreateHealthEndpoints(true);				// TODO: Health Check - 03
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
			startup!.Configure(app, builder.Environment);

			return app;
		}
	}
}
