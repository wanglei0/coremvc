using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using WebApp.Deployment;

namespace WebApp.Infrastructure.Test.Deployment.Helpers
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    class NullWebHostConfigurator : IWebHostConfiguratorForEnvironment
    {
        public void Configure(IWebHostBuilder builder) { }
    }
}