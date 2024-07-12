using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Unity.IL2CPP;

namespace TheOtherUs.Modules.Compatibility;

public sealed class CompatibilityManager : ListManager<CompatibilityManager, ICompatibility>
{
    public CompatibilityManager()
    {
        IL2CPPChainloader.Instance.PluginLoad += OnPluginLoad;
    }

    private void OnPluginLoad(PluginInfo arg1, Assembly arg2, BasePlugin arg3)
    {
        if (List.TryGet(n => n.GUID == arg1.Metadata.GUID, out var compatibility))
            compatibility.OnLoad(arg1, arg2, arg3);
    }

    public CompatibilityManager Use<T>() where T : ICompatibility, new()
    {
        if (List.Exists(n => n is T))
            return this;
        
        List.Add(new T());
        return this;
    }

    public T GetCompatibility<T>() where T : class, ICompatibility, new()
    {
        Use<T>();
        return List.First(n => n is T) as T;
    }

    public void UnUse<T>() where T : ICompatibility
    {
        if (!List.TryGet(n => n is T, out var value)) return;
        value.UnUse();
        List.Remove(value);
    }

    #nullable enable
    public CompatibilityManager DisableHarmony(string PluginId, string HarmonyName = "Harmony")
    {
        if (!IL2CPPChainloader.Instance.Plugins.TryGetValue(PluginId, out var plugin)) return this;
        var _harmony = plugin.Instance.GetType().GetField(HarmonyName)?.GetValue(plugin.Instance) as Harmony;
        _harmony?.UnpatchSelf();
        return this;
    }
}