using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Core;
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
                // program to create a logger instance. And they will also use the same config
                // instance to initialize the web-host. I don't think it is a good idea since the
                // configuration contains too many things which may be hard to figure out at the
                // entry point. And there is no method exposed to initialize the configuration
                // consistently.
                //
                // So my recommendation is to create a temporary logger to record issues happened
                // during web host initialization. Then just abandoned the logger as soon as the
                // web host is ready.
                using (IEmergencyLogger logger = WebAppLogger.CreateEmergencyLogger(args))
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
                .UseWebAppLogger();
        }
    }
}