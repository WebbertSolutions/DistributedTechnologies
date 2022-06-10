using System.Diagnostics;
using System.Net;
using DistributedShare;
using Microsoft.AspNetCore.Mvc;

namespace DistributedWebsite.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase       // TODO: Distributed Tracing - 03
	{
		[HttpGet(Name = "SubmitOrder")]
		public async Task<StatusCodeResult> Get(int cartID, [FromServices] PaymentClient paymentClient)
		{
			var parentContext = Activity.Current?.Context ?? default;
			using var activity = ApplicationTraceSource.Instance.StartActivity("Submit Order", ActivityKind.Producer, parentContext);
			{
				await Task.Delay(10);

				await PurchaseCart(paymentClient, cartID);
			}

			await SendEmail(parentContext);

			return StatusCode((int)HttpStatusCode.Accepted);
		}

		static private async Task SendEmail(ActivityContext parentContext)
		{
			using var activity = ApplicationTraceSource.Instance.StartActivity("Send Email", ActivityKind.Producer, parentContext);
			{
				await Task.Delay(1500);
			}
		}

		static private Task PurchaseCart(PaymentClient paymentClient, int cartID)
		{
			return paymentClient.Purchase(cartID);
		}
	}
}