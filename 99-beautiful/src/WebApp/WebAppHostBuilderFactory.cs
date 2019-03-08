using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using WebApp.Deployment;

namespace WebApp
{
    public class WebAppHostBuilderFactory
    {
        readonly WebHostConfiguratorFactory configuratorFactory;

        public WebAppHostBuilderFactory() : this(new WebHostConfiguratorFactory())
        {
        }
        
        public WebAppHostBuilderFactory(WebHostConfiguratorFactory configuratorFactory)
        {
            this.configuratorFactory = configuratorFactory;
        }
        
        public IWebHostBuilder Create(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel(ko => ko.AddServerHeader = false)
                .ConfigureAppConfiguration(cb => cb.AddCommandLine(args))
                .ConfigureServices((context, services) =>
                    configuratorFactory.Create(context.HostingEnvironment.EnvironmentName)
                        .ConfigureLogging(context, services))
                .UseStartup<Startup>();
        }
    }
}