using System;
using System.Net;
using System.Net.Http;
using TestingShare;

namespace TestBuilderProject.Tests
{
	public class HttpClientBuilder : MockBuilder
	{
		public HttpClient BuildOK()
		{
			var returnObject = dependencies.GetDependency<string>("returnObject");
			var uri = dependencies.GetDependency<Uri>();

			var mockHttpMessageHandler = new MockHttpMessageHandler(Moq.MockBehavior.Strict);
			mockHttpMessageHandler.CreateMockOk(returnObject);

			return new HttpClient(mockHttpMessageHandler.Mock.Object)
			{
				BaseAddress = uri
			};
		}

		public HttpClient BuildException()
		{
			var uri = dependencies.GetDependency<Uri>();

			var webExceptionStatus = dependencies.GetDependency<WebExceptionStatus>("status");
			string message = dependencies.GetDependency<string>("message");

			var mockHttpMessageHandler = new MockHttpMessageHandler(Moq.MockBehavior.Strict);
			mockHttpMessageHandler.CreateMockWebException(webExceptionStatus, message);

			return new HttpClient(mockHttpMessageHandler.Mock.Object)
			{
				BaseAddress = uri
			};
		}



		static public HttpClientBuilder CreateHttpClientBuilder(
			string uri,
			string? returnObject)
		{
			var builder = new HttpClientBuilder();

			builder.AddDependency(new Uri(uri));

			if (returnObject != null)
				builder.AddDependency(returnObject);

			return builder;
		}

		static public HttpClientBuilder CreateHttpClientBuilder(
			string uri,
			WebExceptionStatus webExceptionStatus,
			string message)
		{
			var builder = new HttpClientBuilder();

			builder.AddDependency(new Uri(uri));
			builder.AddDependency("status", webExceptionStatus);
			builder.AddDependency("message", message);

			return builder;
		}
	}
}