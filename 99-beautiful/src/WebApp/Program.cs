using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using WebApp.Adapting;
using WebApp.Logging;

namespace WebApp
{
    static class Program
    {
        public static void Main(string[] args)
        {
            IWebHost webHost = null;
            
            try
            {
                webHost = CreateWebHostBuilder(args).Build();
            }
            catch (Exception error)
            {
                // Some project template initializes the configuration at the entry point of the
                // program to create a logger instance. This is not feasible for certain
                // environment. For example. If the application is hosted on IIS Server. The
                // GetCurrentDirectory method will returns the work's base address rather than the
                // application content root.
                //
                // So my recommendation is to create a temporary logger to record issues happened
                // during web host initialization. Then just abandoned the logger as soon as the
                // web host is ready. Since it is most possible that the logger will record fatal
                // errors, it is better set a global wide sink to ensure the logs are recorded.
                // (e.g. OS Event Sink, or local file system).
                using (IEmergencyLogger logger = EmergencyLoggerFactory.Create())
                {
                    logger.Fatal(
                        error,
                        "An error occured during initialization with start args {args}",
                        (object) args);
                }
            }

            webHost?.Run();
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
                .UseStartup<Startup>()
                .ConfigureLogging(
                    (context, logging) =>
                    {
                        // The default web host builder will add at least 3 logging sources:
                        // Console, Debug and EventSource. This configuration may not meet your
                        // requirement. So it is better to re-config by yourself.
                        logging.ClearProviders();
                    })
                .UseHttpClient()
                .UseWebAppLogger();
        }
    }
}