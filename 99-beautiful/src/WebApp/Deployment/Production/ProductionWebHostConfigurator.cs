using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WebApp.Deployment.Production
{
    class ProductionWebHostConfigurator : IWebHostConfigurator
    {
        public void ConfigureLogging(WebHostBuilderContext context, IServiceCollection services)
        {
            services.AddSerilog(config => config.ReadFrom.Configuration(context.Configuration));
        }

        public void ConfigureConfiguration(WebHostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddEnvironmentSpecificJsonConfiguration(context.HostingEnvironment.EnvironmentName);
        }
    }
}