using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApp.ExceptionHandler;
using WebApp.Plugins;

namespace WebApp
{
    public class Startup
    {   
        // We can find all the dependent modules here. That means it is very easy for us to add or
        // remove a module.
        readonly IList<IModuleStartup> modules = new List<IModuleStartup>
        {
            new WebModule.HealthCheck.Startup()
        };
        
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesGlobally(services);
            
            foreach (IModuleStartup startup in modules)
            {
                startup.ConfigureServices(services);
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigurePipelineGlobally(app, env);

            foreach (IModuleStartup startup in modules)
            {
                startup.Configure(app, env);
            }
        }

        static void ConfigurePipelineGlobally(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // We use exception handler to create common error response rather than logging,
                // since logging is done by the framework.
                app.UseApiExceptionHandler();
            }

            app.UseMvc();
        }

        static void ConfigureServicesGlobally(IServiceCollection services)
        {
            // The MVC framework sits at the infrastructure level and thus not belong to any module.
            // It is not good to create a module called "Infrastructure Module" because the module
            // should represent business rather than a layer.
            services.AddMvc();
            
            services.AddSingleton(_ =>
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

                // Modify timeout value. TODO: expose it via configuration
                return new HttpClient(handler) {Timeout = TimeSpan.FromSeconds(20)};
            });
        }
    }
}