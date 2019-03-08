using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Deployment
{
    public interface IWebHostConfigurator
    {
        void ConfigureLogging(WebHostBuilderContext context, IServiceCollection services);
    }
}