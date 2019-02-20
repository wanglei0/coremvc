using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace WebApp.PluginEngine
{
    public class InternalControllerFeatureProvider : ControllerFeatureProvider
    {
        const string ControllerTypeNameSuffix = "Controller";
        
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            // We only consider public and internal top-level classes as controllers. IsPublic
            // and IsNonPublic returns false for nested classes, regardless of visibility modifiers.
            if (!typeInfo.IsPublic && !typeInfo.IsNotPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(
                    ControllerTypeNameSuffix,
                    StringComparison.OrdinalIgnoreCase) &&
                !typeInfo.IsDefined(typeof(ControllerAttribute)))
            {
                return false;
            }

            return true;
        }
    }
}