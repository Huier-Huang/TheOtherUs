using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using NextPatcher;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using TheOtherUs.Chat.Patches;
using TheOtherUs.CustomCosmetics;
using TheOtherUs.Languages;
using TheOtherUs.Modules.Compatibility;
using TheOtherUs.Patches;

namespace TheOtherUs;

[BepInAutoPlugin("TheOtherUs.MengChu.Next")]
[BepInProcess("Among Us.exe")]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
// ReSharper disable once ClassNeverInstantiated.Global
public partial class TheOtherRolesPlugin : BasePlugin
{
    public static readonly Assembly MainAssembly = typeof(Main).Assembly;
    public static readonly Version version = System.Version.Parse(Version);
    public static Main Instance;

    public static NextPatcher.NextPatcher Patcher => NextPatcher.NextPatcher.Instance;
    private static readonly HashSet<(string, string)> CheckPaths = 
        [
            (Paths.GameRootPath, "Data")
        ];
    
    public static readonly List<string> NoLoads = [];

    public static readonly string ModEx = ".NexDat";
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
        CheckNextPatcher();
        CheckPath();
        
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

    private static void CheckNextPatcher()
    {
        var path = Path.Combine(Paths.PatcherPluginPath, "NextPatcher.dll");
        if (File.Exists(path))
            return;

        using var stream = ResourceHelper.ResourcePath.AddSplit("NextPatcher.dll").GetResStream();
        using var NewFile = File.Create(path);
        stream?.CopyTo(NewFile);
        Assembly.LoadFile(path);
        new NextPatcher.NextPatcher().Initialize();
    }

    private static void DownLoadDependent()
    {
        var nugetName = MainAssembly.GetFramework().GetNugetName();
        Patcher.CheckAndAdd(
            (nameof(Csv), "2.0.93", nugetName), 
            (nameof(YamlDotNet), "15.3.0", nugetName),
            ("EPPlus", "7.2.0", nugetName)
            );
    }

    private static void CheckPath()
    {
        foreach (var (root, name) in CheckPaths)
        {
            try
            {
                var path = Path.Combine(root, name);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Info($"Create Directory {path}");
                }
            
                Info($"Check Path Root:{root} Name:{name} End");
            }
            catch
            {
                // ignored
            }
        }
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
        InitLogFile("NextLog");
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
        TaskQueue.GetOrCreate()
            .StartTask(ChatCensorPatch.AddCensorWord, "AddCensorWord")
            .StartTask(DIYColor.LoadDIYColor, "LoadDiskDIYColor");
        AttributeManager.Instance
                    .SetInit(MainAssembly)
                    .Add<ManagerBaseLoad>(TaskQueue.GetOrCreate())
                    /*.Add<MonoRegisterAndDontDestroy>()*/
                    .Add<RegisterRole>(_RoleManager)
                    .Add<OnEvent>()
                    .Add<RPCMethod>()
                    .Add<RPCListener>()
                    .Start();

            /*.StartTask(CosmeticsManager.Instance.DefConfigCreateAndInit, "DefConfigCreate")
            .StartTask(SoundEffectsManager.Load, "LoadSoundEffect")*/
            
        Info("Start Main Task");
    }

    internal static void OnTranslationController_Initialized_Load()
    {
        LanguageManager.Instance.Load();
            /*.StartTask(AnnouncementManager.Instance.DownLoadREADME, "DownloadREADME")
            .StartTask(AnnouncementManager.Instance.DownloadAnnouncements, "DownLoadAnnouncements")
            .StartTask(AnnouncementManager.Instance.DownloadMOTDs, "DownLoadMOTDs");*/
        DIYColor.SetColors();
        Info("OnTranslationController_Initialized_Load End");
    }
}