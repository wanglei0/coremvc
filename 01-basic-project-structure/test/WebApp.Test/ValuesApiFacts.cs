using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WebApp.Test
{
    public class ValuesApiFacts
    {
        [Fact]
        public async Task should_get_ok_when_get_values()
        {
            var factory = new WebApplicationFactory<Startup>();
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/api/values");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}