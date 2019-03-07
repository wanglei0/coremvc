using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace WebApp.Test
{
    public class CreateMyWebHostBuilder
    {
        class MyWebHostBuilder
        {
            readonly List<Action<IServiceCollection>> addServiceDelegates =
                new List<Action<IServiceCollection>>();

            public ServiceCollection ServiceCollection { get; private set; }

            public MyWebHostBuilder ConfigureServices(Action<IServiceCollection> configure)
            {
                Action<IServiceCollection> action = 
                    configure ?? throw new ArgumentNullException(nameof(configure));
                addServiceDelegates.Add(action);
                return this;
            }

            public void Build()
            {
                ServiceCollection = new ServiceCollection();
                foreach (Action<IServiceCollection> action in addServiceDelegates)
                {
                    action(ServiceCollection);
                }

                ServiceProvider provider = ServiceCollection.BuildServiceProvider();
                
                // Do some validation
            }
        }
        
        [Fact]
        public void should_not_configure_service_when_adding_delegate()
        {
            MyWebHostBuilder builder = new MyWebHostBuilder()
                .ConfigureServices(r => { r.AddTransient<SomeType>(); })
                .ConfigureServices(r => { r.AddSingleton<SomeType>(); });
            
            builder.Build();

            Assert.Equal(
                ServiceLifetime.Singleton,
                builder.ServiceCollection.Last(d => d.ServiceType == typeof(SomeType)).Lifetime);
        }
    }

    class SomeType { }
} 