using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestBuilderProject
{
	//
	//  I got this test from the following Microsoft reference application
	//	https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Web/WebMVC/Services/BasketService.cs
	//

	public class BasketService : IBasketService
	{
		private readonly IOptions<AppSettings> settings;
		private readonly HttpClient apiClient;
		private readonly ILogger<BasketService> logger;
		private readonly string purchaseUrl;

		public BasketService(HttpClient httpClient, IOptions<AppSettings> settings, ILogger<BasketService> logger)
		{
			apiClient = httpClient;
			this.settings = settings;
			this.logger = logger;

			purchaseUrl = $"{this.settings.Value.PurchaseUrl}/api/v1";
		}


		public async Task<Order> GetOrderDraft(string basketId)
		{
			var uri = API.Purchase.GetOrderDraft(purchaseUrl, basketId);

			var responseString = await apiClient.GetStringAsync(uri);

			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var response = JsonSerializer.Deserialize<Order>(responseString, options);

			logger.LogTrace("Successfully retrieved the Order");

			return response!;
		}
	}
}