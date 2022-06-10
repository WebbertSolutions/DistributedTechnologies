using System.Text.Json;
using System.Diagnostics;
using DistributedShare;
using OpenTelemetry;

namespace DistributedPayment
{
	public class HelloRabbitMessageProcessor
	{
		public async Task Callback(object data)     // TODO: Distributed Tracing - 08
		{
			using var activity = ApplicationTraceSource.Instance.StartActivity("Submit Order", ActivityKind.Consumer);

			foreach (var bag in Baggage.Current.GetBaggage())
				activity?.AddTag(bag.Key, bag.Value);

			var json = data.ToString();
			if (json == null)
				return;

			var cart = JsonSerializer.Deserialize<Cart>(json);
			cart = null;

			await Task.Delay(1000);
		}
	}
}
