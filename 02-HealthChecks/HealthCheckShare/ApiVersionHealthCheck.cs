using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckShare
{
	public class ApiVersionHealthCheck : IHealthCheck           // TODO: Health Check - 06
	{
		static public string? Version { get; private set; }


		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			if (Version == null)
				Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "Unknown";

			return Task.FromResult(HealthCheckResult.Healthy(Version));
		}
	}
}