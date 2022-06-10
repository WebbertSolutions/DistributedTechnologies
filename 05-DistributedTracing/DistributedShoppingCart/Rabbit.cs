using System.Diagnostics;
using System.Text;
using System.Text.Json;
using DistributedShare;
using OpenTelemetry;
using RabbitMQ.Client;

namespace DistributedShoppingCart
{
	//  This code was taken from the RabbitMQ tutorial section and is in no way the best
	//  practice on how to set it up and use it in a production environment.
	//  https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html

	public class Rabbit
	{
		public const string RoutingKey = "hello";
		public const string RabbitSource = nameof(RabbitSource);


		private static readonly ActivitySource RabbitActivitySource = new(RabbitSource);


		static private readonly ConnectionFactory connectionFactory;
		static private readonly IConnection connection;
		static private readonly IModel channel;

		static Rabbit()
		{
			connectionFactory = new ConnectionFactory()
			{
				HostName = "localhost"
			};

			connection = connectionFactory.CreateConnection();
			channel = connection.CreateModel();

			channel.QueueDeclare(
				queue: RoutingKey,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null);
		}

		static public Task Publish(object obj, string routingKey)   // TODO: Distributed Tracing - 05
		{
			return Task.Run(() =>
			{
				Baggage.Current = Baggage.Current
					.SetBaggage("TenantID", "abc-1234")
					.SetBaggage("UserID", "def-5678");

				using var activity = StartActivity();
				SetActivityTags(activity, routingKey);

				activity?.AddBaggage("TenantID", "xxx-123");
				activity?.AddBaggage("UserID", "yyy-456");

				var message = CreateMessage(obj);

				channel.BasicPublish(
					exchange: "",
					routingKey: routingKey,
					basicProperties: null,
					body: message);
			});
		}


		static private byte[] CreateMessage(object obj)
		{
			var message = new Message(obj);
			string json = JsonSerializer.Serialize(message);
			return Encoding.UTF8.GetBytes(json);
		}

		static private Activity? StartActivity()
		{
			return RabbitActivitySource.StartActivity("RabbitMQ Publish", ActivityKind.Producer);
		}

		static private void SetActivityTags(Activity? activity, string routingKey)
		{
			activity?.SetTag("messaging.system", "rabbitmq");
			activity?.SetTag("messaging.destination_kind", "queue");
			activity?.SetTag("messaging.rabbitmq.queue", routingKey);
		}
	}
}
