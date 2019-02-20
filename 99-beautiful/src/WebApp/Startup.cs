using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.ExceptionHandling;
using WebApp.PluginEngine;
using WebApp.Plugins;

namespace WebApp
{
    public class Startup
    {
        readonly IList<IModuleStartup> modules;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            PluginLoader pluginLoader = new PluginLoader(configuration, logger)
                .AddPluginStartupClass(typeof(WebModule.HealthCheck.Startup))
                .AddPluginStartupClass(typeof(WebModule.SampleModule.Startup));
            modules = pluginLoader.Load();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            foreach (IModuleStartup startup in modules)
            {
                startup.ConfigureServices(services);
            }
            
            ConfigureServicesGlobally(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            foreach (IModuleStartup startup in modules)
            {
                startup.Configure(app, env);
            }
            
            // Although undesirable, we will allow modules to add their own common processing logic
            // before the final MVC pipeline.
            // 
            ConfigurePipelineGlobally(app, env);
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
                //
                app.UseApiExceptionResponse();
            }

            app.UseMvc();
        }

        void ConfigureServicesGlobally(IServiceCollection services)
        {
            // The MVC framework sits at the infrastructure level and thus not belong to any module.
            // It is not good to create a module called "Infrastructure Module" because the module
            // should represent business rather than a layer.
            IMvcBuilder mvcBuilder = services.AddMvc()
                .ConfigureApplicationPartManager(
                    partManager =>
                        partManager.FeatureProviders.Add(new InternalControllerFeatureProvider()));
            
            foreach (Assembly assembly in modules.Select(m => m.GetType().Assembly).Distinct())
            {
                mvcBuilder.AddApplicationPart(assembly);
            }
        }
    }
}