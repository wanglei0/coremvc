using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Core;

namespace WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .ConfigureLogging((context, loggerBuilder) =>
                {
                    Logger logger;
                    
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .CreateLogger();
                    }
                    else
                    {
                        logger = new LoggerConfiguration()
                            .MinimumLevel.Warning()
                            .WriteTo.Console()
                            .CreateLogger();
                    }

                    loggerBuilder.AddSerilog(logger);
                })
                .UseStartup<Startup>();
        }
    }
}
