using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.Migrator;
using WebApp.Resources;
using WebApp.Resources.Providers;
using WebApp.Resources.Repository;
using WebApp.Resources.Repository.Models;

namespace WebApp
{
    public static class Program
    {
        
        public static void Main(string[] args)
        {

            var webHost = CreateWebHostBuilder(args).Build();
            var runner = webHost.Services.GetService<IMigrationRunner>();
            runner.MigrateUp();
            webHost.Run();
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
//                .ConfigureServices(collection =>
//                {
//                        collection
//                            .AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>))
//                            .AddTransient<UserRepository>()
//                            .AddTransient<IDatabaseSessionProvider, DatabaseSessionProvider>()
//                            .AddTransient<DatabaseModel>()
//                            .AddTransient<SqlStatementInterceptor>()
//                            .AddTransient<User>()
//                            .AddTransient<TableNameConvention>();
//                    })
                    .UseStartup<Startup>();
        }
    }
}
