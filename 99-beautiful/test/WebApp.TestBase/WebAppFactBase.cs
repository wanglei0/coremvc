using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using Axe.SimpleHttpMock;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace WebApp.TestBase
{
    /// <inheritdoc />
    /// <summary>
    /// This is the base class for running end-to-end API test.
    /// </summary>
    /// <typeparam name="T">The type of the startup class for the web application.</typeparam>
    public abstract class WebAppFactBase<T> : IDisposable 
        where T : class
    {
        readonly WebApplicationFactoryClientOptions clientOptions;
        readonly TestLoggingConfiguration loggingConfiguration;
        readonly ITestServiceConfigurator testServiceConfigurator;
        readonly ITestOutputHelper output;
        
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
                    () => new WebApplicationFactory<T>().WithWebHostBuilder(
                        wb => wb
                            .ConfigureServices(ConfigureServicesForTest)
                            .ConfigureServices(ConfigureServices)
                            .ConfigureAppConfiguration(ConfigureConfiguration)));
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

        /// <inheritdoc />
        /// <summary>
        /// Create an end-to-end API test using the default configuration.
        /// </summary>
        protected WebAppFactBase() : this(
            WebAppFactHttpClientConfiguration.DefaultHttpClientOptions,
            new TestLoggingConfiguration(false, false, LogLevel.Debug),
            null) { }

        protected WebAppFactBase(
            TestLoggingConfiguration loggingConfiguration, 
            ITestOutputHelper output) : this(
            WebAppFactHttpClientConfiguration.DefaultHttpClientOptions,
            loggingConfiguration,
            output) { }

        /// <summary>
        /// Create an end-to-end API test using customized configuration.
        /// </summary>
        /// <param name="clientOptions">The HTTP client configuration.</param>
        /// <param name="loggingConfiguration">Logging configuration.</param>
        /// <param name="outputHelper">The output helper.</param>
        /// <param name="serviceConfigurator">
        /// The service configuration that can be used to override default services configuration
        /// in a common manner.
        /// </param>
        protected WebAppFactBase(
            WebApplicationFactoryClientOptions clientOptions,
            TestLoggingConfiguration loggingConfiguration,
            ITestOutputHelper outputHelper,
            ITestServiceConfigurator serviceConfigurator = null)
        {
            this.clientOptions =
                clientOptions ?? throw new ArgumentNullException(nameof(clientOptions));
            this.loggingConfiguration =
                loggingConfiguration ??
                throw new ArgumentNullException(nameof(loggingConfiguration));
            testServiceConfigurator =
                serviceConfigurator ?? new WebAppServiceConfigurator();
            output = outputHelper ?? new NullOutputHelper();
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
        /// You can rewrite the method to add or override configuration to application. 
        /// </summary>
        /// <param name="context">
        /// The web-host builder context from which you can get information.
        /// </param>
        /// <param name="builder">The configuration builder.</param>
        protected virtual void ConfigureConfiguration(
            WebHostBuilderContext context,
            IConfigurationBuilder builder)
        {
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

        void ConfigureServicesForTest(IServiceCollection services)
        {
            testServiceConfigurator.ConfigureService(
                services,
                new Dictionary<string, object>
                {
                    { TestServiceConfiguratorKeys.ExternalService, ExternalServices },
                    { TestServiceConfiguratorKeys.LoggingConfiguration, loggingConfiguration },
                    { TestServiceConfiguratorKeys.TestOutputHelper, output }
                });
        }
    }
}