using Microsoft.AspNetCore.Mvc;

namespace MetricsWebApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		static private Random random = new Random();
		static private readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly WebAppMetrics webMetrics;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, WebAppMetrics webMetrics)
		{
			_logger = logger;
			this.webMetrics = webMetrics;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public async Task<IEnumerable<WeatherForecast>> Get()
		{
			// Increment counter
			webMetrics.IncrementRequestCounter();

			// Fake some processing time
			var sw = webMetrics.StartWeatherOperationDuration();
			await Task.Delay(random.Next(50, 500));
			webMetrics.StopWeatherOperationDuration(sw);

			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();			
		}
	}
}