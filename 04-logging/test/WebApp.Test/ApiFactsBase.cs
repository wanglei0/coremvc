using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApp.Test
{
    public abstract class ApiFactsBase : IDisposable
    {
        readonly WebApplicationFactory<Startup> factory;
        protected HttpClient Client { get; }

        protected ApiFactsBase()
        {
            factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(wb => wb.UseEnvironment(EnvironmentName.Development));
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false
            });
        }

        public void Dispose()
        {
            Client?.Dispose();
            factory?.Dispose();
        }
    }
}