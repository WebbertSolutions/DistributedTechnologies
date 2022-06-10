using Microsoft.AspNetCore.Mvc;

namespace CustomSwagger.Controllers
{
	public partial class WeatherForecastController : ControllerBase
	{
		[ApiVersion("2.1")]         // TODO: API Version - 07 - Custom Swagger
		[HttpGet(Name = "GetWeatherForecastV2.1")]
		public IEnumerable<WeatherForecast> GetV21()
		{
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