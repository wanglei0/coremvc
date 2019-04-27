using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Resources;
using WebApp.Resources.Providers;
using WebApp.Resources.Repository;
using WebApp.Resources.Repository.Models;

namespace WebApp
{
    public class Autofac
    {
        readonly Action<ContainerBuilder> customContainer; 
        
        public IContainer RootScope { get; private set; }

        public Autofac(Action<ContainerBuilder> customContainer = null)
        {
            this.customContainer = customContainer;
        }

        public static IContainer Register(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();
            builder.RegisterType<UserRepository>().InstancePerDependency();
            builder.RegisterType<DatabaseSessionProvider>().As<IDatabaseSessionProvider>().OwnedByLifetimeScope();
            builder.RegisterType<DatabaseModel>().OwnedByLifetimeScope();
            builder.RegisterType<SqlStatementInterceptor>().SingleInstance();
            builder.RegisterType<User>().InstancePerDependency();
            builder.RegisterType<TableNameConvention>().SingleInstance();
            builder.Populate(services);
            return builder.Build();
        }
    }
}