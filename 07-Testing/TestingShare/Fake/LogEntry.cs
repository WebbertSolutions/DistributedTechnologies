using Microsoft.Extensions.Logging;

namespace TestingShare
{
	/// <summary>
	/// 
	/// </summary>
	public class LogEntry
	{
		public LogLevel LogLevel { get; }
		public string Message { get; }
		public EventId EventId { get; }
		public Exception Exception { get; }


		public LogEntry(LogLevel logLevel, string message, EventId eventId, Exception exception)
		{
			LogLevel = logLevel;
			EventId = eventId;
			Exception = exception;
			Message = message;
		}
	}
}