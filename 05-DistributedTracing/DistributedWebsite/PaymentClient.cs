namespace DistributedWebsite
{
	public class PaymentClient
	{
		private readonly HttpClient httpClient;


		public PaymentClient(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task Purchase(int cartID)
		{
			const string endpoint = "Purchase";
			var request = new HttpRequestMessage(HttpMethod.Get, $"{endpoint}?cartID={cartID}");

			_ = await httpClient.SendAsync(request);
		}
	}
}
