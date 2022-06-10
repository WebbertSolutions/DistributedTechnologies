using Logging.LogMessages;
using Microsoft.AspNetCore.Mvc;

namespace Logging.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly NLog.Logger nlogLogger = NLog.LogManager.GetCurrentClassLogger();

		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> netCoreLogger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			netCoreLogger = logger;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			CreateLogMessage();

			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}


		private void CreateLogMessage()
		{
			// TODO: Logging - 03
			try
			{
				throw new ApplicationException("some random message");
			}
			catch (Exception ex)
			{
				// NLog
				nlogLogger.Warn(ex, "NLog direct - An exception was thrown by {user} on {date}", "NLog User", DateTime.Now);
				// https://github.com/NLog/NLog/wiki/Message-Layout-Renderer#layout-options
				// https://github.com/NLog/NLog/wiki/How-to-use-structured-logging#output-captured-properties


				// .NET Core Logger
				netCoreLogger.LogDebug(ex, ".NET Core - An exception was thrown by {user} on {date}", "Super User", DateTime.Now);

				// High Performant .NET Core Logger
				LogRandomMessage(netCoreLogger, "Super User", DateTime.Now, ex);
				// https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging

				// Source Code Generator
				WeatherLogDefinitions.LogRandomMessage(netCoreLogger, "Super User", DateTime.Now);
			}
		}


		//
		// Logger Templates
		//
		private static readonly Action<ILogger, string, DateTime, Exception> LogRandomMessage =
			LoggerMessage.Define<string, DateTime>(LogLevel.Error, 0, $"{nameof(WeatherForecastController)} - {nameof(CreateLogMessage)}: " +
				".NET Core Performant - An exception was thrown by {user} on {date}");

		/*
	
		.NET 6.0 - Source Code Generator - this will replace the Template directly above
		 
		public partial class MyFavoriteLoggerClass
		{
			[LoggerMessage(0, LogLevel.Error, 
				"$"{nameof(WeatherForecastController)} - {nameof(CreateLogMessage)}: .NET Core Performant - An exception was thrown by {user} on {date}")]
			partial void LogRandomMessage(string user, DateTime date);
		}

		MyFavoriteLoggerClass.LogRandomMessage(netCoreLogger, "Super User", DateTime.Now, ex);

		 */
	}
}