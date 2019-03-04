using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Deployment
{
    class DevStartup : IStartupForEnvironment
    {
        public void Configure(IApplicationBuilder app, IServiceProvider scopedProvider)
        {
            app.Run(c => c.Response.WriteAsync("Hello Development"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}