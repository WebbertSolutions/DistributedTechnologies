using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingShare;

namespace TestBuilderProject.Tests
{
	[TestClass]
	public class BasketServiceTests     // TODO: Testing - 04
	{

		[TestMethod]
		public async Task GetBasket_Successful()
		{
			//
			// Arrange 
			//

			// HttpClient
			var expirationDate = DateTime.Now;
			var returnObject = CreateOrderJson(expirationDate);
			var httpClientBuilder = new HttpClientBuilder();
			httpClientBuilder.AddDependency(new Uri("https://localhost/builder"));
			httpClientBuilder.AddDependency("returnObject", returnObject);
			var httpClient = httpClientBuilder.BuildOK();

			// Application Settings
			var appSettings = Options.Create(new AppSettings());

			// Logger
			var logger = new LoggerFake<BasketService>();

			// Service
			var builder = BasketServiceBuilder.CreateBasketServiceBuilder(httpClient, appSettings, logger);
			var service = builder.Build();

			//
			//	Act
			//
			var basketId = "cart-1";
			var order = await service.GetOrderDraft(basketId);

			//
			//	Assert
			//
			Assert.IsNotNull(order);
			Assert.AreEqual(expirationDate, order.CardExpiration);

			logger.AssertContainsMessage("Successfully retrieved the Order");
		}


		[ExpectedException(typeof(HttpRequestException))]
		[TestMethod]
		public async Task GetBasket_Throws_Timeout()
		{
			//
			// Arrange 
			//

			// HttpClient
			var httpClientBuilder = HttpClientBuilder.CreateHttpClientBuilder("https://localhost/builder", WebExceptionStatus.Timeout, "A Timeout occurred");
			var httpClient = httpClientBuilder.BuildException();

			// Application Settings
			var appSettings = Options.Create(new AppSettings());

			// Logger
			var logger = new LoggerFake<BasketService>();

			// Service
			var builder = BasketServiceBuilder.CreateBasketServiceBuilder(httpClient, appSettings, logger);
			var service = builder.Build();

			//
			//	Act
			//
			var basketId = "cart-1";
			_ = await service.GetOrderDraft(basketId);

			//
			//	Assert
			//
			Assert.Fail("Should never get here !!");
		}



		//
		//	Helpers
		//
		static private string CreateOrderJson(DateTime expirationDate)
		{
			var order = new Order { CardExpiration = expirationDate };
			return JsonSerializer.Serialize(order);
		}
	}
}