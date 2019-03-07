using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace WebApp.Test
{
    public class DisposeObjectFacts
    {
        [Fact]
        public void should_dispose_object_when_root_provider_is_disposed()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            var instance = provider.GetService<Disposable>();

            // When
            provider.Dispose();

            // Then
            Assert.True(instance.IsDisposed);
        }

        [Fact]
        public void should_dispose_all_disposable_objects_when_root_provider_is_disposed()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            var instances = Enumerable
                .Range(0, 3)
                .Select(_ => provider.GetService<Disposable>())
                .ToArray();

            // When
            provider.Dispose();

            // Then
            Assert.True(instances.All(item => item.IsDisposed));
        }
        
        [Fact]
        public void should_dispose_object_when_scope_is_disposed()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            IServiceScope scope = provider.CreateScope();

            var instance = scope.ServiceProvider.GetService<Disposable>();

            // When
            scope.Dispose();

            // Then
            Assert.True(instance.IsDisposed);
        }

        [Fact]
        public void should_dispose_all_disposable_objects_when_scope_is_disposed()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            IServiceScope scope = provider.CreateScope();

            var instances = Enumerable
                .Range(0, 3)
                .Select(_ => scope.ServiceProvider.GetService<Disposable>())
                .ToArray();

            // When
            scope.Dispose();

            // Then
            Assert.True(instances.All(item => item.IsDisposed));
        }

        [Fact]
        public void should_not_dispose_object_created_by_another_scope()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            IServiceScope scope = provider.CreateScope();
            IServiceScope anotherScope = provider.CreateScope();

            var createdByScope = scope.ServiceProvider.GetService<Disposable>();
            var createdByAnotherScope = anotherScope.ServiceProvider.GetService<Disposable>();

            // When
            scope.Dispose();
            
            // Then
            Assert.True(createdByScope.IsDisposed);
            Assert.False(createdByAnotherScope.IsDisposed);
        }

        [Fact]
        public void should_not_dispose_object_created_by_child_scope()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            IServiceScope scope = provider.CreateScope();
            IServiceScope childScope = scope.ServiceProvider.CreateScope();

            var createdByScope = scope.ServiceProvider.GetService<Disposable>();
            var createdByChildScope = childScope.ServiceProvider.GetService<Disposable>();
            
            // When
            scope.Dispose();
            
            // Then
            Assert.True(createdByScope.IsDisposed);
            Assert.False(createdByChildScope.IsDisposed);
        }

        [Fact]
        public void should_not_dispose_object_even_if_root_provider_is_disposed()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            IServiceScope scope = provider.CreateScope();

            var createdByRootProvider = provider.GetService<Disposable>();
            var createdByScope = scope.ServiceProvider.GetService<Disposable>();
            
            // When
            provider.Dispose();
            
            // Then
            Assert.True(createdByRootProvider.IsDisposed);
            Assert.False(createdByScope.IsDisposed);
        }

        [Fact]
        public void should_not_dispose_singleton_object_created_in_child_scope()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            IServiceScope scope = provider.CreateScope();
            var instance = scope.ServiceProvider.GetService<Disposable>();
            
            // When
            scope.Dispose();
            
            // Then
            Assert.False(instance.IsDisposed);
        }

        [Fact]
        public void should_not_get_service_if_root_provider_is_disposed()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            IServiceScope scope = provider.CreateScope();
            
            // When
            provider.Dispose();
            
            // Then
            Assert.Throws<ObjectDisposedException>(
                () => scope.ServiceProvider.GetService<Disposable>());
        }

        [Fact]
        public void should_not_dispose_singleton_instance_from_child_scope()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            IServiceScope scope = provider.CreateScope();

            // When
            var disposable = scope.ServiceProvider.GetService<Disposable>();
            scope.Dispose();
            
            // Then
            Assert.False(disposable.IsDisposed);
        }
        
        [Fact]
        public void should_dispose_singleton_instance_from_root_scope()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<Disposable>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            IServiceScope scope = provider.CreateScope();

            // When
            var disposable = scope.ServiceProvider.GetService<Disposable>();
            provider.Dispose();
            
            // Then
            Assert.True(disposable.IsDisposed);
        }
    }
}