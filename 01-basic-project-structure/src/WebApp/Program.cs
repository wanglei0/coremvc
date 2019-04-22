using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.Resources;
using WebApp.Resources.Providers;
using WebApp.Resources.Repository;

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
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection
                        .AddLogging(loggingBuilder =>
                        {
                            loggingBuilder
                                .AddFilter("Microsoft", LogLevel.Debug)
                                .AddConsole();
                        })
                        .AddAuthentication(serviceCn => { });
                })
                    .UseKestrel()
//                .ConfigureLogging((context, logBuilder) => { logBuilder.AddConsole(); })
                .ConfigureServices(collection =>
                    {
                        collection.AddTransient<IUsersRepository, UsersRepository>();
                        collection.AddTransient<IDatabaseSessionProvider, DatabaseSessionProvider>();
                        collection.AddTransient<DatabaseModel>();
                        collection.AddTransient<SqlStatementInterceptor>();
                    })
                    .UseStartup<Startup>();
        }
    }
}
