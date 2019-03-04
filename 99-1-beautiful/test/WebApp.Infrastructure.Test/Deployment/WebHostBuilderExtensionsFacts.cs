using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Deployment;
using WebApp.Infrastructure.Test.Deployment.Helpers;
using Xunit;

namespace WebApp.Infrastructure.Test.Deployment
{
    public class WebHostBuilderExtensionsFacts
    {
        [Theory]
        [InlineData("Development", "DevStartup")]
        [InlineData("Production", "ProdStartup")]
        public void should_call_different_startup_for_specific_environment_using_type(
            string environmentName,
            string selectedStartup)
        {
            var recorder = new SimpleRecorder();

            new WebHostBuilder().UseEnvironment(environmentName)
                .ConfigureServices(s => s.AddSingleton(recorder))
                .ConfigureEnvironment
                (
                    ("Development", EnvironmentSetup.Create<NullWebHostConfigurator, DevStartup>()),
                    ("Production", EnvironmentSetup.Create<NullWebHostConfigurator, ProdStartup>())
                )
                .Build();

            Assert.Equal(
                new[] {$"{selectedStartup}.ConfigureServices"},
                recorder.Records);
        }

        [Fact]
        public void should_be_override()
        {
            var recorder = new SimpleRecorder();
            string environmentName = EnvironmentName.Development;

            new WebHostBuilder().UseEnvironment(environmentName)
                .ConfigureServices(s => s.AddSingleton(recorder))
                .ConfigureEnvironment
                (
                    (environmentName, EnvironmentSetup.Create<NullWebHostConfigurator, DevStartup>())
                )
                .UseStartup<ProdStartup>()
                .Build();
            
            Assert.Equal("ProdStartup.ConfigureServices", recorder.Records.Single());
        }

        [Theory]
        [InlineData("SameEnvironment", "SameEnvironment")]
        [InlineData("IgnoreCase", "IGNORECASE")]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void should_fail_if_duplicated_environment_specified(
            string environmentName, string sameEnvironmentName)
        {
            IWebHostBuilder builder = new WebHostBuilder();

            Assert.Throws<ArgumentException>(
                () => builder.ConfigureEnvironment(
                    (environmentName,
                        EnvironmentSetup
                            .Create<NullWebHostConfigurator, DevStartup>()),
                    (sameEnvironmentName,
                        EnvironmentSetup
                            .Create<NullWebHostConfigurator, DevStartup>())));
        }

        [Fact]
        public void should_fail_if_builder_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                () => ((IWebHostBuilder) null).ConfigureEnvironment(
                    (EnvironmentName.Development,
                        EnvironmentSetup
                            .Create<NullWebHostConfigurator, NullStartupForEnvironment>())));
        }

        [Fact]
        public void should_fail_if_config_is_null()
        {
            IWebHostBuilder builder = new WebHostBuilder();
            
            Assert.Throws<ArgumentNullException>(
                () => builder.ConfigureEnvironment(((string, Action<IWebHostBuilder>, Type)[])null));
            Assert.Throws<ArgumentNullException>(
                () => builder.ConfigureEnvironment(((string, IEnvironmentSetup)[]) null));
        }

        [Fact]
        public void should_fail_if_config_is_not_fully_provided()
        {
            IWebHostBuilder builder = new WebHostBuilder();
            
            Assert.Throws<ArgumentException>(
                () => builder.ConfigureEnvironment(
                    (null, wb => { }, typeof(NullStartupForEnvironment))));
            Assert.Throws<ArgumentException>(
                () => builder.ConfigureEnvironment(("EnvironmentName", null, typeof(NullStartupForEnvironment))));
            Assert.Throws<ArgumentException>(
                () => builder.ConfigureEnvironment(("EnvironmentName", wb => { }, null)));
            
            Assert.Throws<ArgumentException>(
                () => builder.ConfigureEnvironment(("EnvironmentName", null)));
            Assert.Throws<ArgumentException>(
                () => builder.ConfigureEnvironment(
                    (null,
                        EnvironmentSetup
                            .Create<NullWebHostConfigurator, NullStartupForEnvironment>())));
        }

        [Fact]
        public void should_fail_if_no_startup_is_selected()
        {
            IWebHostBuilder builder = new WebHostBuilder().UseEnvironment(EnvironmentName.Staging)
                .ConfigureEnvironment(
                    ("Development",
                        EnvironmentSetup
                            .Create<NullWebHostConfigurator, NullStartupForEnvironment>()),
                    ("Production",
                        EnvironmentSetup
                            .Create<NullWebHostConfigurator, NullStartupForEnvironment>()));

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }
        
        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public void should_call_different_configure_service(string environmentName)
        {
            var recorder = new SimpleRecorder();

            new WebHostBuilder()
                .UseEnvironment(environmentName)
                .ConfigureEnvironment(
                    (
                        EnvironmentName.Development, 
                        wb => wb
                            .ConfigureServices(
                                (c, s) => recorder.Record("Development: ConfigureServices(context, services)"))
                            .ConfigureServices(
                                s => recorder.Record("Development: ConfigureServices(services)")),
                        typeof(NullStartupForEnvironment)),
                    (
                        EnvironmentName.Production,
                        wb => wb
                            .ConfigureServices(
                                (c, s) => recorder.Record("Production: ConfigureServices(context, services)"))
                            .ConfigureServices(s => recorder.Record("Production: ConfigureServices(services)")),
                        typeof(NullStartupForEnvironment)
                    ))
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
                .ConfigureEnvironment(
                    (
                        EnvironmentName.Development,
                        wb => wb.ConfigureAppConfiguration(
                            (c, cb) => recorder.Record("Development: ConfigureAppConfiguration(context, builder)")),
                        typeof(NullStartupForEnvironment)
                    ),
                    (
                        EnvironmentName.Production,
                        wb => wb.ConfigureAppConfiguration(
                            (c, cb) => recorder.Record("Production: ConfigureAppConfiguration(context, builder)")),
                        typeof(NullStartupForEnvironment)
                    ))
                .Build();

            Assert.Equal(
                $"{environmentName}: ConfigureAppConfiguration(context, builder)",
                recorder.Records.Single());
        }

        [Fact]
        public void should_be_ok_to_get_settings_under_different_environments()
        {
            string valueFromDevEnv = null;
            string valueFromProdEnv = null;

            new WebHostBuilder()
                .UseSetting("key", "value")
                .ConfigureEnvironment(
                    (EnvironmentName.Development, wb => valueFromDevEnv = wb.GetSetting("key"),
                        typeof(NullStartupForEnvironment)),
                    (EnvironmentName.Production, wb => valueFromProdEnv = wb.GetSetting("key"),
                        typeof(NullStartupForEnvironment)))
                .Build();
            
            Assert.Equal("value", valueFromDevEnv);
            Assert.Equal("value", valueFromProdEnv);
        }

        [Fact]
        public void should_throw_when_use_settings_under_certain_environment()
        {
            IWebHostBuilder builder = new WebHostBuilder();

            Assert.Throws<InvalidOperationException>(
                () => builder.ConfigureEnvironment(
                    (EnvironmentName.Development, wb => wb.UseSetting("key", "value"),
                        typeof(NullStartupForEnvironment))));
        }

        [Fact]
        public void should_throw_when_nesting_environment()
        {
            IWebHostBuilder builder = new WebHostBuilder();

            Assert.Throws<InvalidOperationException>(
                () =>
                    builder.ConfigureEnvironment(
                        (
                            EnvironmentName.Development,
                            wb => wb.ConfigureEnvironment(
                                (
                                    EnvironmentName.Production,
                                    anotherBuilder => { },
                                    typeof(NullStartupForEnvironment)
                                )),
                            typeof(NullStartupForEnvironment))));
        }

        [Fact]
        public void should_throw_when_calling_build_from_environment_specific_web_builder()
        {
            IWebHostBuilder builder = new WebHostBuilder();

            Assert.Throws<InvalidOperationException>(
                () => builder.ConfigureEnvironment(
                    (EnvironmentName.Development, wb => wb.Build(), typeof(NullStartupForEnvironment))));
        }

        class TestStartupBase : IStartupForEnvironment
        {
            readonly SimpleRecorder recorder;

            public TestStartupBase(SimpleRecorder recorder)
            {
                this.recorder = recorder;
            }

            public void Configure(IApplicationBuilder app, IServiceProvider scopedProvider)
            {
                recorder.Record($"{GetType().Name}.{nameof(Configure)}");
            }

            public void ConfigureServices(IServiceCollection services)
            {
                recorder.Record($"{GetType().Name}.{nameof(ConfigureServices)}");
            }
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class DevStartup : TestStartupBase
        {
            public DevStartup(SimpleRecorder recorder) : base(recorder) { }
        }

        class ProdStartup : TestStartupBase
        {
            public ProdStartup(SimpleRecorder recorder) : base(recorder) { }
        }
    }
}