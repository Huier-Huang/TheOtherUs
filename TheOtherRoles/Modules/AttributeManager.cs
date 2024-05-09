using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TheOtherRoles.Modules;

#nullable enable
public class AttributeManager : ManagerBase<AttributeManager>
{
    private readonly Dictionary<Type, object[]> CreateTargets = [];
    private readonly Dictionary<Type, MethodInfo> _methodInfos = [];
    private Assembly? targetAssembly;

    public AttributeManager Set(Assembly assembly)
    {
        targetAssembly = assembly;
        return this;
    }

    public AttributeManager Init()
    {
        var types = Assembly.GetCallingAssembly().GetTypes().Where(n => n.IsSubclassOf(typeof(RegisterAttribute)));
        foreach (var type in types)
        {
            var method =  type.GetMethods(BindingFlags.Static).FirstOrDefault(n => n.Name == "Register");
            if (method != null)
                _methodInfos.Add(type, method);
        }
        return this;
    }

    public void Start()
    {
        targetAssembly ??= Assembly.GetCallingAssembly();
        foreach (var (type, objects) in CreateTargets)
        {
            var arg = new List<object> { targetAssembly };
            arg.AddRange(objects);
            if (_methodInfos.TryGetValue(type, out var method) && method.GetGenericArguments().Length == arg.Count)
                method.Invoke(null, arg.ToArray());
        }
    }

    public AttributeManager Add<T>(params object[] Instances) => Add(typeof(T), Instances);

    public AttributeManager Add(Type type, params object[] Instances)
    {
        CreateTargets.Add(type, Instances);
        return this;
    }
}

public class RegisterAttribute : Attribute;