using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.TestBase;
using Xunit;

namespace WebApp.Test.End2End
{
    public class HealthCheckFacts : ApiFactBase<Startup>
    {
        [Fact]
        public async Task should_check_health()
        {
            HttpResponseMessage response = await Client.GetAsync("health");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string content = await response.Content.ReadAsStringAsync();

            const string expected =
                "Healthy\n" +
                "database:Healthy";
            Assert.Equal(expected, content);
        }
    }
}