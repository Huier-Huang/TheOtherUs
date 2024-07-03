#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TheOtherUs.Helper;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal class RPCMethod(CustomRPC rpc) : RegisterAttribute
{
    public static readonly List<RPCMethod> _AllRPCMethod =
        [];

    private readonly CustomRPC RPC = rpc;

    private Type[] _types = [];

    public Action<object[]>? Start;

    private int count => _types.Length;

    [Register]
    public static void Register(Assembly assembly)
    {
        var types = assembly.GetTypes().SelectMany(n => n.GetMethods(BindingFlags.Static))
            .Where(n => n.IsDefined(typeof(RPCMethod)));
        types.Do(n =>
        {
            var method = n.GetCustomAttribute<RPCMethod>();
            if (method == null) return;
            method.Start = objs => n.Invoke(null, objs);
            method._types = n.GetGenericArguments();
            _AllRPCMethod.Add(method);
        });
    }

    public bool Match(object[] objects)
    {
        if (objects.Length != count) return false;
        for (var i = 0; i < count; i++)
            if (objects[i].GetType() != _types[i])
                return false;
        return true;
    }

    public static void StartRPCMethod(CustomRPC rpc, params object[] objects)
    {
        _AllRPCMethod.Where(n => n.RPC == rpc && n.Match(objects)).Do(n => n.Start?.Invoke(objects));
    }
}