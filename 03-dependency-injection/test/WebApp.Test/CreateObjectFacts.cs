using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace WebApp.Test
{
    public class CreateObjectFacts
    {
        class IndependentType {}
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class UnregisteredType {}

        [Fact]
        public void should_return_null_if_not_registered() 
        {
            // Given
            var serviceCollection = new ServiceCollection();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            // When
            var instance = provider.GetService<UnregisteredType>();

            // Then
            Assert.Null(instance);
        }

        [Fact]
        public void should_throw_if_dependency_cannot_be_resolved()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<WithDependency>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            // When
            Assert.Throws<InvalidOperationException>(() => provider.GetService<WithDependency>());
        }
        
        [Fact]
        public void should_create_object_using_delegate_registration()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(notUsed => new IndependentType());
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            // When
            var instance = provider.GetService<IndependentType>();
            
            // Then
            Assert.Same(typeof(IndependentType), instance.GetType());
        }

        [Fact]
        public void should_create_object_using_type_registration()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IndependentType>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            // When
            var instance = provider.GetService<IndependentType>();
            
            // Then
            Assert.Same(typeof(IndependentType), instance.GetType());
        }

        [Fact]
        public void should_create_new_instance_per_call_for_transient_scope()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IndependentType>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            // When
            var instance = provider.GetService<IndependentType>();
            var another = provider.GetService<IndependentType>();
            
            // Then
            Assert.NotSame(instance, another);
        }

        [Fact]
        public void should_create_one_instance_per_scope()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IndependentType>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            // When
            var instance = provider.GetService<IndependentType>();
            var another = provider.GetService<IndependentType>();
            
            // Then
            Assert.Same(instance, another);
        }

        [Fact]
        public void should_create_one_instance_per_root_scope()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IndependentType>();
            ServiceProvider rootScopeProvider = serviceCollection.BuildServiceProvider();
            IServiceScope childScope = rootScopeProvider.CreateScope();
            IServiceScope anotherScope = rootScopeProvider.CreateScope();

            // When
            var instance = childScope.ServiceProvider.GetService<IndependentType>();
            var another = anotherScope.ServiceProvider.GetService<IndependentType>();
            var theThird = rootScopeProvider.GetService<IndependentType>();
            
            // Then
            Assert.Same(instance, another);
            Assert.Same(another, theThird);
        }

        class WithDependency
        {
            public IndependentType Dependency { get; }

            public WithDependency(IndependentType dependency)
            {
                Dependency = dependency;
            }
        }

        [Fact]
        public void should_create_instance_with_dependency_using_delegate_registration()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(_ => new IndependentType());
            serviceCollection.AddTransient(p => new WithDependency(p.GetService<IndependentType>()));
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            // When
            var instance = provider.GetService<WithDependency>();
            
            // Then
            Assert.Same(typeof(WithDependency), instance.GetType());
            Assert.Same(typeof(IndependentType), instance.Dependency.GetType());
        }

        [Fact]
        public void should_create_instance_with_dependency_using_type_registration()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IndependentType>();
            serviceCollection.AddTransient<WithDependency>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            
            // When
            var instance = provider.GetService<WithDependency>();
            
            // Then
            Assert.Same(typeof(WithDependency), instance.GetType());
            Assert.Same(typeof(IndependentType), instance.Dependency.GetType());
        }

        interface ICommon {}
        class CommonImplA : ICommon {}
        class CommonImplB : ICommon {}

        [Fact]
        public void should_replace_type()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ICommon, CommonImplA>();
            serviceCollection.AddTransient<ICommon, CommonImplB>();

            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var instance = provider.GetService<ICommon>();
            Assert.True(instance is CommonImplB);
        }
        
        [Fact]
        public void should_get_all_type_instances()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ICommon, CommonImplA>();
            serviceCollection.AddTransient<ICommon, CommonImplB>();

            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var instances = provider.GetService<IEnumerable<ICommon>>();
            Assert.Equal(2, instances.Count());
        }
    }
}