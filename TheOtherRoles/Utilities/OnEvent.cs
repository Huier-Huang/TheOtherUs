using System;
using System.Reflection;

namespace TheOtherRoles.Utilities;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
public class OnEvent(string EventName) : RegisterAttribute
{
    public string eventName = EventName;

    public static void Register(Assembly assembly)
    {
        
    }
}