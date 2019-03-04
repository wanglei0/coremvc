using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Deployment.Initialization
{
    class EnvironmentAwareStartup
    {
        readonly IStartupForEnvironment selectedStartup;
        readonly ILogger<EnvironmentAwareStartup> logger;
        readonly string selectedStartupName;

        public EnvironmentAwareStartup(
            ILogger<EnvironmentAwareStartup> logger, 
            IStartupForEnvironment selectedStartup)
        {
            this.selectedStartup = selectedStartup;
            this.logger = logger;
            selectedStartupName = selectedStartup.GetType().FullName;
            
            this.logger.LogDebug("Will use {startupType} as startup type.", selectedStartupName);
        }

        public void Configure(IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                // We need to do this since the configure process may resolve resource critical instances such as
                // database connections. This behavior is familiar with the default behavior when invoking
                // Startup.Configure by AspNetCore.
                logger.LogDebug("Invoking {startupType}.Configure(app, scopedProvider)", selectedStartupName);
                selectedStartup.Configure(app, scope.ServiceProvider);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            logger.LogDebug("Invoking {startupType}.ConfigureServices(services)", selectedStartupName);
            selectedStartup.ConfigureServices(services);
        }
    }
}