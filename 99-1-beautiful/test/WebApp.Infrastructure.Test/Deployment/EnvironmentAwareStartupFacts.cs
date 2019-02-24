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
        public void should_call_different_startup_for_specific_environment(
            string environmentName,
            string selectedStartup)
        {
            var recorder = new SimpleRecorder();

            new WebHostBuilder().UseEnvironment(environmentName)
                .ConfigureServices(s => s.AddSingleton(recorder))
                .ConfigureLogging(lb => { })
                .UseEnvironmentAwareStartup(
                    (h => h.IsDevelopment(), sp => new DevStartup(sp.GetService<SimpleRecorder>())),
                    (h => h.IsProduction(), sp => new ProdStartup(sp.GetService<SimpleRecorder>())))
                .Build();

            Assert.Equal(
                new[] {$"{selectedStartup}.ConfigureServices"},
                recorder.Records);
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