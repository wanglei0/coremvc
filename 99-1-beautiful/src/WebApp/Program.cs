using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using WebApp.Deployment;

namespace WebApp
{
    static class Program
    {
        public static void Main(string[] args) { CreateWebHostBuilder(args).Build().Run(); }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .ConfigureLogging(lb => lb.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .ConfigureLogging(lb => lb.AddConsole().SetMinimumLevel(LogLevel.Warning))
//                .WithWebHostBuilder<DevWebHostConfigurator>(h => h.IsDevelopment())
//                .WithWebHostBuilder<ProdWebHostConfigurator>(h => h.IsProduction())
                .UseEnvironmentAwareStartup(
                    (h => h.IsDevelopment(), typeof(DevStartup)),
                    (h => true, typeof(ProductionStartup)));
        }
    }
}