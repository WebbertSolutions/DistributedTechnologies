using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExceptionShare
{
	static public class DependencyInjection
	{
		static public void ConfigureServices(IServiceCollection services)
		{
			//
			// Transient
			//
			services
				.AddTransient<ExceptionToJsonHandler>()     // TODO: Logging - 05
				;
		}
	}
}
