using Microsoft.AspNetCore.Mvc;

namespace ErrorHandling.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ZeroWeatherForecastController : ControllerBase
	{
		[HttpGet(Name = "GetZeroWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			throw new DivideByZeroException();
		}
	}
}