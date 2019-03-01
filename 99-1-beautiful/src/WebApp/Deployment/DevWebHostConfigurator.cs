using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApp.Deployment
{
    class DevWebHostConfigurator : IWebHostConfiguratorForEnvironment
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(
                (c, lb) => lb.AddConfiguration(c.Configuration).AddConsole().SetMinimumLevel(LogLevel.Debug));
        }
    }
}