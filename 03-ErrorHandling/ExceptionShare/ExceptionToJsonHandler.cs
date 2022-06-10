using System.Net;
using System.Text;
using System.Text.Json;
using DomainExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExceptionShare
{
	/// <summary>
	/// This class provides a way to capture things thrown out of the service and provide a returned object that
	/// is appropriate as well as logs the exception.
	/// </summary>
	public class ExceptionToJsonHandler : IMiddleware       // TODO: Error Handling - 04
	{
		private readonly ILogger<ExceptionToJsonHandler> logger;
		static private readonly Dictionary<Type, HttpStatusCode> Map = CreateExceptionMap();


		public ExceptionToJsonHandler(ILogger<ExceptionToJsonHandler> logger)
		{
			this.logger = logger;
		}


		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				ExceptionShareLogging.JsonHandler(logger, ex);
				await HandleExceptionAsync(context, ex);
			}
		}


		private static Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			var statusCode = GetStatusCode(ex);

#if DEBUG
			var errorText = ex.Message;

			var details = ex.StackTrace;
			var instance = string.Empty;
			var status = (int)statusCode;
			var title = errorText;
			var type = ex.GetType().Name;
#else
			var errorText = (statusCode == HttpStatusCode.InternalServerError)
				? "Internal Server Error"
				: ex.Message;

			var details = string.Empty;
			var instance = string.Empty;
			var status = (int)statusCode;
			var title = errorText;
			var type = statusCode == HttpStatusCode.InternalServerError
				? errorText
				: ex.GetType().Name;
#endif

			var pd = new ProblemDetails()
			{
				Detail = details,
				Instance = instance,
				Status = status,
				Title = title,
				Type = type
			};

			context.Response.StatusCode = (int)statusCode;
			var body = JsonSerializer.Serialize(pd);

			return context.Response.WriteAsync(body, Encoding.UTF8);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		static private HttpStatusCode GetStatusCode(Exception ex)
		{
			return Map.TryGetValue(ex.GetType(), out var statusCode)
				? statusCode
				: HttpStatusCode.InternalServerError;
		}


		/// <summary>
		/// Mapping of acceptable exceptions to HTTP Status Codes
		/// </summary>
		/// <returns></returns>
		static private Dictionary<Type, HttpStatusCode> CreateExceptionMap()
		{
			return new Dictionary<Type, HttpStatusCode>
			{
				{ typeof(BadRequestException),           HttpStatusCode.BadRequest},
				{ typeof(NotAuthorizedException),        HttpStatusCode.Forbidden},
				{ typeof(ObjectDoesNotExistException),   HttpStatusCode.NotFound},
				{ typeof(NotAuthenticatedException),     HttpStatusCode.Unauthorized}
			};
		}


		static public void AddExceptionMaps(IEnumerable<KeyValuePair <Type, HttpStatusCode>> additionalMapping)
		{
			if (!additionalMapping.Any())
				return;

			foreach (var map in additionalMapping)
				Map.TryAdd(map.Key, map.Value);
		}
	}
}
