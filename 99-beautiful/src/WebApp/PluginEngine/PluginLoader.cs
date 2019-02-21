using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApp.Plugins;

namespace WebApp.PluginEngine
{
    class PluginLoader
    {
        readonly ILogger logger;
        readonly IList<Type> moduleStartupClassTypes = new List<Type>();
        readonly IList<ModuleCreatingOption> moduleCreatingOptions;

        public PluginLoader(IConfiguration configuration, ILogger logger)
        {
            this.logger = logger;
            moduleCreatingOptions = new List<ModuleCreatingOption>
            {
                new ModuleCreatingOption(
                    new[] {typeof(IConfiguration), typeof(ILogger)},
                    new object[] {configuration, this.logger}),
                new ModuleCreatingOption(
                    new[] {typeof(IConfiguration)},
                    new object[] {configuration}),
                new ModuleCreatingOption(Array.Empty<Type>(), Array.Empty<object>())
            };
        }

        public PluginLoader AddPluginStartupClass(Type startupType)
        {
            if (!typeof(IModuleStartup).IsAssignableFrom(startupType))
            {
                throw new ArgumentException(
                    $"The module startup must implement \"{typeof(IModuleStartup).FullName}\"");
            }
            
            moduleStartupClassTypes.Add(startupType);
            return this;
        }

        public PluginLoader AddPluginStartupClasses(IEnumerable<Type> startupTypes)
        {
            foreach (Type type in startupTypes)
            {
                AddPluginStartupClass(type);
            }

            return this;
        }

        public IList<IModuleStartup> Load()
        {
            return moduleStartupClassTypes.Select(CreateModule).ToArray();
        }

        IModuleStartup CreateModule(Type type)
        {
            logger.LogDebug("Creating module {type}", type.FullName);
            
            ConstructorInfo[] constructors = type.GetConstructors();
            foreach (ModuleCreatingOption option in moduleCreatingOptions)
            {
                IModuleStartup module = TryCreateWithParameters(
                    constructors, 
                    option.ParameterTypes,
                    option.Arguments);
                if (module != null)
                {
                    logger.LogDebug(
                        "Creating module {type} using constructor with {parameterCount} parameter(s).",
                        type.FullName,
                        option.ParameterTypes.Length);
                    return module;
                }                
            }

            throw new ArgumentException(
                $"Cannot find supported constructor for {type.FullName}");
        }

        static IModuleStartup TryCreateWithParameters(
            IEnumerable<ConstructorInfo> constructorInfos, Type[] parameterTypes, object[] args)
        {
            ConstructorInfo constructor = constructorInfos
                .FirstOrDefault(c => ContainsParameter(c, parameterTypes));
            if (constructor == null) return null;
            return (IModuleStartup) constructor.Invoke(args);
        }

        static bool ContainsParameter(ConstructorInfo constructorInfo, params Type[] parameterTypes)
        {
            ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
            Type[] expectedParameterTypes = parameterTypes ?? Array.Empty<Type>();
            return parameterInfos.Select(p => p.ParameterType)
                .SequenceEqual(expectedParameterTypes);
        }
    }
}