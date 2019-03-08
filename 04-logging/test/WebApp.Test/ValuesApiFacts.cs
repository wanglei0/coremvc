using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace WebApp.Test
{
    public class ValuesApiFacts : ApiFactsBase
    {
        [Fact]
        public async Task should_get_ok_when_get_values()
        {
            HttpResponseMessage response = await Client.GetAsync("/api/values");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}