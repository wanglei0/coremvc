using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WebApp.Deployment.Production
{
    class ProductionWebHostConfigurator : IWebHostConfigurator
    {
        public void ConfigureLogging(WebHostBuilderContext context, IServiceCollection services)
        {
            services.AddSerilog(config => config.MinimumLevel.Warning().WriteTo.Console());
        }
    }
}