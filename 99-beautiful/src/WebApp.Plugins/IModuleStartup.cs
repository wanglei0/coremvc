using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Plugins
{
    /// <summary>
    /// This interface represents the configuration part of a module. A module is a standalone
    /// component that expose its APIs. The interface highly consists of the `Startup` interface
    /// define by ASP.NET Core, so that we can easily extract module as a web service.
    /// </summary>
    public interface IModuleStartup
    {
        /// <summary>
        /// Add service definitions to DI only for this module.
        /// </summary>
        /// <param name="services">
        /// This argument is the service collection which holds all the service definitions for the
        /// current host.
        /// </param>
        void ConfigureServices(IServiceCollection services);
        
        /// <summary>
        /// You can add middleware to pipeline for this module. Please note that this may affect
        /// other modules in the same hosting process as well.
        /// </summary>
        /// <param name="app">The pipeline builder.</param>
        /// <param name="env">The environment information.</param>
        void Configure(IApplicationBuilder app, IHostingEnvironment env);
    }
}