using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Deployment
{
    public class EnvironmentAwareStartup
    {
        readonly ILogger<EnvironmentAwareStartup> logger;
        readonly IEnvironmentSpecificStartup selectedStartup;

        public EnvironmentAwareStartup(
            ILogger<EnvironmentAwareStartup> logger, 
            IEnvironmentSpecificStartup selectedStartup)
        {
            this.logger = logger;
            this.selectedStartup = selectedStartup;

            logger.LogInformation("Will use {startupType} as startup type.", selectedStartup.GetType().FullName);
        }

        public void Configure(IApplicationBuilder app) { selectedStartup.Configure(app); }

        public void ConfigureServices(IServiceCollection services) { selectedStartup.ConfigureServices(services); }
    }
}