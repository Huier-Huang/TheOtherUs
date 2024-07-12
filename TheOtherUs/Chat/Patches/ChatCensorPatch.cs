using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;

namespace TheOtherUs.Chat.Patches;

[Harmony]
internal static class ChatCensorPatch
{
    public static string FilePath(string langName) => Path.Combine(Paths.GameRootPath, "Data", $"CensorWord_{langName}.txt");
    private static readonly Dictionary<SupportedLangs, HashSet<string>> censorTextDictionary = [];

    internal static void AddCensorWord()
    {
        foreach (var lang in Enum.GetValues<SupportedLangs>())
        {
            var name = Enum.GetName(lang);
            if (!File.Exists(FilePath(name))) continue;
            censorTextDictionary[lang] = [];
            using var stream = File.OpenText(FilePath(name));
            while (!stream.EndOfStream)
            {
                var word = stream.ReadLine()?.Trim();
                if (!word.IsNullOrWhiteSpace())
                    censorTextDictionary[lang].Add(word);
            }
        }
    }

    [HarmonyPatch(typeof(BlockedWords), nameof(BlockedWords.SetLanguage)), HarmonyPostfix]
    private static void BlockedWords_SetLanguage_Postfix(TranslatedImageSet newLang)
    {
        if (!censorTextDictionary.TryGetValue(newLang.languageID, out var set)) return;
        foreach (var word in set)
            
        {
            BlockedWords.SkipList.AddWord(word);
        }
    }
}