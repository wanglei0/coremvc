using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.Deployment;

namespace WebApp
{
    public class Startup
    {
        public Startup(
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
        
        public void Configure(IApplicationBuilder app)
        {
        }
    }
}