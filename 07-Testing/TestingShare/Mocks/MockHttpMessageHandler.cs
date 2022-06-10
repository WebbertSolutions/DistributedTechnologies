using System.Net;
using Moq;
using Moq.Protected;

namespace TestingShare
{
	public class MockHttpMessageHandler
	{
		public Mock<HttpMessageHandler> Mock { get; private set; }


		/// <summary>
		/// 
		/// </summary>
		public MockHttpMessageHandler()
			: this(MockBehavior.Strict)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="behavior"></param>
		public MockHttpMessageHandler(MockBehavior behavior)
		{
			Mock = new Mock<HttpMessageHandler>(behavior);
			Mock.Protected().Setup("Dispose", ItExpr.IsAny<bool>());
		}

		/// <summary>
		/// Create a Mock Http Message Handler
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public Mock<HttpMessageHandler> CreateMockOk(string content)
		{
			Mock
			   .Protected()
			   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			   .ReturnsAsync(new HttpResponseMessage()
			   {
				   StatusCode = HttpStatusCode.OK,
				   Content = (content == null) ? null : new StringContent(content)
			   })
			   .Verifiable();

			return Mock;
		}

		/// <summary>
		/// Create a Mock Http Message Handler that returns an error
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public Mock<HttpMessageHandler> CreateMockError(HttpStatusCode statusCode, string content)
		{
			Mock
			   .Protected()
			   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			   .ReturnsAsync(new HttpResponseMessage()
			   {
				   StatusCode = statusCode,
				   Content = (content == null) ? null : new StringContent(content)
			   })
			   .Verifiable();

			return Mock;
		}

		/// <summary>
		/// Create a Mock Http Message Handler that throws a HttpResponseMessage Exception with inner WebException 
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public Mock<HttpMessageHandler> CreateMockWebException(WebExceptionStatus statusCode, string content)
		{
			return CreateMockWebException(statusCode, $"Error Code: {statusCode}", content);
		}

		/// <summary>
		/// Create a Mock Http Message Handler that throws a HttpResponseMessage Exception with inner WebException 
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="errorMessage"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public Mock<HttpMessageHandler> CreateMockWebException(WebExceptionStatus statusCode, string errorMessage, string content)
		{
			Mock
			   .Protected()
			   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			   .Throws(new HttpRequestException(content, new WebException(errorMessage, statusCode)))
			   .Verifiable();

			return Mock;
		}

		/// <summary>
		/// Verify the Mock condition for the Get Request was satisfied
		/// </summary>
		/// <param name="method"></param>
		/// <param name="expectedUri"></param>
		/// <param name="numberOfRequests"></param>
		public void VerifyHttpRequest(HttpMethod method, string expectedUri, int numberOfRequests = 1)
		{
			Mock.Protected().Verify("SendAsync",
				Times.Exactly(numberOfRequests),
				ItExpr.Is<HttpRequestMessage>(req => req.Method == method && req.RequestUri!.AbsoluteUri == expectedUri),
				ItExpr.IsAny<CancellationToken>());
		}

		/// <summary>
		/// Verify the dispose method is called on the mock
		/// </summary>
		public void VerifyDispose()
		{
			Mock.Protected().Verify("Dispose",
				Times.Exactly(1),
				ItExpr.IsAny<bool>());
		}
	}
}
