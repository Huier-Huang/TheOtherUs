using System.Reflection;
using BepInEx;
using BepInEx.Unity.IL2CPP;

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