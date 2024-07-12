using System.Reflection;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using UnityEngine;

namespace TheOtherUs.Modules.Compatibility;

public interface ICompatibility
{
    public string GUID { get; set; }

    public virtual void OnLoad(PluginInfo pluginInfo, Assembly assembly, BasePlugin plugin)
    {
    }
    
    public virtual void UnUse()
    {
    }
}

public static class CompatibilityExtension
{
    public static MonoBehaviour AddSubmergedComponent(this GameObject gameObject, string typeName)
    {
        return CompatibilityManager.Instance.GetCompatibility<SubmergedCompatibility>()
            .AddSubmergedComponent(gameObject, typeName);
    }
}