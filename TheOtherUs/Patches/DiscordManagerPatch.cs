using Discord;

namespace TheOtherUs.Patches;

public static class DiscordManagerPatch
{
    // Form TOHE and TOU
    [HarmonyPatch(typeof(ActivityManager), nameof(ActivityManager.UpdateActivity)), HarmonyPrefix]
    private static void DiscordPatchPreFix([HarmonyArgument(0)] Activity activity)
    { 
        if (activity == null) return;

        var details = $"{Main.Name} {Main.Version}";
        try
        {
            if (activity.State == "In Menus") return;
            var maxSize = GameOptionsManager.Instance.CurrentGameOptions.MaxPlayers;
            if (CachedPlayer.GameStates.IsLobby)
            {
                var lobbyCode = GameStartManager.Instance.GameRoomNameCode.text;
                var region = ServerManager.Instance.CurrentRegion.Name;

                details += $"{lobbyCode} {MapOptions.gameMode} {region} {maxSize}";
            }

            activity.Details = details;
        }
        catch
        {
            // 
            Info("Discord SetError");
        }
    }
}