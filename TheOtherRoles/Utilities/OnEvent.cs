using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TheOtherRoles.Utilities;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
public class OnEvent(string EventName) : RegisterAttribute
{
    public string eventName = EventName;

    public static List<OnEvent> onEvents = [];

    [UsedImplicitly]
    public static void Register(Assembly assembly)
    {
        var methodInfos = assembly
            .GetTypes()
            .SelectMany(n => n.GetMethods(BindingFlags.Static))
            .Where(n => n.IsDefined(typeof(OnEvent)))
            .ToList();

        foreach (var method in methodInfos)
        {
            var OnEvent = method.GetCustomAttribute<OnEvent>();
            if (OnEvent == null) continue;
            OnEvent.method = method;
            onEvents.Add(OnEvent);
        }
    }
    
    #nullable enable
    public MethodInfo? method;
    public static void Call(string EventName, params object[] instances) =>
        onEvents.Where(n => n.eventName == EventName).Do(n => n.method?.Invoke(null, instances));
}