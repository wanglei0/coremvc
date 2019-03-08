using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApp
{
    public class WebAppHostBuilderFactory
    {
        public IWebHostBuilder Create(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel(ko => ko.AddServerHeader = false)
                .ConfigureAppConfiguration(cb => cb.AddCommandLine(args))
                .UseStartup<Startup>();
        }
    }
}