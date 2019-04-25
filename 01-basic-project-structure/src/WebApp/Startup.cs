using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.Migrator;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // This method gets called by the runtime. Use this method to add services to
            // the container.
            services
                .AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder =>
                {
                    runnerBuilder
                        .AddSqlServer2014()
                        .WithGlobalConnectionString("Server=.; Database=NHibernatePractice; Integrated Security=SSPI;")
                        .ScanIn(typeof(M001_CreateUserTable).Assembly).For.Migrations();
                })

//                .AddLogging(builder => { 
//                    builder
//                    .AddFilter("Microsoft", LogLevel.Debug)
//                    .AddConsole(); 
//                })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // This method gets called by the runtime. Use this method to configure the HTTP
            // request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
        }
    }
}
