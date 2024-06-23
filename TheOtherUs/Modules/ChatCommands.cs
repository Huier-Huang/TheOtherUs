using System.Linq;
using InnerNet;
using TheOtherUs.CustomGameMode;

namespace TheOtherUs.Modules;

[HarmonyPatch]
public static class ChatCommands
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    private static class SendChatPatch
    {
        private static bool Prefix(ChatController __instance)
        {
            var text = __instance.freeChatField.Text;
            var handled = false;
            if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started)
            {
                var strings = text.ToLower().Split(string.Empty);
                var Command = strings[0].Replace("/", string.Empty);

                switch (Command)
                {
                    case "kick":
                    case "ban":
                        var playerName = strings[1];
                        PlayerControl target =
                            CachedPlayer.AllPlayers.FirstOrDefault(x => x.NetPlayerInfo.PlayerName.Equals(playerName));
                        if (target != null && AmongUsClient.Instance != null && AmongUsClient.Instance.CanBan())
                        {
                            var client = AmongUsClient.Instance.GetClient(target.OwnerId);
                            if (client != null)
                            {
                                AmongUsClient.Instance.KickPlayer(client.Id, Command == "ban");
                                handled = true;
                            }
                        }

                        break;

                    case "gm":
                        var mode = strings[1];
                        CustomGameModes? gameMode = mode switch
                        {
                            "prop" or "ph" => CustomGameModes.PropHunt,
                            "guess" or "gm" => CustomGameModes.Guesser,
                            "hide" or "hn" => CustomGameModes.HideNSeek,
                            _ => null
                        };

                        if (gameMode == null) break;

                        if (AmongUsClient.Instance.AmHost)
                        {
                            var writer = FastRpcWriter.StartNewRpcWriter(CustomRPC.ShareGamemode)
                                .Write(gameMode);
                            writer.RPCSend();
                            RPCProcedure.shareGamemode((byte)gameMode);
                        }
                        else
                        {
                            __instance.AddChat(CachedPlayer.LocalPlayer.Control,
                                "Nice try, but you have to be the host to use this feature");
                        }

                        handled = true;
                        break;
                }
            }

            if (!handled) return true;
            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();

            return false;
        }
    }

    /*[HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class EnableChat
    {
        public static void Postfix(HudManager __instance)
        {
            if (!__instance.Chat.isActiveAndEnabled && (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay ||
                                                        (CachedPlayer.LocalPlayer.Control.isLover() &&
                                                         Lovers.enableChat) ||
                                                        CachedPlayer.LocalPlayer.Control.isTeamCultist()))
                __instance.Chat.SetVisible(true);

            if (Multitasker.multitasker.FindAll(x => x.PlayerId == CachedPlayer.LocalPlayer.PlayerId).Count > 0 ||
                MapOptions.transparentTasks)
            {
                if (PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Disconnected) return;
                if (!Minigame.Instance) return;

                var Base = Minigame.Instance as MonoBehaviour;
                SpriteRenderer[] rends = Base.GetComponentsInChildren<SpriteRenderer>();
                for (var i = 0; i < rends.Length; i++)
                {
                    var oldColor1 = rends[i].color[0];
                    var oldColor2 = rends[i].color[1];
                    var oldColor3 = rends[i].color[2];
                    rends[i].color = new Color(oldColor1, oldColor2, oldColor3, 0.5f);
                }
            }
        }
    }

    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetName))]
    public static class SetBubbleName
    {
        public static void Postfix(ChatBubble __instance, [HarmonyArgument(0)] string playerName)
        {
            var sourcePlayer = PlayerControl.AllPlayerControls.ToArray().ToList()
                .FirstOrDefault(x => x.Data != null && x.Data.PlayerName.Equals(playerName));
            if (CachedPlayer.LocalPlayer != null && CachedPlayer.LocalPlayer.Data.Role.IsImpostor &&
                ((Spy.spy != null && sourcePlayer.PlayerId == Spy.spy.PlayerId) ||
                 (Sidekick.sidekick != null && Sidekick.wasTeamRed &&
                  sourcePlayer.PlayerId == Sidekick.sidekick.PlayerId) ||
                 (Jackal.jackal != null && Jackal.wasTeamRed && sourcePlayer.PlayerId == Jackal.jackal.PlayerId)) &&
                __instance != null) __instance.NameText.color = Palette.ImpostorRed;
        }
    }*/

    /*[HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))] //test
    public static class AddChat
    {
        public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer)
        {
            var playerControl = CachedPlayer.LocalPlayer.Control;
            var flag = MeetingHud.Instance != null || LobbyBehaviour.Instance != null || playerControl.Data.IsDead ||
                       sourcePlayer.PlayerId == CachedPlayer.LocalPlayer.PlayerId;
            if (__instance != FastDestroyableSingleton<HudManager>.Instance.Chat) return true;
            if (playerControl == null) return true;
            /* brb
            if (playerControl == Detective.detective)
            {
                return flag;
            }
            #1#
            if (!playerControl.isTeamCultist() && !playerControl.isLover()) return flag;
            if ((playerControl.isTeamCultist() && Follower.chatTarget) ||
                (playerControl.isLover() && Lovers.enableChat) ||
                (playerControl.isTeamCultistAndLover() && !Follower.chatTarget))
                return sourcePlayer.getChatPartner() == playerControl ||
                       playerControl.getChatPartner() == playerControl == (bool)sourcePlayer || flag;
            return flag;
        }
    }*/
}