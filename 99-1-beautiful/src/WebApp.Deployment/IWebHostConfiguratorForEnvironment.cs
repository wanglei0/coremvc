using Microsoft.AspNetCore.Hosting;

namespace WebApp.Deployment
{
    public interface IWebHostConfiguratorForEnvironment
    {
        void Configure(IWebHostBuilder builder);
    }
}