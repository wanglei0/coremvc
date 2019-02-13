using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApp
{
    static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // The project template will create this method automatically for you. This method is
            // the entry point of a web application. The default implementation of
            // WebApplicationFactory class (in your unit test) requires this method. So if you
            // inline this method to Main, then your unit test is going to crash.
            // 
            // If you want to change the default behavior, you can derive form WebApplicationFactory
            // class and override the CreateWebHostBuilder() method.
            
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}