using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace SwaggerShare
{
	// https://dotnetcoretutorials.com/2017/01/17/api-versioning-asp-net-core/
	// https://www.hanselman.com/blog/aspnet-core-restful-web-api-versioning-made-easy

	// Decorate Controllers with one or more attributes
	//		[ApiVersion("1.0")] [ApiVersion("2.0")]  public class MyController : ControllerBase

	// To map a single method to a version you have 2 options
	//		- create a new Class with the new version attribute and add that method only
	//		- within existing class add new method and decorate it with [MapToApiVersion("3.0")]
	//        this requires the a version "3.0" to have an attribute somewhere else in the application

	// Calling Examples:
	//		?api-version=2.0
	//		?v=2.0
	//		Header:   x-api-version: 2.0

	// Deprecate API Version
	//		[ApiVersion( "1.0", Deprecated = true )]
	//      [Obsolete]


	public class ApiVersioning
	{
		// TODO: API Version - 05
		static public void ConfigureServices(IServiceCollection services, ApiVersion defaultVersion)
		{
			services.AddApiVersioning(opt =>
			{
				opt.AssumeDefaultVersionWhenUnspecified = true;
				opt.DefaultApiVersion = defaultVersion;
				opt.ReportApiVersions = true;

				opt.ApiVersionReader = ApiVersionReader.Combine(
				  new HeaderApiVersionReader("x-api-version"),
				  new QueryStringApiVersionReader("v", "api-version"));
			});

			services.AddVersionedApiExplorer();

			//services.AddVersionedApiExplorer(
			//	options =>
			//	{
			//		// add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
			//		// note: the specified format code will format the version as "'v'major[.minor][-status]"
			//		options.GroupNameFormat = "'v'VVV";

			//		// note: this option is only necessary when versioning by url segment. the SubstitutionFormat
			//		// can also be used to control the format of the API version in route templates
			//		options.SubstituteApiVersionInUrl = true;
			//	});
		}
	}
}

