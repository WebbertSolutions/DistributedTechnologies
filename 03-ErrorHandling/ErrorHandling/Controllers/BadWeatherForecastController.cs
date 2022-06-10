using DomainExceptions;
using Microsoft.AspNetCore.Mvc;

namespace ErrorHandling.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BadWeatherForecastController : ControllerBase
	{
		[HttpGet(Name = "GeBadtWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			throw new BadRequestException();
		}
	}
}