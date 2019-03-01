using System;
using Microsoft.AspNetCore.Hosting;
using WebApp.Deployment;

namespace WebApp.Infrastructure.Test.Deployment.Helpers
{
    class DelegatedWebHostConfigurator : IWebHostConfiguratorForEnvironment
    {
        readonly Action<IWebHostBuilder> configure;

        DelegatedWebHostConfigurator(Action<IWebHostBuilder> configure)
        {
            this.configure = configure;
        }
        
        public void Configure(IWebHostBuilder builder) { configure(builder); }

        public static IWebHostConfiguratorForEnvironment Create(Action<IWebHostBuilder> configure)
        {
            return new DelegatedWebHostConfigurator(configure);
        }
    }
}