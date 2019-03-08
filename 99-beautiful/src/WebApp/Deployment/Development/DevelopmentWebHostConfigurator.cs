using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WebApp.Deployment.Development
{
    class DevelopmentWebHostConfigurator : IWebHostConfigurator
    {
        public void ConfigureLogging(WebHostBuilderContext context, IServiceCollection services)
        {
            services.AddSerilog(config => config.MinimumLevel.Debug().WriteTo.Console());
        }
    }
}