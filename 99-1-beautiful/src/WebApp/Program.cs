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
                .ConfigureEnvironment(
                    (
                        EnvironmentName.Development,
                        EnvironmentSetup.Create<DevWebHostConfigurator, DevStartup>()
                    ),
                    (
                        EnvironmentName.Production,
                        EnvironmentSetup.Create<ProdWebHostConfigurator, ProductionStartup>()
                    ));
        }
    }
}