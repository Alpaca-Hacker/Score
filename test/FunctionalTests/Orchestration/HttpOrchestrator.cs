using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FunctionalTests.Extensions;
using FunctionalTests.Setup;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.WebUtilities;

namespace FunctionalTests.Orchestration
{
    public class HttpOrchestrator : IDisposable
    {
        private HttpRequestMessage _request;
        private TestHttpServerBuilder _testServerBuilder = new TestHttpServerBuilder();

        private TestServer _testServer;
        public TestServer TestServer
        {
            get
            {
                if (_testServer == null)
                {
                    _testServer = _testServerBuilder.Build();
                    _testServerBuilder = null;
                }

                return _testServer;
            }
        }

        public HttpResponseMessage Response;
        public Dictionary<string, string> Query;
        public string AbsolutePath { get; set; }
        private Uri Uri { get; set; }

        public HttpClient HttpClient { get; }

        public HttpOrchestrator()
        {
             HttpClient = TestServer.CreateClient();
        }

        public async Task SendAsync(HttpMethod method)
        {
            if (AbsolutePath == null)
            {
                throw new InvalidOperationException(nameof(AbsolutePath) + " must be set");
            }

            Uri = Query != null
                ? new Uri(QueryHelpers.AddQueryString(TestServer.GetAbsoluteUri(AbsolutePath).ToString(), Query))
                : TestServer.GetAbsoluteUri(AbsolutePath);

            _request = new HttpRequestMessage
            {
                RequestUri = Uri,
                Method = method
            };

            Response = await HttpClient.SendAsync(_request);
        }

        public async Task<T> GetResponseContentModel<T>()
        {
           var httpContent =  Response.Content;

            byte[] byteArr = await httpContent.ReadAsByteArrayAsync();
            var serialiser = new JsonSerialiser<T>(Encoding.UTF8);
            T model = serialiser.Deserialise(byteArr);
            return model;
        }

        public void Dispose()
        {
            _testServer?.Dispose();
        }

        public async Task AssertStatusCode(HttpStatusCode statusCode)
        {
            await Response.Content.ReadAsStringAsync();
            Response.StatusCode.Should().Be(statusCode);
        }
    }
}
