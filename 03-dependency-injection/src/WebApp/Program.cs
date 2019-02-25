using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()                        // services = new ServiceCollection(); 
                                                //    -> Add Kestrel to services
                                                //    -> Add Logging to services
                                                //    -> Add Startup to services
                                                // serviceProvider = serviceCollection.BuildServiceProvider()
                .Run();                         // serviceProvider.GetService<IServer>() # Kestrel
                                                // serviceProvider.GetService<Startup>()
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()         
                .UseKestrel()
                .ConfigureServices(AddTypesToServices)
                .UseStartup<Startup>(); 
        }

        static void AddTypesToServices(IServiceCollection services)
        {
            services.AddLogging(lb => lb.AddConsole().SetMinimumLevel(LogLevel.Debug));
        }
    }
}
