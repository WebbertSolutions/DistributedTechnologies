using System.Security.Claims;

//
//  I got this test from the following Microsoft reference application
//	https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Web/WebMVC/Controllers/OrderController.cs
//

namespace TestBuilderProject
{
	public class OrderService
	{
		private readonly IOrderingService orderSvc;
		private readonly IBasketService basketSvc;
		private readonly IIdentityParser<ApplicationUser> appUserParser;

		public OrderService(IOrderingService orderSvc, IBasketService basketSvc, IIdentityParser<ApplicationUser> appUserParser)
		{
			this.orderSvc = orderSvc;
			this.basketSvc = basketSvc;
			this.appUserParser = appUserParser;
		}


		public async Task Create(ClaimsPrincipal principal)
		{
			var user = appUserParser.Parse(principal);
			var order = await basketSvc.GetOrderDraft(user.Id);
			var vm = orderSvc.MapUserInfoIntoOrder(user, order);
			vm.CardExpirationShortFormat();

			throw new FileNotFoundException("Some Random Message");
		}
	}
}