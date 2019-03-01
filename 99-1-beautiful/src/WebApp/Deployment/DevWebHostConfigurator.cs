using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace WebApp.Deployment
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    class DevWebHostConfigurator : IWebHostConfiguratorForEnvironment
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.UseSerilog(
                (ctx, logBuilder) =>
                {
                    logBuilder
                        .MinimumLevel.Debug()
                        .WriteTo.Console();
                });
        }
    }
}