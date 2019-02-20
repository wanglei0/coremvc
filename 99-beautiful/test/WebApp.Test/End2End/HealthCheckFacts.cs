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
        readonly HealthCheckConfig config = new HealthCheckConfig();
        
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            // TODO: Replace connection string with BaseClass values.
            services.AddSingleton<IOptionsSnapshot<HealthCheckConfig>>(
                _ => new OptionsSnapshot<HealthCheckConfig>(config));
        }

        [Fact]
        public async Task should_get_healthy_state_if_all_env_are_in_good_condition()
        {
            config.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
            
            HttpResponseMessage response = await Client.GetAsync("health");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string content = await response.Content.ReadAsStringAsync();

            const string expected =
                "Healthy\n" +
                "database:Healthy";
            Assert.Equal(expected, content);
        }

        [Fact]
        public async Task should_get_unhealthy_state_if_database_connection_is_not_available()
        {
            config.ConnectionString = "";
            
            HttpResponseMessage response = await Client.GetAsync("health");
            
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);

            string content = await response.Content.ReadAsStringAsync();

            const string expected =
                "Unhealthy\n" +
                "database:Unhealthy";
            Assert.Equal(expected, content);
        }
    }
}