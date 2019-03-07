using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Deployment
{
    public interface IStartupForEnvironment
    {
        void Configure(IApplicationBuilder app, IServiceProvider scopedProvider);
        void ConfigureServices(IServiceCollection services);
    }
}