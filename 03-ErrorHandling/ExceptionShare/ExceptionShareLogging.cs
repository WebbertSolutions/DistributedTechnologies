using Microsoft.Extensions.Logging;

namespace ExceptionShare
{
	static public partial class ExceptionShareLogging
	{
		// It may seem strange that the Exception is not defined anywhere.
		// The first exception is appended to the message.
		// Any additional would then be manually tacked on to the message
		// Example: https://docs.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator#indeterminate-parameter-order

		[LoggerMessage(
			EventId = 0, 
			EventName = "ExceptionToJsonMessage", 
			Level = LogLevel.Debug, 
			Message = "Message: ", 
			SkipEnabledCheck = true)]


		// Ignore the Red squiggly line here - It will compile just fine
		static public partial void JsonHandler(ILogger logger, Exception ex);
	}
}

