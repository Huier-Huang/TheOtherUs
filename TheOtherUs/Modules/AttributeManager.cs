extern alias JetBrains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TheOtherUs.Modules;

#nullable enable
public class AttributeManager : ManagerBase<AttributeManager>
{
    private readonly Dictionary<Type, MethodInfo> _methodInfos = [];
    private readonly Dictionary<Type, object[]> CreateTargets = [];
    private Assembly? targetAssembly;
    private Assembly? GetAssembly;
    private List<Type> _types = [];
    private List<MethodInfo> _methods = [];
    private List<ConstructorInfo> _constructors = [];
    private List<EventInfo> _events = [];
    private List<FieldInfo> _fields = [];
    

    
    public AttributeManager SetInit(Assembly? assembly = null)
    {
        targetAssembly = assembly;
        GetAssembly = targetAssembly ?? Assembly.GetCallingAssembly();
        _types = GetAssembly.GetTypes().ToList();
        _methods = _types.SelectMany(n => n.GetMethods()).ToList();
        _constructors = _types.SelectMany(n => n.GetConstructors()).ToList();
        _events = _types.SelectMany(n => n.GetEvents()).ToList();
        _fields = _types.SelectMany(n => n.GetFields()).ToList();
        
        Info($"types {_types.Count}");
        foreach (var type in _types.Where(n => n.IsSubclassOf(typeof(RegisterAttribute))))
        {
            Info($"ISSUB {type}");
            foreach (var method in type.GetMethods().Where(n => n.IsDefined(typeof(RegisterAttribute))))
            {
                Info($"Add {type} {method}");
                    _methodInfos.Add(type, method);
            }
        }

        return this;
    }

    public void Start()
    {
        targetAssembly ??= Assembly.GetCallingAssembly();
        foreach (var (type, objects) in CreateTargets)
        {
            try
            {
                if (!_methodInfos.TryGetValue(type, out var method)) continue;
                var arguments = method.GetParameters().Select(n => n.ParameterType).ToList();
                Info($"Method {method.Name} {arguments}");
                if (arguments.Count == 0)
                {
                    method.Invoke(null, null);
                    continue;
                }
                
                if (arguments[0] == typeof(Assembly))
                {
                    var arg = new List<object> { targetAssembly };
                    arg.AddRange(objects);
                    method.Invoke(null, arg.ToArray());
                    Info($"Invoke {method.Name}");
                }

                if (targetAssembly != GetAssembly) continue;
                if (arguments[0] == typeof(List<Type>))
                {
                    var types = _types.Where(n => n.IsDefined(type)).ToList();
                    var arg = new List<object> { types };
                    arg.AddRange(objects);

                    method.Invoke(null, arg.ToArray());
                }
                
                if (arguments[0] == typeof(List<MethodInfo>))
                {
                    var types = _methods.Where(n => n.IsDefined(type)).ToList();
                    var arg = new List<object> { types };
                    arg.AddRange(objects);

                    method.Invoke(null, arg.ToArray());
                }
            }
            catch (Exception e)
            {
                Exception(e);
            }
        }
    }

    public AttributeManager Add<T>(params object[] Instances)
    {
        return Add(typeof(T), Instances);
    }

    public AttributeManager Add(Type type, params object[] Instances)
    {
        CreateTargets.Add(type, Instances);
        return this;
    }
}

[MeansImplicitUse]
public class RegisterAttribute : Attribute;