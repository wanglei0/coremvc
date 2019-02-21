using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Adapting
{
    static class HttpClientWebHostBuilderExtensions
    {
        const int DefaultTimeoutValue = 20;

        public static IWebHostBuilder UseHttpClient(this IWebHostBuilder builder)
        {
            builder.ConfigureServices(ConfigureServices);
            return builder;
        }

        static void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            var timeoutValue = context.Configuration
                .GetSection("HttpClient")
                .GetValue<int>("TimeoutInSeconds");
            if (timeoutValue == default(int))
            {
                timeoutValue = DefaultTimeoutValue;
            }

            services.AddSingleton(
                _ =>
                {
                    var handler = new HttpClientHandler
                    {
                        // Add your own SSL validation rule here to ensure cert can be trusted. Here
                        // we just return true indicating that we will simply accept the cert.
                        ServerCertificateCustomValidationCallback =
                            (message, cert, chain, error) => true,

                        // Normally server side http client will treat redirection as a failure, or
                        // will redirect manually. You can change this settings on demand.
                        AllowAutoRedirect = false
                    };

                    // Modify timeout value.
                    return new HttpClient(handler) {Timeout = TimeSpan.FromSeconds(timeoutValue)};
                });
        }
    }
}