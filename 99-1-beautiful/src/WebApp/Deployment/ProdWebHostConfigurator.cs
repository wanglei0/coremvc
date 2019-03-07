using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WebApp.Deployment
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    class ProdWebHostConfigurator : IWebHostConfiguratorForEnvironment
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.UseSerilog(
                (ctx, logBuilder) =>
                {
                    logBuilder
                        .MinimumLevel.Warning()
                        .WriteTo.Console(); // TODO: Write to Splunk after centralized config repository is configured.
                });
        }
    }
}