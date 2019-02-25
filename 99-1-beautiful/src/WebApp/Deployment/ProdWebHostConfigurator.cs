using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApp.Deployment
{
    class ProdWebHostConfigurator : IEnvironmentSpecificWebHostConfigurator
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(lb => lb.AddConsole().SetMinimumLevel(LogLevel.Warning));
        }
    }
}