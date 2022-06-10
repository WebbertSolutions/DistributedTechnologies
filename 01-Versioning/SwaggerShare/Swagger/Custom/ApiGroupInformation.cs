using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using static Microsoft.AspNetCore.Mvc.Versioning.ApiVersionMapping;

namespace SwaggerShare
{
	public class ApiGroupInformation
	{
		public string GroupName { get; private set; }
		public ISet<ApiVersion> Supported { get; private set; }
		public ISet<ApiVersion> Deprecated { get; private set; }


		public ApiGroupInformation(ControllerActionDescriptor actionDescriptor)
		{
			GroupName = GetGroupName(actionDescriptor);
			(Supported, Deprecated) = GetVersions(actionDescriptor);
		}


		static private string GetGroupName(ControllerActionDescriptor actionDescriptor)
		{
			var apiDescription = actionDescriptor.Properties
				.FirstOrDefault(p => ((Type)p.Key) == typeof(ApiDescriptionActionData)).Value as ApiDescriptionActionData;

			return apiDescription?.GroupName ?? actionDescriptor.ControllerName;
		}


		static private (ISet<ApiVersion> supported, ISet<ApiVersion> deprecated) GetVersions(ControllerActionDescriptor actionDescriptor)
		{
			var model = actionDescriptor.GetApiVersionModel(Explicit | Implicit);

			var declared = model.DeclaredApiVersions.ToHashSet();
			var supported = model.SupportedApiVersions.ToHashSet();
			var advertisedSupported = model.SupportedApiVersions.ToHashSet();
			var deprecated = model.DeprecatedApiVersions.ToHashSet();
			var advertisedDeprecated = model.DeprecatedApiVersions.ToHashSet();

			advertisedSupported.ExceptWith(declared);
			advertisedDeprecated.ExceptWith(declared);
			supported.ExceptWith(advertisedSupported);
			deprecated.ExceptWith(supported.Concat(advertisedDeprecated));

			return (supported, deprecated);
		}
	}
}