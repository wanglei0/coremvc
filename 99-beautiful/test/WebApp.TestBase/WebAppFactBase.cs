using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
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
        readonly WebAppForTest<T> app;
        bool isDisposed;

        /// <summary>
        /// Get an HTTP client that can send a request to the testing web application. 
        /// </summary>
        protected HttpClient Client =>
            app.GetOrCreateHttpClient(clientOptions, wb => wb
                .ConfigureServices(ConfigureServicesForTest)
                .ConfigureServices(ConfigureServices)
                .ConfigureAppConfiguration(ConfigureConfiguration));

        /// <summary>
        /// Get a configurable object that simulates all the external services.
        /// </summary>
        protected MockHttpServer ExternalServices { get; } = new MockHttpServer();

        /// <summary>
        /// Get the output helper which can help you to write some diagnostic information during
        /// test.
        /// </summary>
        protected ITestOutputHelper Output { get; }

        /// <inheritdoc />
        /// <summary>
        /// Create an end-to-end API test using the default configuration.
        /// </summary>
        protected WebAppFactBase() : this(
            WebAppFactHttpClientConfiguration.DefaultHttpClientOptions,
            new TestLoggingConfiguration(false, false, LogLevel.Debug),
            null) { }

        /// <summary>
        /// Create an end-to-end API test.
        /// </summary>
        /// <param name="loggingConfiguration">
        /// Custom logging configuration.
        /// </param>
        /// <param name="output">
        /// The output helper that logger will write information to.
        /// </param>
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
        /// <param name="outputHelper">
        /// The output helper that logger will write information to.
        /// </param>
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
            Output = outputHelper ?? new NullOutputHelper();
            app = new WebAppForTest<T>();
        }

        /// <summary>
        /// Please rewrite the method if you want to replace some of the components in the service
        /// collection.
        /// </summary>
        /// <param name="context">
        /// The web-host builder context from which you can get information.
        /// </param>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> object.
        /// </param>
        protected virtual void ConfigureServices(
            WebHostBuilderContext context,
            IServiceCollection services)
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

        /// <inheritdoc />
        /// <summary>
        /// Release all the resources in the test.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;
            Dispose(true);
            app.Dispose();
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
                    { TestServiceConfiguratorKeys.TestOutputHelper, Output }
                });
        }
    }
}