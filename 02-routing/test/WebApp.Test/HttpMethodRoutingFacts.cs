using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace WebApp.Test
{
    public class HttpMethodRoutingFacts : ApiFactsBase
    {
        [Theory]
        [InlineData("http://what.ever.it.is/api/method")]
        [InlineData("http://what.ever.it.is/api/centralized-method")]
        public async Task should_route_to_get_method(string uri)
        {
            HttpResponseMessage response = await Client.GetAsync(uri);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string text = await response.Content.ReadAsStringAsync();
            
            Assert.Equal("GET", text);
        }

        [Theory]
        [InlineData("http://what.ever.it.is/api/method")]
        [InlineData("http://what.ever.it.is/api/centralized-method")]
        public async Task should_route_to_custom_method(string uri)
        {
            HttpRequestMessage request = CreateRequest("EAT", uri);
            HttpResponseMessage response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string text = await response.Content.ReadAsStringAsync();
            
            Assert.Equal("EAT", text);
        }

        static HttpRequestMessage CreateRequest(string method, string uriString)
        {
            var request = new HttpRequestMessage();
            request.Method = new HttpMethod(method);
            request.RequestUri = new Uri(uriString);
            return request;
        }

        [Theory]
        [InlineData("GET", "http://what.ever.it.is/api/method/multiple")]
        [InlineData("GET", "http://what.ever.it.is/api/centralized-method/multiple")]
        [InlineData("POST", "http://what.ever.it.is/api/method/multiple")]
        [InlineData("POST", "http://what.ever.it.is/api/centralized-method/multiple")]
        [InlineData("DELETE", "http://what.ever.it.is/api/method/multiple")]
        [InlineData("DELETE", "http://what.ever.it.is/api/centralized-method/multiple")]
        public async Task should_route_to_multiple_methods(string method, string uri)
        {
            HttpRequestMessage request = CreateRequest(
                method, uri);
            HttpResponseMessage response = await Client.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string text = await response.Content.ReadAsStringAsync();
            
            Assert.Equal($"Multiple:{method}", text);
        }
    }
}