using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks
{
	public class RandomHealthCheck : IHealthCheck           // TODO: Health Check - 07
	{
		private static readonly Random random = new Random();


		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			var value = random.Next(0, 10);
			var msg = $"Value: {value}";

			return (value % 3 == 0)
				? Task.FromResult(HealthCheckResult.Unhealthy(msg))
				: Task.FromResult(HealthCheckResult.Healthy(msg));
		}
	}
}