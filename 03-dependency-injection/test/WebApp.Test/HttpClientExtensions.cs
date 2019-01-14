using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace WebApp.Test
{
    static class HttpClientExtensions
    {
        public static async Task<string> AssertStatusAndGetStringAsync(
            this HttpResponseMessage response,
            HttpStatusCode expected = HttpStatusCode.OK)
        {
            Assert.Equal(expected, response.StatusCode);
            return await response.Content.ReadAsStringAsync();
        }
    }
}