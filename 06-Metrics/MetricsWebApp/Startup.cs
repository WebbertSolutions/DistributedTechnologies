//using App.Metrics;
//using App.Metrics.Builder;
//using App.Metrics.Registry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;

namespace MetricsWebApp
{
	public class Startup
	{
		const string appName = "MetricsWebApp";

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
			services.AddSingleton<WebAppMetrics>();


			//
			// Services
			//
			// Add services to the container.

			services.AddControllers();
			services.AddMetrics();
			services.AddMetricsReportingHostedService();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			AddMetrics(services);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="apiProvider"></param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Configure the HTTP request pipeline.
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			
			app.UseMetricServer();

			app.UseRouting();

			app.UseHttpMetrics(); // Make sure this is called after app.UseRouting()			

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}


		private void AddMetrics(IServiceCollection services)
		{
			string[] meterNames = { WebAppMetrics.Name };

			services.AddOpenTelemetryMetrics(builder =>
			{
				builder
					.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName))
					.AddMeter(meterNames)
					.AddPrometheusExporter(options => {
						options.StartHttpListener = true;
						// 9464 is the default value and therefore is commented out
						//options.HttpListenerPrefixes = new string[] { "http://localhost:9464/" };
					})
					.AddHttpClientInstrumentation()
					.AddAspNetCoreInstrumentation()
					;
			});

			var metrics = AppMetrics.CreateDefaultBuilder() // configure a reporter
                                    .Build();

			services.AddMetrics(metrics);
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
