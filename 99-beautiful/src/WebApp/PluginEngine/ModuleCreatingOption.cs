using System;

namespace WebApp.PluginEngine
{
    class ModuleCreatingOption
    {
        public Type[] ParameterTypes { get; }
        public object[] Arguments { get; }

        public ModuleCreatingOption(Type[] parameterTypes, object[] arguments)
        {
            ParameterTypes = parameterTypes;
            Arguments = arguments;
        }
    }
}