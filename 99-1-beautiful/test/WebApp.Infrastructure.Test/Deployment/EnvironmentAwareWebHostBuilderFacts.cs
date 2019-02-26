using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using WebApp.Deployment;
using WebApp.Infrastructure.Test.Deployment.Helpers;
using Xunit;

namespace WebApp.Infrastructure.Test.Deployment
{
    public class EnvironmentAwareWebHostBuilderFacts
    {
        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public void should_call_different_configure_service(string environmentName)
        {
            var recorder = new SimpleRecorder();

            new WebHostBuilder()
                .UseEnvironment(environmentName)
                .WithWebHostBuilder(
                    EnvironmentName.Development,
                    wb => wb
                        .ConfigureServices(
                            (c, s) => recorder.Record("Development: ConfigureServices(context, services)"))
                        .ConfigureServices(
                            s => recorder.Record("Development: ConfigureServices(services)")))
                .WithWebHostBuilder(
                    EnvironmentName.Production,
                    wb => wb
                        .ConfigureServices(
                            (c, s) => recorder.Record("Production: ConfigureServices(context, services)"))
                        .ConfigureServices(s => recorder.Record("Production: ConfigureServices(services)")))
                .UseStartup<NullStartup>()
                .Build();
            
            Assert.Equal(
                new[]
                {
                    $"{environmentName}: ConfigureServices(context, services)",
                    $"{environmentName}: ConfigureServices(services)"
                },
                recorder.Records);
        }
        
        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public void should_call_different_configure_application_config_according_to_environment(
            string environmentName)
        {
            var recorder = new SimpleRecorder();

            new WebHostBuilder()
                .UseEnvironment(environmentName)
                .WithWebHostBuilder(
                    EnvironmentName.Development,
                    wb => wb.ConfigureAppConfiguration(
                        (c, cb) => recorder.Record("Development: ConfigureAppConfiguration(context, builder)")))
                .WithWebHostBuilder(
                    EnvironmentName.Production,
                    wb => wb.ConfigureAppConfiguration(
                        (c, cb) => recorder.Record("Production: ConfigureAppConfiguration(context, builder)")))
                .UseStartup<NullStartup>()
                .Build();

            Assert.Equal(
                $"{environmentName}: ConfigureAppConfiguration(context, builder)",
                recorder.Records.Single());
        }

        [Fact]
        public void should_execute_all_satisfied_web_host_builder_configuration()
        {
            var recorder = new SimpleRecorder();

            new WebHostBuilder()
                .UseEnvironment(EnvironmentName.Development)
                .WithWebHostBuilder(
                    EnvironmentName.Development,
                    wb => wb.ConfigureServices(s => recorder.Record("ConfigureServices for Development - 1")))
                .WithWebHostBuilder(
                    EnvironmentName.Development,
                    wb => wb.ConfigureServices(s => recorder.Record("ConfigureServices for Development - 2")))
                .WithWebHostBuilder(
                    EnvironmentName.Production,
                    wb => wb.ConfigureServices(s => recorder.Record("ConfigureServices for Production")))
                .UseStartup<NullStartup>()
                .Build();

            Assert.Equal(
                new[] {"ConfigureServices for Development - 1", "ConfigureServices for Development - 2"},
                recorder.Records);
        }

        [Fact]
        public void should_be_ok_to_get_settings_under_different_environments()
        {
            string valueFromDevEnv = null;
            string valueFromProdEnv = null;

            new WebHostBuilder()
                .UseSetting("key", "value")
                .WithWebHostBuilder(
                    EnvironmentName.Development, wb => valueFromDevEnv = wb.GetSetting("key"))
                .WithWebHostBuilder(
                    EnvironmentName.Production, wb => valueFromProdEnv = wb.GetSetting("key"));
            
            Assert.Equal("value", valueFromDevEnv);
            Assert.Equal("value", valueFromProdEnv);
        }

        [Fact]
        public void should_throw_when_use_settings_under_certain_environment()
        {
            IWebHostBuilder builder = new WebHostBuilder();

            Assert.Throws<InvalidOperationException>(
                () => builder.WithWebHostBuilder(
                    EnvironmentName.Development,
                    wb => wb.UseSetting("key", "value")));
        }

        [Fact]
        public void should_throw_when_nesting_environment()
        {
            IWebHostBuilder builder = new WebHostBuilder();

            Assert.Throws<InvalidOperationException>(
                () => builder.WithWebHostBuilder(
                    EnvironmentName.Development,
                    wb => wb.WithWebHostBuilder(
                        EnvironmentName.Production,
                        anotherBuilder => { })));
        }

        [Fact]
        public void should_throw_when_calling_build_from_environment_specific_web_builder()
        {
            IWebHostBuilder builder = new WebHostBuilder();

            Assert.Throws<InvalidOperationException>(
                () => builder.WithWebHostBuilder(
                    EnvironmentName.Development,
                    wb => wb.Build()));
        }
    }
}