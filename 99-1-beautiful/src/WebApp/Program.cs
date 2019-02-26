using Microsoft.AspNetCore.Hosting;
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
                .WithWebHostBuilder<DevWebHostConfigurator>(h => h.IsDevelopment())
                .WithWebHostBuilder<ProdWebHostConfigurator>(h => h.IsProduction())
                .UseEnvironmentAwareStartup(
                    (h => h.IsDevelopment(), typeof(DevStartup)),
                    (h => h.IsProduction(), typeof(ProductionStartup)));
        }
    }
}