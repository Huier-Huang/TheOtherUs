using System.Collections.Generic;

namespace TheOtherUs.Chat.Patches;

[Harmony]
internal static class ChatControllerPatch
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat)), HarmonyPrefix]
    private static bool ChatControllerSendChatPatch(ChatController __instance)
    {
        var strings = __instance.freeChatField.Text.ToLower().Split(string.Empty);
        if (strings[0][0] != '/')
            return true;
        
        var command = strings[0].Remove(0);
        var context = new List<string>();
        var index = 1;
        foreach (var str in strings)
        {
            if (index != 1)
                context.Add(str);
            index++;
        }
        
        if (CommandManager.Instance.Clear)
        {
            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            CommandManager.Instance.Clear = false;
            return false;
        }
        return true;
    }
}