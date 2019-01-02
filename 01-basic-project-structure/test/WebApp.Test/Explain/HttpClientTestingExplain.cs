using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace WebApp.Test.Explain
{
    public class HttpClientTestingExplain
    {
        class HelloMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                responseMessage.Content = new StringContent("Hello", Encoding.UTF8, "text/plain");
                return Task.FromResult(responseMessage);
            }
        }
        
        [Fact]
        public async Task should_handle_request_using_http_message_handler()
        {
            using (var client = new HttpClient(new HelloMessageHandler()))
            using (HttpResponseMessage response = await client.GetAsync("http://whatever.it.is"))
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                string content = await response.Content.ReadAsStringAsync();
                Assert.Equal("Hello", content);
            }
        }

        [Fact]
        public async Task should_wrap_application_as_http_message_handler()
        {
            var builder = new WebHostBuilder();
            builder.UseStartup<Startup>();
            
            using (var testServer = new TestServer(builder))
            using (var client = new HttpClient(testServer.CreateHandler()))
            using (HttpResponseMessage response =
                await client.GetAsync("http://whatever.it.is/api/values"))
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}