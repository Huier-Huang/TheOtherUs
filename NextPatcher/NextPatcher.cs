using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Preloader.Core.Patching;


namespace NextPatcher;

[PatcherPluginInfo("NextPatcher","NextPatcher","1.0.0")]
public class NextPatcher : BasePatcher
{
    public static ManualLogSource LogSource = null!;
    public const string FileName = "Dependents.toml";
    public static NextPatcher Instance = null!;
    
    public DirectoryCreator Creator { get; set; }
    
    public NextScriptManager ScriptManager { get; set; } = null!;

    public NextPatcher()
    {
        LogSource = Log;
        Instance = this;
        Creator = new DirectoryCreator(Paths.GameRootPath, "Dependents", "dotnet-8.0.0", "NextScripts");
        Creator.Create();
        DownLoadAndLoadToml();
        WriteAndSaveDotnet();
    }
    

    public async void WriteAndSaveDotnet()
    {
        var path = Path.Combine(Creator.Get("dotnet-8.0.0"), "net8.0.0-win-x86.zip");
        if (File.Exists(path))
            return;
        await using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NextPatcher.Resources.dotnet-runtime-8.0.0-win-x86.zip");
        if (stream == null)
            return;
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        foreach (var entry in archive.Entries)
        {
            if (!entry.FullName.EndsWith(".dll") && !entry.FullName.EndsWith(".version")) continue;
            var NewPath = Path.Combine(Creator.Get("dotnet-8.0.0"), entry.Name);
            if (File.Exists(NewPath)) continue;
            await using var file = File.Create(NewPath);
            await entry.Open().CopyToAsync(file);
        }

        await using var fileStream = File.Create(path);
        await stream.CopyToAsync(fileStream);
    }
    
    public override void Initialize()
    {
        AppDomain.CurrentDomain.AssemblyResolve += LocalResolve;
        ScriptManager = new NextScriptManager();
        ScriptManager
            .SetFindDir(Creator.Get("NextScripts"))
            .BuildAll();
    }

    public async void DownLoadAndLoadToml(string version = "0.17.0", string Framework = "net7.0")
    {
        try
        {
            var path = Path.Combine(Creator.Get("Dependents"), "Tomlyn.dll");
            if (File.Exists(path))
                return;
            using var download = new NuGetDownloader("Tomlyn", version);
            await using var stream = await download.Download();
            if (stream == Stream.Null) return;
            using var file = new ZipArchive(stream, ZipArchiveMode.Read);
            var entry = file.GetEntry($"lib/{Framework}/Tomlyn.dll");
            if (entry == null) return;
            await using var NewStream = File.Create(path);
            await stream.CopyToAsync(NewStream); 
        }
        catch (Exception e)
        {
            LogSource.LogError(e);
        }
    }

    public static readonly HashSet<string> RootDownloads =
        [
            "https://dotnetcli.azureedge.net/dotnet",
            "https://dotnetbuilds.azureedge.net/public"
        ];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">Runtime,aspnetcore,WindowsDesktop,Sdk</param>
    /// <param name="version"></param>
    /// <param name="platform">win,osx,linux,linux-bionic</param>
    /// <param name="arch">
    /// x86,x64,arm32,arm64,x64-alpine,arm64-alpine,arm32-alpine
    /// </param>
    /// <returns>FileStream</returns>
    public string GetDownLoadDotnetUrl(
        string type = "Runtime",
        string version = "8.0.0",
        string platform = "win",
        string arch = "x86"
        )
    {
        var fileRoot = type switch
        {
            "Runtime" => "dotnet-runtime",
            "aspnetcore" => "aspnetcore-runtime",
            "WindowsDesktop" => "windowsdesktop-runtime",
            "Sdk" => "dotnet-sdk",
            _ => string.Empty
        };
        var ex = platform == "win" ? ".zip" : ".tar.gz";
        return fileRoot == string.Empty ? string.Empty : $"https://dotnetcli.azureedge.net/dotnet/{type}/{version}/{fileRoot}-{version}-{platform}-{arch}{ex}";
    }
    
    
    internal static Assembly? LocalResolve(object? sender, ResolveEventArgs args)
    {
        if (args.Name.Contains("Il2CppSystem")) return null;
        var assemblyName = new AssemblyName(args.Name);
        LogSource.LogInfo($"Resolve {assemblyName.Name} {assemblyName.FullName}");
        if (assemblyName.Version?.Major == 8)
        {
            var net8Path = Path.Combine(Instance.Creator.Get("dotnet-8.0.0"), assemblyName.Name + ".dll");
            if (File.Exists(net8Path))
            {
                LogSource.LogInfo($"is Net8 {assemblyName}");
                return Assembly.LoadFile(net8Path);
            }
        }

        var path = Path.Combine(Instance.Creator.Get("Dependents"), assemblyName.Name + ".dll");
        return File.Exists(path) ? Assembly.LoadFile(path) : null;
    }
}