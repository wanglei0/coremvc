using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Deployment
{
    public interface IWebHostConfigurator
    {
        void ConfigureLogging(WebHostBuilderContext context, IServiceCollection services);
        void ConfigureConfiguration(WebHostBuilderContext context, IConfigurationBuilder configurationBuilder);
    }
}