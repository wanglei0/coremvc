using Microsoft.AspNetCore.Hosting;

namespace WebApp.Deployment
{
    public interface IEnvironmentSpecificWebHostConfigurator
    {
        void Configure(IWebHostBuilder builder);
    }
}