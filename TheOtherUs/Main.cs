using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using TheOtherUs.CustomCosmetics;
using TheOtherUs.Languages;
using TheOtherUs.Modules.Compatibility;
using TheOtherUs.Patches;

namespace TheOtherUs;

[BepInAutoPlugin("me.spex.theotherus")]
[BepInProcess("Among Us.exe")]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
// ReSharper disable once ClassNeverInstantiated.Global
public partial class TheOtherRolesPlugin : BasePlugin
{
    public static readonly Version version = System.Version.Parse(Version);
    public static Main Instance;

    public static readonly List<string> NoLoads = [];
    
    public Harmony Harmony { get; private set; }

    // This is part of the Mini.RegionInstaller, Licensed under GPLv3
    // file="RegionInstallPlugin.cs" company="miniduikboot">
    public static void UpdateRegions()
    {
        var serverManager = FastDestroyableSingleton<ServerManager>.Instance;
        var regions = serverManager.AvailableRegions;
        var region = UnityHelper.CreateHttpRegion("Custom", TheOtherUsConfig.Ip, TheOtherUsConfig.Port);
        
        Info($"Add{region} regions:{regions.Length}");
        serverManager.AddOrUpdateRegion(region);
    }

    public override void Load()
    {
        SetConsole();
        
        if (!CheckNoLoad())
            return;
        
        CreateInstance();
        
        TheOtherUsConfig.Bind(Config);
        MainMenuPatch.addSceneChangeCallbacks();
        /*AddToKillDistanceSetting.addKillDistance();*/
        
        UpdateRegions();
        DownLoadDependent();
        StartMainTask();
        
        Info($"Loading {Name} completed!");
    }

    private void CheckUpdate()
    {
        /*if (BepInExUpdater.UpdateRequired)
        {
            AddComponent<BepInExUpdater>();
            return;
        }

        AddComponent<ModUpdater>();*/
    }

    private static void DownLoadDependent()
    {
        DependentDownload.Instance.CheckLoad();
        DependentDownload.Instance.DownLoadDependentMap("https://raw.githubusercontent.com/SpexGH/TheOtherUs/the-other-us/LoadDependent/");
    }

    private void SetCompatibility()
    {
        CompatibilityManager.Instance
            .Use<SubmergedCompatibility>()
            .DisableHarmony("MalumMenu");
    }

    private void SetConsole()
    {
        if (ConsoleManager.ConsoleEnabled) 
            System.Console.OutputEncoding = Encoding.UTF8;
        SetLogSource(Log);
        InitConsole();
    }

    private void CreateInstance()
    {
        Instance = this;
        Harmony = new Harmony(Id);
        Harmony.PatchAll();
        Info("Create Instance");
    }

    private static bool CheckNoLoad()
    {
        var list = IL2CPPChainloader.Instance.Plugins;
        return NoLoads.All(noLoad => !list.ContainsKey(noLoad));
    }

    private static void StartMainTask()
    {
        TaskQueue.GetOrCreate(1)
            .StartTask(DIYColor.LoadDIYColor, "LoadDiskDIYColor")
            .StartTask(DIYColor.SetColors, "SetColor")
            .StartTask(() => DependentDownload.Instance.DownLoadDependentFormMap("Csv"), "LoadDependentFormMap Csv")
            .StartTask(() => DependentDownload.Instance.DownLoadDependentFormMap("Excel"), "LoadDependentFormMap Excel")
            .StartTask(() =>
            {
                AttributeManager.Instance
                    .SetInit()
                    .Add<ManagerBaseLoad>(TaskQueue.GetOrCreate(1))
                    .Add<MonoRegisterAndDontDestroy>()
                    .Add<RegisterRole>(_RoleManager)
                    .Add<OnEvent>()
                    .Add<RPCMethod>()
                    .Add<RPCListener>()
                    .Start();
            }, "RegisterAttributes")
            .StartTask(CosmeticsManager.Instance.DefConfigCreateAndInit, "DefConfigCreate")
            .StartTask(SoundEffectsManager.Load, "LoadSoundEffect");
        Info("Start Main Task");
    }

    internal static void OnTranslationController_Initialized_Load()
    {
        TaskQueue.GetOrCreate(2)
            .StartTask(AnnouncementManager.Instance.DownLoadREADME, "DownloadREADME")
            .StartTask(AnnouncementManager.Instance.DownloadAnnouncements, "DownLoadAnnouncements")
            .StartTask(AnnouncementManager.Instance.DownloadMOTDs, "DownLoadMOTDs")
            .StartTask(LanguageManager.Instance.Load, "LoadLanguage");
            /*.StartTask(CustomOptionHolder.Load, "LoadOption");*/
        
        Info("OnTranslationController_Initialized_Load End");
    }
}