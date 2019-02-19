using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using Axe.SimpleHttpMock;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.TestBase
{
    /// <inheritdoc />
    /// <summary>
    /// This is the base class for running end-to-end API test.
    /// </summary>
    /// <typeparam name="T">The type of the startup class for the web application.</typeparam>
    public abstract class ApiFactBase<T> : IDisposable 
        where T : class
    {
        readonly WebApplicationFactoryClientOptions clientOptions;
        WebApplicationFactory<T> factory;
        HttpClient client;
        bool isDisposed;

        WebApplicationFactory<T> Factory
        {
            get
            {
                // There is no need to put thread-safe into consideration here since the testing
                // methods in the same test class will not run in parallel.
                //
                // We lazily create the factory mainly because we want to avoid calling a virtual
                // method in the constructor.
                LazyInitializer.EnsureInitialized(
                    ref factory,
                    () => new WebApplicationFactory<T>()
                        .WithWebHostBuilder(wb => wb.ConfigureServices(ConfigureServices)));
                return factory;
            }
        }

        /// <summary>
        /// Get an HTTP client that can send a request to the testing web application. 
        /// </summary>
        protected HttpClient Client
        {
            get
            {
                LazyInitializer.EnsureInitialized(
                    ref client,
                    () => Factory.CreateClient(clientOptions));
                return client;
            }
        }

        /// <summary>
        /// Get a configurable object that simulates all the external services.
        /// </summary>
        protected MockHttpServer ExternalServices { get; } = new MockHttpServer();

        /// <summary>
        /// Create an end-to-end API test using the default configuration.
        /// </summary>
        protected ApiFactBase() : this(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("http://www.baseaddress.com"),
            HandleCookies = true
        }) { }

        /// <summary>
        /// Create an end-to-end API test using customized configuration.
        /// </summary>
        /// <param name="clientOptions">The HTTP client configuration.</param>
        protected ApiFactBase(WebApplicationFactoryClientOptions clientOptions)
        {
            this.clientOptions = clientOptions;
        }

        /// <summary>
        /// Please rewrite the method if you want to replace some of the components in the service
        /// collection.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> object.
        /// </param>
        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// Create an HTTP client object which connects to the external services. You can use the
        /// created HTTP client instance to replace the one in your application's service
        /// collection.
        /// </summary>
        /// <returns>The HTTP client instance.</returns>
        protected HttpClient CreateHttpClientToCallExternalServices()
        {
            return new HttpClient(ExternalServices);
        }

        /// <summary>
        /// Release all the resources in the test.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;
            Dispose(true);
            
            client?.Dispose();
            factory?.Dispose();
            
            isDisposed = true;
        }

        /// <summary>
        /// Release all the resources in the test.
        /// </summary>
        /// <param name="disposing">
        /// If it is called by GC, then this value is <c>false</c>. Otherwise this value is
        /// <c>true</c>.
        /// </param>
        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        protected virtual void Dispose(bool disposing) { }
    }
}