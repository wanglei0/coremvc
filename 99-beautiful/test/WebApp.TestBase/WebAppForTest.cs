using System;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApp.TestBase
{
    class WebAppForTest<T> : IDisposable where T: class
    {
        readonly object sync = new object();
        WebApplicationFactory<T> factory;
        HttpClient client;

        public HttpClient GetOrCreateHttpClient(
            WebApplicationFactoryClientOptions options,
            Action<IWebHostBuilder> configureWebHostBuilder)
        {
            // We disable warning because the client is contained in a volatile read block so that
            // It will always read the latest assigned value. (ensure load-acquire semantics)
            //
            // ReSharper disable once InconsistentlySynchronizedField
            HttpClient theClient = Volatile.Read(ref client);
            if (theClient != null) { return theClient;}
                
            lock (sync)
            {
                theClient = Volatile.Read(ref client);
                if (theClient != null) { return theClient; }

                factory = new WebApplicationFactory<T>()
                    .WithWebHostBuilder(configureWebHostBuilder);
                
                // Ensure store-release semantics
                Volatile.Write(ref client, factory.CreateClient(options));
                return client;
            }
        }

        public void Dispose()
        {
            factory?.Dispose();
            client?.Dispose();
        }
    }
}