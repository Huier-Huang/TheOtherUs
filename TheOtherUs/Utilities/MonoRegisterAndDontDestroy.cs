using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Unity.IL2CPP;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace TheOtherUs.Utilities;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Class)]
public sealed class MonoRegisterAndDontDestroy : RegisterAttribute
{
    public static Dictionary<Type, MonoBehaviour> RegisterObjects = [];

    public static T GetRegister<T>() where T : MonoBehaviour
    {
        var type = typeof(T);
        if (RegisterObjects.TryGetValue(type, out var mono))
        {
            return mono as T;
        }

        return Register(type) as T;
    }

    public static MonoBehaviour Register(Type _type)
    {
        RegisterInIl2cpp(_type);
        var obj = (IL2CPPChainloader.AddUnityComponent(_type) as MonoBehaviour)!.DontDestroyOnLoad();
        RegisterObjects.Add(_type, obj);
        return obj;
    }
    
    [Register]
    public static void Register(List<Type> findTypes)
    {
        var types = findTypes.Where(IsMono);

        foreach (var _type in types)
        {
            Register(_type);
        }
    }

    public static void RegisterInIl2cpp(Type type)
    {
        if (type.BaseType != null)
            RegisterInIl2cpp(type.BaseType);
        
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