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
            app.Run(c => c.Response.WriteAsync("Hello Development"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}