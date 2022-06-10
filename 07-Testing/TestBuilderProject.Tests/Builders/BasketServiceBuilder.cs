using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestingShare;

namespace TestBuilderProject.Tests
{
	public class BasketServiceBuilder : MockBuilder		// TODO: Testing - 02
	{
		public BasketService Build()
		{
			var httpClient = dependencies.GetDependency<HttpClient>();
			var appSettings = dependencies.GetDependency<IOptions<AppSettings>>();
			var logger = dependencies.GetDependency<ILogger<BasketService>>();

			return new BasketService(httpClient, appSettings, logger);
		}

		static public BasketServiceBuilder CreateBasketServiceBuilder(
			HttpClient? httpClient, 
			IOptions<AppSettings>? appSettings,
			ILogger<BasketService>? logger)
		{
			var builder = new BasketServiceBuilder();

			if (httpClient != null)
				builder.AddDependency(httpClient);

			if (appSettings != null)
				builder.AddDependency(appSettings);

			if (logger != null)
				builder.AddDependency(logger);

			return builder;
		}
	}
}