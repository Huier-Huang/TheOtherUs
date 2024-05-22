using TheOtherUs.Utilities;

namespace TheOtherUs.Patches;

public static class EventsPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd)), HarmonyPostfix]
    private static void OnGameEnd() => OnEvent.Call("OnGameEnd");
}