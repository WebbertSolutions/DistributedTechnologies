namespace Logging.LogMessages
{
	static public partial class WeatherLogDefinitions
	{
		// It may seem strange that the Exception is not defined anywhere.
		// The first exception is appended to the message.
		// Any additional would then be manually tacked on to the message
		// Example: https://docs.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator#indeterminate-parameter-order

		[LoggerMessage(
			EventId = 0,
			EventName = "LogRandomMessage",
			Level = LogLevel.Warning,
			Message = "Source Code Generator - An exception was thrown by {user} on {date}",
			SkipEnabledCheck = true)]


		// Ignore the Red squiggly line here - It will compile just fine
		static public partial void LogRandomMessage(ILogger logger, string user, DateTime date, Exception? ex = null);


		[LoggerMessage(
			EventId = 1,
			EventName = "My Log 2",
			Level = LogLevel.Trace,
			Message = "Something really random: {id} {user}",
			SkipEnabledCheck = false)]


		// Ignore the Red squiggly line here - It will compile just fine
		static public partial void MyLogger2(ILogger logger, int id, string user, Exception? ex = null);
	}
}

