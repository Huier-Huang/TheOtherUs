using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Unity.IL2CPP;
using UnityEngine;

namespace TheOtherRoles.Utilities;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Class)]
public sealed class MonoRegisterAndDontDestroy : RegisterAttribute
{
    [Register]
    public static void Register(List<Type> findTypes)
    {
        var types = findTypes.Where(IsMono);

        foreach (var _type in types)
        {
            RegisterInIl2cpp(_type);
            IL2CPPChainloader.AddUnityComponent(_type);
        }
    }

    public static void RegisterInIl2cpp(Type type)
    {
        if (type.BaseType != null)
            RegisterInIl2cpp(type.BaseType);

        if (type == typeof(MonoBehaviour))
            return;
        
        if (ClassInjector.IsTypeRegisteredInIl2Cpp(type))
            return;
        
        ClassInjector.RegisterTypeInIl2Cpp(type);
    }

    public static bool IsMono(Type type)
    {
        var monoType = typeof(MonoBehaviour);
        if (type.BaseType == null)
            return false;

        return type.BaseType == monoType || IsMono(type.BaseType);
    }
}