using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using static System.Globalization.CultureInfo;

namespace SwaggerShare
{
	/// <summary>
	/// Represents the default implementation of an object that discovers and describes the API version information within an application.
	/// </summary>
	public class MyCustomApiVersionDescriptionProvider : IApiVersionDescriptionProvider
	{
		readonly Lazy<IReadOnlyList<ApiVersionDescription>> apiVersionDescriptions;
		readonly IOptions<ApiExplorerOptions> options;

		/// <summary>
		/// Initializes a new instance of the <see cref="MyCustomApiVersionDescriptionProvider"/> class.
		/// </summary>
		/// <param name="actionDescriptorCollectionProvider">The <see cref="IActionDescriptorCollectionProvider">provider</see> used to enumerate the actions within an application.</param>
		/// <param name="apiExplorerOptions">The <see cref="IOptions{TOptions}">container</see> of configured <see cref="ApiExplorerOptions">API explorer options</see>.</param>
		public MyCustomApiVersionDescriptionProvider(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, IOptions<ApiExplorerOptions> apiExplorerOptions)
		{
			apiVersionDescriptions = LazyApiVersionDescriptions.Create(this, actionDescriptorCollectionProvider);
			options = apiExplorerOptions;
		}

		/// <summary>
		/// Gets the options associated with the API explorer.
		/// </summary>
		/// <value>The current <see cref="ApiExplorerOptions">API explorer options</see>.</value>
		protected ApiExplorerOptions Options => options.Value;

		/// <summary>
		/// Gets a read-only list of discovered API version descriptions.
		/// </summary>
		/// <value>A <see cref="IReadOnlyList{T}">read-only list</see> of <see cref="ApiVersionDescription">API version descriptions</see>.</value>
		public IReadOnlyList<ApiVersionDescription> ApiVersionDescriptions => apiVersionDescriptions.Value;

		/// <summary>
		/// Determines whether the specified action is deprecated for the provided API version.
		/// </summary>
		/// <param name="actionDescriptor">The <see cref="ActionDescriptor">action</see> to evaluate.</param>
		/// <param name="apiVersion">The <see cref="ApiVersion">API version</see> to evaluate.</param>
		/// <returns>True if the specified <paramref name="actionDescriptor">action</paramref> is deprecated for the
		/// <paramref name="apiVersion">API version</paramref>; otherwise, false.</returns>
		public virtual bool IsDeprecated(ActionDescriptor actionDescriptor, ApiVersion apiVersion)
		{
			var model = actionDescriptor.GetApiVersionModel();
			return !model.IsApiVersionNeutral && model.DeprecatedApiVersions.Contains(apiVersion);
		}

		/// <summary>
		/// Enumerates all API versions within an application.
		/// </summary>
		/// <param name="actionDescriptorCollectionProvider">The <see cref="IActionDescriptorCollectionProvider">provider</see> used to enumerate the actions within an application.</param>
		/// <returns>A <see cref="IReadOnlyList{T}">read-only list</see> of <see cref="ApiVersionDescription">API version descriptions</see>.</returns>
		protected virtual IReadOnlyList<ApiVersionDescription> EnumerateApiVersions(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
		{
			if (actionDescriptorCollectionProvider == null)
				throw new ArgumentNullException(nameof(actionDescriptorCollectionProvider));

			var list = actionDescriptorCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>()
				.Select(d => new ApiGroupInformation(d))
				.ToList();

			var newDescriptions = GetMyDescriptions(list);
			return newDescriptions.OrderBy(d => d.ApiVersion).ToArray();
		}


		private IList<ApiVersionDescription> GetMyDescriptions(List<ApiGroupInformation> list)
		{
			return list.SelectMany(info =>
			{
				var groupName = info.GroupName;

				return info.Supported.Select(v => new ApiVersionDescription(v, GetGroupName(v, groupName), false))
					.Union(info.Deprecated.Select(v => new ApiVersionDescription(v, GetGroupName(v, groupName), true)));
			}).ToList();
		}

		private string GetGroupName(ApiVersion v, string groupName) => 
			GroupNameGenerator($"{v.ToString(Options.GroupNameFormat, CurrentCulture)}", groupName);


		static public string GroupNameGenerator(string version, string? name)
		{
			return string.IsNullOrWhiteSpace(name)
				? $"{version}"
				: $"{version}-{name}";
		}


		sealed class LazyApiVersionDescriptions : Lazy<IReadOnlyList<ApiVersionDescription>>
		{
			readonly MyCustomApiVersionDescriptionProvider apiVersionDescriptionProvider;
			readonly IActionDescriptorCollectionProvider actionDescriptorCollectionProvider;

			LazyApiVersionDescriptions(MyCustomApiVersionDescriptionProvider apiVersionDescriptionProvider, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
			{
				this.apiVersionDescriptionProvider = apiVersionDescriptionProvider;
				this.actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
			}

			internal static Lazy<IReadOnlyList<ApiVersionDescription>> Create(MyCustomApiVersionDescriptionProvider apiVersionDescriptionProvider, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
			{
				var descriptions = new LazyApiVersionDescriptions(apiVersionDescriptionProvider, actionDescriptorCollectionProvider);
				return new Lazy<IReadOnlyList<ApiVersionDescription>>(descriptions.EnumerateApiVersions);
			}

			IReadOnlyList<ApiVersionDescription> EnumerateApiVersions() => 
				apiVersionDescriptionProvider.EnumerateApiVersions(actionDescriptorCollectionProvider);
		}
	}
}