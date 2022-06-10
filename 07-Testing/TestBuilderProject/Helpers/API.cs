namespace TestBuilderProject
{
	public static class API
	{

		public static class Purchase
		{
			public static string AddItemToBasket(string baseUri) => $"{baseUri}/basket/items";
			public static string UpdateBasketItem(string baseUri) => $"{baseUri}/basket/items";

			public static string GetOrderDraft(string baseUri, string basketId) => $"{baseUri}/order/draft/{basketId}";
		}
	}
}