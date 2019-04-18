using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.Resources;

namespace WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (var session = FluentNHibernateHelper.OpenSession())

            {

                var product = new Users { Id = 1, FirstName = "firstname", LastName = "lastname" };

                session.Save(product);
                session.Flush();

            }
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
                .UseStartup<Startup>();
        }
    }
}
