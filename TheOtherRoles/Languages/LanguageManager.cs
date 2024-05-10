using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AmongUs.Data.Legacy;
using BepInEx;
using HarmonyLib;

namespace TheOtherRoles.Modules.Languages;

#nullable enable
public class LanguageManager : ManagerBase<LanguageManager>
{
    private const string ResourcePath = "TheOtherRoles.Resources.Languages.";

    private static readonly HashSet<LanguageLoaderBase> DefLoaders =
    [
        new DataLoader(),
        new JsonLoader(),
        new CsvLoader(),
        new ExcelLoader()
    ];

    private static readonly HashSet<string> DefLanguageFile =
    [
        "Strings.csv",
        "strings.xlsx"
    ];

    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    private readonly List<LanguageLoaderBase> _AllLoader = [];
    internal SupportedLangs? CurrentLang;
    private Dictionary<SupportedLangs, Dictionary<string, string>> StringMap = null!;
    private bool Loaded;

    public LanguageManager()
    {
        _AllLoader.AddRange(DefLoaders);
        InitDic();
    }

    public void InitDic()
    {
        Info("init stringMap");
        StringMap = new Dictionary<SupportedLangs, Dictionary<string, string>>();
        foreach (var lang in TextHelper.LangNameDictionary.Keys)
        {
            StringMap[lang] = new Dictionary<string, string>();
        }
    }

    public LanguageLoaderBase? GetLoader(string extensionName)
    {
        return _AllLoader.FirstOrDefault(n => n.Filter.Contains(extensionName));
    }

    private bool TryGetResourceFile(string Path, out Stream? stream)
    {
        stream = null;
        if (!_assembly.GetManifestResourceNames().Contains(Path))
            return false;

        stream = _assembly.GetManifestResourceStream(Path);

        return true;
    }

    internal void LoadCustomLanguage()
    {
        var path = Path.Combine(Paths.GameRootPath, "CustomLanguages");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return;
        }

        var dir = new DirectoryInfo(path);
        foreach (var file in dir.GetFiles())
        {
            var Loader = GetLoader(file.Extension);
            if (Loader == null) continue;
            var stream = file.OpenRead();
            Loader.Load(this, stream, file.Name);
            stream.Close();
        }
    }

    internal void Load()
    {
        foreach (var FileName in DefLanguageFile)
        {
            var extension = Path.GetExtension(FileName);
            var Loader = GetLoader(extension);
            if (Loader == null || !TryGetResourceFile(ResourcePath + FileName, out var stream)) continue;
            Loader.Load(this, stream, FileName);
            stream?.Close();
        }

        LoadCustomLanguage();
        Loaded = true;
    }

    public void AddLoader(LanguageLoaderBase _loader)
    {
        _AllLoader.Add(_loader);
    }

    internal void LoadLanguage()
    {
        if (Loaded) return;
        CurrentLang ??= (SupportedLangs)LegacySaveManager.LastLanguage;
        Info($"Current Lang {CurrentLang}");
        Load();
    }

    internal void AddToMap(SupportedLangs lang, string key, string value, string loaderName)
    {
        Info($"AddToMap Lang:{lang} Key:{key} Value:{value} Loader:{loaderName}");
        StringMap[lang][key] = value;
    }

    internal string GetString(string Key)
    {
        if (!Loaded)
            LoadLanguage();

        if (CurrentLang == null)
            goto NullString;

        var lang = (SupportedLangs)CurrentLang;
        var langMap = StringMap[lang];
        if (!langMap.ContainsKey(Key))
            goto NullString;

        var str = langMap[Key];
        Info($"获取成功 Key:{Key} Value:{str} Language:{CurrentLang}");
        return str;

        NullString:
        Info($"获取失败 Key{Key} Language{CurrentLang}");
        return $"'{Key}'";
    }
}

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
}