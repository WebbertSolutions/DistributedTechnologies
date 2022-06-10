/*
	https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0#customize-output

	https://stackoverflow.com/questions/58770795/performing-a-health-check-in-net-core-worker-service
	https://dzone.com/articles/monitoring-health-of-aspnet-core-background-servic
 */


using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckShare
{
	/// <summary>
	/// 
	/// </summary>
	static public class HealthCheckExtension            // TODO: Health Check - 04
	{
		public const string TAG_LIVE = "Live";
		public const string TAG_READY = "Ready";

		static public readonly string[] TagLive = new[] { TAG_LIVE };
		static public readonly string[] TagReady = new[] { TAG_READY };
		static public readonly string[] TagReadyLive = new[] { TAG_READY, TAG_LIVE };

		public const string HealthEndpointRoot = "/health";
		public const string LiveEndpoint = HealthEndpointRoot + "/live";
		public const string ReadyEndpoint = HealthEndpointRoot + "/ready";
		public const string DevHealthEndpointRoot = "/healthdev";

		/// <summary>
		/// Create the health check endpoints
		/// </summary>
		/// <param name="endpoints"></param>
		static public void CreateHealthEndpoints(this IEndpointRouteBuilder endpoints, bool includeMetadataEndpoint = false)
		{
			endpoints.MapHealthChecks(ReadyEndpoint, new HealthCheckOptions()
			{
				Predicate = (check) => check.Tags.Contains(TAG_READY)
			});

			endpoints.MapHealthChecks(LiveEndpoint, new HealthCheckOptions()
			{
				Predicate = (check) => check.Tags.Contains(TAG_LIVE)
			});

			if (includeMetadataEndpoint)
			{
				endpoints.MapHealthChecks(HealthEndpointRoot, new HealthCheckOptions()
				{
					Predicate = _ => true,
					ResponseWriter = WriteResponse
				});
			}

#if DEBUG
			// https:<url>/healthchecks-ui#/healthchecks
			// This endpoint is so Developers can used the UI
			endpoints.MapHealthChecks(DevHealthEndpointRoot, new HealthCheckOptions()
			{
				Predicate = _ => true,
				ResponseWriter = GetWriteHealthCheckUIResponse(),
				AllowCachingResponses = false
			});
#endif
		}

		private static Func<HttpContext, HealthReport, Task> GetWriteHealthCheckUIResponse()
		{
			return UIResponseWriter.WriteHealthCheckUIResponse;
		}

		private static Task WriteResponse(HttpContext context, HealthReport healthReport)
		{
			return Task.Delay(1);


			//context.Response.ContentType = "application/json; charset=utf-8";

			//var options = new JsonWriterOptions { Indented = true };

			//using var memoryStream = new MemoryStream();
			//using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
			//{
			//	jsonWriter.WriteStartObject();
			//	jsonWriter.WriteString("status", healthReport.Status.ToString());
			//	jsonWriter.WriteStartObject("results");

			//	foreach (var healthReportEntry in healthReport.Entries)
			//	{
			//		jsonWriter.WriteStartObject(healthReportEntry.Key);
			//		jsonWriter.WriteString("status",
			//			healthReportEntry.Value.Status.ToString());
			//		jsonWriter.WriteString("description",
			//			healthReportEntry.Value.Description);
			//		jsonWriter.WriteStartObject("data");

			//		foreach (var item in healthReportEntry.Value.Data)
			//		{
			//			jsonWriter.WritePropertyName(item.Key);

			//			JsonSerializer.Serialize(jsonWriter, item.Value,
			//				item.Value?.GetType() ?? typeof(object));
			//		}

			//		jsonWriter.WriteEndObject();
			//		jsonWriter.WriteEndObject();
			//	}

			//	jsonWriter.WriteEndObject();
			//	jsonWriter.WriteEndObject();
			//}

			//return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
		}
	}
}