using System.Net;
using HealthCheckShare;
using ConstantsHC = HealthCheckShare.HealthCheckExtension;

namespace HealthChecks
{
	static public class HealthCheckExtension                // TODO: Health Check - 05
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <returns></returns>
		static public IServiceCollection ConfigureServicesHealthChecks(this IServiceCollection services, IConfiguration configuration)
		{
			var healthCheckBuilder = services.AddHealthChecks();

			//
			//	API Version
			//
			healthCheckBuilder.AddCheck<ApiVersionHealthCheck>("API Version", tags: ConstantsHC.TagReadyLive);


			//
			//	Random Health Check
			//

			healthCheckBuilder
				.AddCheck<RandomHealthCheck>(name: "Randomizer", tags: ConstantsHC.TagLive);


			//
			//	Ensure the CustomSwagger Website is available
			//
			{
				var CustomSwaggerWebsite = "https://localhost:7232/swagger";   // Custom Swagger Web API

				healthCheckBuilder.AddUrlGroup((options) =>
				{
					options
						.ExpectHttpCode((int)HttpStatusCode.OK)
						.AddUri(new Uri(CustomSwaggerWebsite));

					options.UseTimeout(TimeSpan.FromSeconds(5));
				},
				name: "Custom Swagger Website",
				tags: ConstantsHC.TagReady);
			}


#if DEBUG
			// https://localhost:44346/healthchecks-ui#/healthchecks

			// This is part of the DEV Tool we don't want exposed in production
			services
				.AddHealthChecksUI(options =>
				{
					options.SetEvaluationTimeInSeconds(10);	// time in seconds between check 
					options.SetApiMaxActiveRequests(1);		// api requests concurrency  
					options.AddHealthCheckEndpoint("My Health Checks", ConstantsHC.DevHealthEndpointRoot);
				})
				.AddInMemoryStorage();
#endif

			return services;
		}


		static public IApplicationBuilder ConfigureHealthChecks(this IApplicationBuilder app)
		{
#if DEBUG
			app.UseHealthChecksUI();
#endif

			return app;
		}
	}
}