
using System.Linq;
using UnityEngine;

namespace TheOtherUs.CustomCosmetics.Patches;

[HarmonyPatch(typeof(CosmeticsCache))]
internal static class CosmeticsCachePatches
{
    
    [HarmonyPatch(typeof(CosmeticData), nameof(CosmeticData.SetPreview)), HarmonyPrefix]
    private static bool SetPre(CosmeticData __instance, SpriteRenderer renderer, int color)
    {
        if (!CosmeticsManager.Instance.TryGet(__instance.ProductId, out var data)) return true;
        renderer.sprite = data.Resource;
        if (Application.isPlaying)
            PlayerMaterial.SetColors(color, renderer);
        return false;
    }
    
    [HarmonyPatch(typeof(CosmeticData), nameof(CosmeticData.GetItemName)), HarmonyPrefix]
    private static bool ItemNam(CosmeticData __instance, ref string __result)
    {
        var data = CosmeticsManager.Instance.CustomCosmetics.FirstOrDefault(n => n.Id == __instance.ProductId);
        if (data == null) return true;
        __result = data.config.Name;
        return false;
    }
    
    
    [HarmonyPatch(nameof(CosmeticsCache.GetHat)), HarmonyPrefix]
    private static bool GetHatPrefix(string id, ref HatViewData __result)
    {
        Info($"trying to load hat {id} from cosmetics cache");
        return !CosmeticsManager.Instance.TryGetHatView(id, out __result);
    }
    
    [HarmonyPatch(nameof(CosmeticsCache.GetVisor)), HarmonyPrefix]
    private static bool GetVisorPrefix(string id, ref VisorViewData __result)
    {
        Info($"trying to load hat {id} from cosmetics cache");
        return !CosmeticsManager.Instance.TryGetVisorView(id, out __result);
    }
    
    
    [HarmonyPatch(nameof(CosmeticsCache.GetNameplate)), HarmonyPrefix]
    private static bool GetNamePlatePrefix(string id, ref NamePlateViewData __result)
    {
        Info($"trying to load hat {id} from cosmetics cache");
        return !CosmeticsManager.Instance.TryGetNamePlateView(id, out __result);
    }
    
    [HarmonyPatch(typeof(CosmeticsCache._CoAddHat_d__12), nameof(CosmeticsCache._CoAddHat_d__12.MoveNext)), HarmonyPrefix]
    private static bool _CoAddHat_d__12Prefix(CosmeticsCache._CoAddHat_d__12 __instance, ref bool __result)
    {
        var id = __instance.id;
        if (CosmeticsManager.Instance.CustomHats.All(n => n.Id != id))
            return true;
        __result = true;
        return false;
    }
    
    
    [HarmonyPatch(typeof(CosmeticsCache._CoAddVisor_d__10), nameof(CosmeticsCache._CoAddVisor_d__10.MoveNext)), HarmonyPrefix]
    private static bool __CoAddVisor_d__10Prefix(CosmeticsCache._CoAddVisor_d__10 __instance, ref bool __result)
    {
        var id = __instance.visorId;
        if (CosmeticsManager.Instance.CustomVisors.All(n => n.Id != id))
            return true;
        __result = true;
        return false;
    }
    
    [HarmonyPatch(typeof(CosmeticsCache._CoAddNameplate_d__8), nameof(CosmeticsCache._CoAddNameplate_d__8.MoveNext)), HarmonyPrefix]
    private static bool _CoAddNameplate_d__8Prefix(CosmeticsCache._CoAddNameplate_d__8 __instance, ref bool __result)
    {
        var id = __instance.namePlateId;
        if (CosmeticsManager.Instance.CustomNamePlates.All(n => n.Id != id))
            return true;
        __result = true;
        return false;
    }
}