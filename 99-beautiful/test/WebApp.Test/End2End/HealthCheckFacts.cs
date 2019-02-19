using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApp.Test.Utils;
using WebApp.TestBase;
using WebModule.HealthCheck;
using Xunit;

namespace WebApp.Test.End2End
{
    public class HealthCheckFacts : ApiFactBase<Startup>
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            // TODO: Replace connection string with BaseClass values.
            services.AddSingleton<IOptionsSnapshot<HealthCheckConfig>>(
                _ => new OptionsSnapshot<HealthCheckConfig>(new HealthCheckConfig
                {
                    ConnectionString = "Data Source=:memory:;Version=3;New=True;"
                }));
        }

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