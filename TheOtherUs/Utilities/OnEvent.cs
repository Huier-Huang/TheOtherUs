using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TheOtherUs.Utilities;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
public class OnEvent(string EventName) : RegisterAttribute
{
    public static List<OnEvent> onEvents = [];
    public string eventName = EventName;

    [Register]
    public static void Register(List<MethodInfo> methodInfos)
    {
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

    public static void Call(string EventName, params object[] instances)
    {
        onEvents.Where(n => n.eventName == EventName).Do(n => n.method?.Invoke(null, instances));
    }
}