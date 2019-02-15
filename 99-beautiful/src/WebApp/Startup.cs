using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
            foreach (IModuleStartup startup in modules)
            {
                startup.ConfigureServices(services);
            }

            // The MVC framework sits at the infrastructure level and thus not belong to any module.
            // It is not good to create a module called "Infrastructure Module" because the module
            // should represent business rather than a layer.
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            foreach (IModuleStartup startup in modules)
            {
                startup.Configure(app, env);
            }

            app.UseMvc();
        }
    }
}