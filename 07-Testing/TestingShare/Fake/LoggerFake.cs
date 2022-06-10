using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingShare          // TODO: Testing - 03
{

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class LoggerFake<T> : LoggerFake, ILogger<T>
	{
	}


	/// <summary>
	/// 
	/// </summary>
	public class LoggerFake : ILogger
	{
		public List<LogEntry> Logs { get; } = new List<LogEntry>();
		public LogLevel LogLevel { get; set; } = LogLevel.Trace;

		//
		//  Took most of this from https://github.com/aspnet/Logging/blob/master/src/Microsoft.Extensions.Logging.Abstractions/NullLoggerOfT.cs
		//

		public LoggerFake()
		{
			Logs = new List<LogEntry>();
		}


		/// <inheritdoc />
		public IDisposable BeginScope<TState>(TState state)
		{
			return NullDisposable.Instance;
		}


		/// <inheritdoc />
		/// <remarks>
		/// This method ignores the parameters and does nothing.
		/// </remarks>
		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
		{
			Logs.Add(new LogEntry(logLevel, formatter(state, exception), eventId, exception));
		}


		/// <inheritdoc />
		public bool IsEnabled(LogLevel logLevel)
		{
			return (LogLevel <= logLevel);
		}


		public void AssertContainsMessage(string message)
		{
			var result = Logs
				.Where(log => log.Exception == null)
				.Any(log => log.Message.Contains(message));

			Assert.IsTrue(result);
		}


		public void AssertContainsException(Type type, string message = null)
		{
			var result = Logs
				.Where(log => log.Exception != null && log.Exception.GetType() == type)
				.Any(log => (message == null || log.Exception!.Message.Contains(message)));

			Assert.IsTrue(result);
		}


		public void AssertNoExceptions()
		{
			var result = Logs.All(log => log.Exception == null);

			Assert.IsTrue(result);
		}
	}
}