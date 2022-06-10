using Microsoft.AspNetCore.Builder;

namespace ExceptionShare
{
	static public class ExceptionHandlerMiddleware
	{
		public static IApplicationBuilder UseExceptionToJsonHandlerMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionToJsonHandler>();     // TODO: Error Handling - 03
		}
	}
}
