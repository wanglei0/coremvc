using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .ConfigureLogging((context, logBuilder) => {
                    if (context.HostingEnvironment.IsDevelopment()) {
                        logBuilder
                            .SetMinimumLevel(LogLevel.Debug)
                            .AddConsole();
                    }
                    if (context.HostingEnvironment.IsProduction()) {
                        logBuilder
                            .SetMinimumLevel(LogLevel.Warning)
                            .AddConsole();
                    }
                })
                .UseStartup<Startup>();
        }
    }
}
