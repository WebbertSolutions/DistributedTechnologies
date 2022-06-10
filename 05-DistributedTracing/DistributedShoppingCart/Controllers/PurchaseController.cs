using System.Diagnostics;
using DistributedShare;
using Microsoft.AspNetCore.Mvc;

namespace DistributedShoppingCart.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PurchaseController : ControllerBase    // TODO: Distributed Tracing - 04
	{
		[HttpGet(Name = "Purchase")]
		public async Task Get(int cartID)
		{
			//using var activity = ApplicationTraceSource.Instance.StartActivity("Submit Order -1", ActivityKind.Producer);

			var activity = Activity.Current;
			if (activity != null)
			{
				activity.DisplayName = "Bob's Your Uncle";
				activity.AddEvent(new ActivityEvent("Purchase Receiver"));
			}

			var items = new List<CartItem>
			{
				new CartItem(1, "Bike", 1, 99.95M),
				new CartItem(2, "Softball", 5, 5.45M)
			};

			var cart = new Cart(cartID);
			cart.Items.AddRange(items);

			activity?.AddEvent(new ActivityEvent("Send message to RabbitMQ"));
			await Rabbit.Publish(cart, Rabbit.RoutingKey);
		}
	}
}