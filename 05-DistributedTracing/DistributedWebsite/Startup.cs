using DistributedShare;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DistributedWebsite
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

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddHttpClient<PaymentClient>(nameof(PaymentClient), client =>
			{
				client.BaseAddress = new Uri("https://localhost:44393");
			});

			AddLogging(services);
			AddTracing(services);
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
				// Project Generated Code - Refactored into Extension
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}


		static private void AddLogging(IServiceCollection services)
		{
			// Configure Trace Information to be automatically added to structured logs
			services.AddLogging(logging =>
			{
				logging.Configure(options =>
				{
					options.ActivityTrackingOptions =
						ActivityTrackingOptions.SpanId |
						ActivityTrackingOptions.TraceId |
						ActivityTrackingOptions.ParentId;
				});
			});
		}


		static private void AddTracing(IServiceCollection services)
		{
			const string appName = "WebSite";

			ApplicationTraceSource.Configure(appName, "1.0.0");
			string[] sources = { appName };

			var zipkinEndpoint = new Uri("http://localhost:9411/api/v2/spans");
			services.AddOpenTelemetryTracing(builder =>
			{
				builder
					.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName))
					.AddSource(sources)
					.AddZipkinExporter(config =>
					{
						config.Endpoint = zipkinEndpoint;
					})
					.AddAspNetCoreInstrumentation()
					.AddHttpClientInstrumentation();
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
