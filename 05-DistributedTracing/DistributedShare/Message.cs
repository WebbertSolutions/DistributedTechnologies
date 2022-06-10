using System.Diagnostics;
using System.Text.Json.Serialization;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace DistributedShare
{
	//
	// Personally, I believe data should be wrapped in an envelope
	// Take a look at https://cloudevents.io/ 
	//

	public class Message : IJsonOnSerializing       // TODO: Distributed Tracing - 07
	{
		private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

		public string Type { get; set; }
		public DateTime DateTime { get; set; }
		public string Version { get; set; }
		public object Data { get; set; }
		public ActivityContextMessage? ActivityContextMessage { get; set; }


		public Message(object data, string version = "1.0")
		{
			Data = data;
			Version = version;
			Type = data.GetType().Name;
			DateTime = DateTime.UtcNow;
		}


		public PropagationContext GetParentContext()
		{
			var parentContext = Propagator.Extract(default, this, ExtractTraceFromMessage);
			Baggage.Current = parentContext.Baggage;

			return parentContext;
		}


		public void OnSerializing()
		{
			var currentActivity = Activity.Current;
			if (currentActivity == null)
				return;

			var currentBaggage = Baggage.Current;
			var activityBaggage = Activity.Current?.Baggage;

			var currentList = (currentBaggage.Count > 0 && activityBaggage != null)
				? currentBaggage.GetBaggage().Union(activityBaggage).ToList()
				: activityBaggage ?? currentBaggage.GetBaggage();

			Baggage baggage = new Baggage();
			baggage = baggage.SetBaggage(currentList);

			var context = new PropagationContext(currentActivity.Context, baggage);
			Propagator.Inject(context, this, InjectTraceIntoMessage);
		}


		private void InjectTraceIntoMessage(Message message, string key, string value)
		{
			message.ActivityContextMessage ??= new ActivityContextMessage();
			message.ActivityContextMessage[key] = new[] { value };
		}


		private IEnumerable<string?> ExtractTraceFromMessage(Message message, string key)
		{
			return (message.ActivityContextMessage != null &&
				message.ActivityContextMessage.TryGetValue(key, out var value) &&
				value != null)
					? value
					: Enumerable.Empty<string?>();
		}
	}


	public class ActivityContextMessage : Dictionary<string, IEnumerable<string?>> { }
}
