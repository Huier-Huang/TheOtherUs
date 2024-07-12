#nullable enable
using System.Linq;
using Il2CppSystem;
using TheOtherUs.Chat;

namespace TheOtherUs.Languages;

[Harmony]
internal static class LanguageExtension
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.Initialize))]
    [HarmonyPostfix]
    private static void OnTranslationController_Initialized_Load(TranslationController __instance)
    {
        LanguageManager.Instance.CurrentLang = __instance.currentLanguage.languageID;
        Main.OnTranslationController_Initialized_Load();
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.SetLanguage))]
    [HarmonyPrefix]
    private static void OnTranslationController_SetLanguage([HarmonyArgument(0)] TranslatedImageSet lang)
    {
        LanguageManager.Instance.CurrentLang = lang.languageID;
    }

    internal static string Translate(this string key)
    {
        return LanguageManager.Instance.GetString(key);
    }

    internal static string GetString(this StringNames name, Object[]? objects = null)
    {
        var array = objects ?? Array.Empty<Object>();
        return TranslationController.Instance.GetString(name, array);
    }

    internal static string Get(string node, string NextNode)
    {
        var Roots = node.Split('.').ToList();
        Roots.Add(NextNode);
        return Get(Roots.ToArray(), null);
    }
    internal static string Get(string s) => Get(s.Split('.'));
    internal static string Get(params string[] strings) => Get(strings, null);
    
    internal static string Get(string[] strings, TextEnvironment? environment = null)
    {
        TranslateNode? node = null;
        var count = 1;
        foreach (var str in strings)
        {
            node = count == 1 ? LanguageManager.Instance._translateNodes.FirstOrDefault(n => n.Id == str) : node?._nodes?.FirstOrDefault(n => n.Id == str);
            
            if (node == null)
                return string.Empty;

            count++;
        }

        return node?.Def ?? string.Empty;
    }

    internal static string[] GetStrings(string[] strings, TextEnvironment? environment = null)
    {
        return new string[] { };
    }
}