using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Plugins;
using WebModule.SampleModule.Domain;

namespace WebModule.SampleModule
{
    // Let's think about how many steps do we need to add a new module to the web application.
    //
    // - First, add a startup class which implements IModuleStartup interface.
    // - Second, create the module startup instance and add it to the module list at the web host
    // (or we can resolve modules from the configuration file).
    // 
    // It seems reasonable enough to have only two steps to create a new module.
    //
    public class Startup : IModuleStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<MessageRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}