using Il2CppSystem;

namespace TheOtherUs.Patches;

[Harmony]
public static class GameWinPatch 
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.FixedUpdate)), HarmonyPrefix]
    private static bool GameManager_FixedUpdatePatch(GameManager __instance)
    {
        if (__instance.GameHasStarted)
            foreach (var Component in __instance.LogicComponents)
                Component.FixedUpdate();
        
        if (CachedPlayer.GameStates.IsHost && __instance.ShouldCheckForGameEnd)
            WinManager.Instance.CheckWin();
        
        return false;
    }
    
    [HarmonyPatch(typeof(NormalGameManager), nameof(NormalGameManager.InitComponents)), HarmonyPostfix]
    private static void NormalGameManager_InitComponentsPatch(NormalGameManager __instance)
    {
        var logicComponent = __instance.LogicComponents.Find((Predicate<GameLogicComponent>)find);
        __instance.LogicComponents.Remove(logicComponent);
        return;

        bool find(GameLogicComponent component)
        {
            return component is LogicGameFlowNormal;
        }
    }
}