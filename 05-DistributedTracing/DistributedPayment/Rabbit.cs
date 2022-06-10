using System.Diagnostics;
using System.Text;
using System.Text.Json;
using DistributedShare;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DistributedPayment
{
	//  This code was taken from the RabbitMQ tutorial section and is in no way the best
	//  practice on how to set it up and use it in a production environment.
	//  https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html

	static public class Rabbit
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

		// TODO: Distributed Tracing - 06

		static public void RegisterCallback(Func<object, Task> callback, string routingKey)
		{
			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += async (model, ea) =>
			{
				await Task.Delay(2000); // Similate delay before getting the message

				var body = ea.Body.ToArray();
				var message = GetMessage(Encoding.UTF8.GetString(body));

				var parentContext = message.GetParentContext();

				using var activity = StartActivity(parentContext);

				await callback(message.Data);
			};

			channel.BasicConsume(queue: routingKey, autoAck: true, consumer: consumer);
		}


		static private Message GetMessage(string data)
		{
			return JsonSerializer.Deserialize<Message>(data)!;
		}


		static private Activity? StartActivity(PropagationContext parentContext)
		{
			return RabbitActivitySource.StartActivity("RabbitMQ Processor", ActivityKind.Consumer, parentContext.ActivityContext);
		}
	}
}
