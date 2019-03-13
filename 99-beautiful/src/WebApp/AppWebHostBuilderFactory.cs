using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using WebApp.Deployment;

namespace WebApp
{
    public class AppWebHostBuilderFactory
    {
        readonly WebHostConfiguratorFactory configuratorFactory;

        public AppWebHostBuilderFactory() : this(new AppConfiguratorFactory())
        {
        }
        
        public AppWebHostBuilderFactory(WebHostConfiguratorFactory configuratorFactory)
        {
            this.configuratorFactory = configuratorFactory;
        }
        
        public IWebHostBuilder Create(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel(ko => ko.AddServerHeader = false)
                //
                // Warning: not suitable for IIS integration. Please call UseIISIntegration() and UseIIS() instead.
                //
                .UseContentRoot(Environment.CurrentDirectory)
                .ConfigureAppConfiguration((context, cb) =>
                {
                    configuratorFactory.Create(context.HostingEnvironment.EnvironmentName)
                        .ConfigureConfiguration(context, cb);
                    cb.AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                    configuratorFactory.Create(context.HostingEnvironment.EnvironmentName)
                        .ConfigureLogging(context, services))
                .UseStartup<Startup>();
        }
    }
}