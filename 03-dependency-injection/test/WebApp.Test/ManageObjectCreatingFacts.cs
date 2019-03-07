using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace WebApp.Test
{
    public class ManageObjectCreatingFacts
    {
        [Fact]
        public void should_create_object_per_get_service()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var instanceA = provider.GetService<Disposable>();
            var instanceB = provider.GetService<Disposable>();
            
            Assert.NotSame(instanceA, instanceB);
        }

        [Fact]
        public void should_create_only_one_object_per_scope()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            IServiceScope scope = provider.CreateScope();
            IServiceScope anotherScope = provider.CreateScope();

            var createdByScope = scope.ServiceProvider.GetService<Disposable>();
            var anotherCreatedByScope = scope.ServiceProvider.GetService<Disposable>();

            var createdByAnotherScope = anotherScope.ServiceProvider.GetService<Disposable>();
            
            Assert.Same(createdByScope, anotherCreatedByScope);
            Assert.NotSame(createdByScope, createdByAnotherScope);
        }

        [Fact]
        public void should_create_only_one_object_per_root_scope()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            IServiceScope scope = provider.CreateScope();
            IServiceScope childScope = scope.ServiceProvider.CreateScope();

            var instanceFromChildScope = childScope.ServiceProvider.GetService<Disposable>();
            var instanceFromScope = scope.ServiceProvider.GetService<Disposable>();
            var instanceFromRoot = provider.GetService<Disposable>();
            
            Assert.Same(instanceFromRoot, instanceFromScope);
            Assert.Same(instanceFromRoot, instanceFromChildScope);
        }
    }
}