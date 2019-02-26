using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Deployment
{
    class DevStartup : IEnvironmentSpecificStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(c =>
            {
                var logger = c.RequestServices.GetService<ILogger<DevStartup>>();
                logger.LogDebug("Hello Debug!");
                return c.Response.WriteAsync("Hello Development");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}