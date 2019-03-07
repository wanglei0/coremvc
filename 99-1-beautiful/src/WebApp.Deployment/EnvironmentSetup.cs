using System;
using System.Reflection;

namespace WebApp.Deployment
{
    public class EnvironmentSetup : IEnvironmentSetup
    {
        public Type ConfiguratorType { get; }
        public Type StartupType { get; }

        EnvironmentSetup(Type webHostBuilderConfiguratorType, Type webHostStartupType)
        {
            EnsureWebHostBuilderConfiguratorInterface(webHostBuilderConfiguratorType);
            EnsureWebHostStartupInterface(webHostStartupType);
            
            ConfiguratorType = webHostBuilderConfiguratorType;
            StartupType = webHostStartupType;
        }

        static void EnsureWebHostStartupInterface(Type webHostStartupType)
        {
            if (IsNonAbstractTypeOf<IStartupForEnvironment>(webHostStartupType)) { return; }
            
            Type interfaceType = typeof(IStartupForEnvironment);
            throw new ArgumentException(
                $"The {webHostStartupType.FullName} must implement {interfaceType.FullName}. And should be able to instantiate.");
        }

        static void EnsureWebHostBuilderConfiguratorInterface(Type webHostBuilderConfiguratorType)
        {
            if (!IsNonAbstractTypeOf<IWebHostConfiguratorForEnvironment>(webHostBuilderConfiguratorType))
            {
                Type interfaceType = typeof(IWebHostConfiguratorForEnvironment);
                throw new ArgumentException(
                    $"The {webHostBuilderConfiguratorType.FullName} must implement {interfaceType.FullName}. And should be able to instantiate.");
            }

            ConstructorInfo defaultConstructor = webHostBuilderConfiguratorType.GetConstructor(Type.EmptyTypes);
            if (defaultConstructor == null)
            {
                throw new ArgumentException(
                    $"The {webHostBuilderConfiguratorType.FullName} must contains a default constructor.");
            }
        }

        static bool IsNonAbstractTypeOf<T>(Type type)
        {
            Type baseType = typeof(T);
            return baseType.IsAssignableFrom(type) &&
                   !type.IsAbstract &&
                   type.IsClass;
        }

        public static IEnvironmentSetup Create<TConfigurator, TStartup>()
        {
            return new EnvironmentSetup(
                typeof(TConfigurator), typeof(TStartup));
        }
    }
}