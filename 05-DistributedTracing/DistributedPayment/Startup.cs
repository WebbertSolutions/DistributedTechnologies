using DistributedShare;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DistributedPayment
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
			services.AddSingleton<HelloRabbitMessageProcessor>();

			//
			// Services
			//

			services.AddControllers();
			services.AddHttpClient();

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

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

			ConfigureRabbitProcessors(app.ApplicationServices);
		}


		public void AddTracing(IServiceCollection services)
		{
			const string appName = "Payment";

			ApplicationTraceSource.Configure(appName, "1.0.0");
			string[] sources = { appName, Rabbit.RabbitSource };

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

		static private void ConfigureRabbitProcessors(IServiceProvider provider)
		{
			var processor = provider.GetService<HelloRabbitMessageProcessor>()!;
			Rabbit.RegisterCallback(processor.Callback, Rabbit.RoutingKey);
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
