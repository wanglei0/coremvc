using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace WebApp.Test
{
    public class RouteGeneratingFacts : ApiFactsBase
    {
        [Fact]
        public async Task should_generate_url_using_action()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/route-generating/users/api");
            Assert.Equal(
                "/api/route-generating/users/3",
                await response.AssertStatusAndGetStringAsync());
        }

        [Fact]
        public async Task should_generate_url_using_route_name()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/route-generating/users/api/get_user_by_id");
            Assert.Equal(
                "/api/route-generating/users/3",
                await response.AssertStatusAndGetStringAsync());
        }
    }
}