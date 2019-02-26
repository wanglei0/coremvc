using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Deployment;
using Xunit;

namespace WebApp.Infrastructure.Test.Deployment
{
    public class EnvironmentAwareStartupFacts
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
                .ConfigureLogging(lb => { })
                .UseEnvironmentAwareStartup(
                    ("Development", typeof(DevStartup)),
                    ("Production", typeof(ProdStartup)))
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
                .ConfigureLogging(lb => { })
                .UseEnvironmentAwareStartup((environmentName, typeof(DevStartup)))
                .UseStartup<ProdStartup>()
                .Build();

            Assert.Equal("ProdStartup.ConfigureServices", recorder.Records.Single());
        }

        [Theory]
        [InlineData("SameEnvironment", "SameEnvironment")]
        [InlineData("IgnoreCase", "IGNORECASE")]
        public void should_fail_if_duplicated_environment_specified(
            string environmentName, string sameEnvironmentName)
        {
            IWebHostBuilder builder = new WebHostBuilder();

            Assert.Throws<ArgumentException>(
                () => builder.UseEnvironmentAwareStartup(
                    (environmentName, typeof(DevStartup)),
                    (sameEnvironmentName, typeof(DevStartup))));
        }

        [Fact]
        public void should_fail_if_builder_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                () => ((IWebHostBuilder)null).UseEnvironmentAwareStartup(
                    (EnvironmentName.Development, typeof(DevStartup))));
        }

        [Fact]
        public void should_fail_if_config_is_null()
        {
            IWebHostBuilder builder = new WebHostBuilder();
            
            Assert.Throws<ArgumentNullException>(
                () => builder.UseEnvironmentAwareStartup(null));
        }

        [Fact]
        public void should_fail_if_config_is_not_fully_provided()
        {
            IWebHostBuilder builder = new WebHostBuilder();
            
            Assert.Throws<ArgumentException>(
                () => builder.UseEnvironmentAwareStartup(
                    (null, typeof(DevStartup))));
            
            Assert.Throws<ArgumentException>(
                () => builder.UseEnvironmentAwareStartup(
                    ("ContainsEnvironment", null)));
        }

        [Fact]
        public void should_fail_if_no_startup_is_selected()
        {
            var recorder = new SimpleRecorder();

            IWebHostBuilder builder = new WebHostBuilder().UseEnvironment(EnvironmentName.Staging)
                .ConfigureServices(s => s.AddSingleton(recorder))
                .ConfigureLogging(lb => { })
                .UseEnvironmentAwareStartup(
                    ("Development", typeof(DevStartup)),
                    ("Production", typeof(ProdStartup)));

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        class TestStartupBase : IEnvironmentSpecificStartup
        {
            readonly SimpleRecorder recorder;

            public TestStartupBase(SimpleRecorder recorder)
            {
                this.recorder = recorder;
            }

            public void Configure(IApplicationBuilder app) { recorder.Record($"{GetType().Name}.{nameof(Configure)}"); }

            public void ConfigureServices(IServiceCollection services)
            {
                recorder.Record($"{GetType().Name}.{nameof(ConfigureServices)}");
            }
        }

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