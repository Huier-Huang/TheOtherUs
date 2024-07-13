using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Preloader.Core.Patching;
using BepInEx.Unity.IL2CPP;
using Cpp2IL.Core.Extensions;
using Tomlyn;


namespace NextPatcher;

[PatcherPluginInfo("NextPatcher","NextPatcher","1.0.0")]
public class NextPatcher : BasePatcher
{
    public static ManualLogSource LogSource = null!;
    public const string FileName = "Dependents.toml";
    public static NextPatcher Instance = null!;
    public static bool UserAutoDownloadNoFind = false;
    public DirectoryCreator Creator { get; set; }
    
    public NextScriptManager ScriptManager { get; set; } = null!;

    public NextPatcher()
    {
        LogSource = Log;
        Instance = this;
        Creator = new DirectoryCreator(Paths.GameRootPath, "Dependents", "dotnet-8.0.0", "NextScripts");
        Creator.Create();
        DownLoadAndLoadToml();
    }

    public async void ReadConfigToml()
    {
        var path = Path.Combine(Paths.GameRootPath, FileName);
        await using var file = File.Open(path, FileMode.OpenOrCreate);
        var toml = Toml.Parse(file.ReadBytes());
    }

    public Stream GetFormNetZip(int version, string Name)
    {
        var path = Path.Combine(Creator.Get($"dotnet-{version}.0.0"), $"net{version}.0.0-win-x86.zip");
        if (!File.Exists(path)) return Stream.Null;
        using var archive = new ZipArchive(File.OpenRead(path), ZipArchiveMode.Read);
        var entry = archive.Entries.FirstOrDefault(n => n.FullName.EndsWith($"{Name}.dll"));
        return entry == null ? Stream.Null : entry.Open();
    }
    
    public async void WriteAndSaveDotnet(string url, string version)
    {
        var dotnetName = $"dotnet-{version}";
        var dir = Creator.Get(dotnetName);
        if (!Directory.Exists(dir))
            Creator.Add(dotnetName).Create();
        
        var path = Path.Combine(Creator.Get($"dotnet-{version}"), $"net{version}-win-x86.zip");
        if (File.Exists(path))
            return;

        using var client = new HttpClient();
        await using var stream = await client.GetStreamAsync(url);
        if (stream == Stream.Null)
            return;
        
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        foreach (var entry in archive.Entries)
        {
            if (!entry.FullName.EndsWith(".dll") && !entry.FullName.EndsWith(".version")) continue;
            var NewPath = Path.Combine(Creator.Get(dotnetName), entry.Name);
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
        ScriptManager = new NextScriptManager(Creator.Get("NextScripts"));
        ScriptManager
            .BuildAll();
    }

    public override void Finalizer()
    {
        AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
    }
    
    private static readonly (int, string)[] RuntimeURLs = 
        [
            (7, "https://download.visualstudio.microsoft.com/download/pr/f479b75e-9ecb-42ea-8371-c94f411eda8d/0cd700d75f1d04e9108bc4213f8a41ec/dotnet-runtime-7.0.20-win-x86.zip"),
            (8, "https://download.visualstudio.microsoft.com/download/pr/3e0c1889-b4f7-414c-9ac9-cdc82938563d/daed61ae792654223bcac886ff3725ba/dotnet-runtime-8.0.7-win-x86.zip"),
        ];

    private void OnAssemblyLoad(object? sender, AssemblyLoadEventArgs args)
    {
        var version = args.LoadedAssembly.GetFramework().GetFrameworkVersion();
        foreach (var (ver, url) in RuntimeURLs)
        {
            if (version.StartsWith(ver.ToString()))
                WriteAndSaveDotnet(url, $"{ver}.0.0");
        }
    }

    public void CheckAndAdd(params (string, string, string)[] all)
    {
        foreach (var (name, version, framework) in all)
        {
            var path = Path.Combine(Creator.Get("Dependents"), $"{name}.dll");
            if (File.Exists(path) || AppDomain.CurrentDomain.GetAssemblies().Any(n => n.GetName().Name == name)) continue;
            var dllStream = Download(name, version, framework);
            LogSource.LogInfo($"{name} IsNull{dllStream == Stream.Null}");
            if (dllStream == Stream.Null) continue;
            var file = File.Create(path);
            dllStream.CopyTo(file);
            file.Close();
            dllStream.Close();
            Assembly.LoadFile(path);
        }
    }
    
    public Stream Download(string name, string version = "", string framework = "")
    {
        var getFramework = framework == string.Empty
            ? Assembly.GetCallingAssembly().GetFramework().GetNugetName()
            : framework;
        if (name == string.Empty) return Stream.Null;
        using var downloader = new NuGetDownloader(name, version);
        var get = new NugetZipGet(downloader);
        var frameworks = get.GetFrameworks();
        var GetFramework = NextPatcher.GetFramework(frameworks, getFramework);
        var Dependency = get.GetDependency(GetFramework);
        if (Dependency.Count != 0)
            foreach (var d in Dependency)
            {
                CheckAndAdd((d.Id, d.Version, framework));
            }
        
        LogSource.LogInfo($"{name} GetFramework: " + GetFramework);
        return GetFramework == string.Empty ? Stream.Null : get.GetAssemblyStream(GetPathFramework(GetFramework));
    }

    public static string GetPathFramework(string framework)
    {
        var path = framework.ToLower();
        return path[0] == '.' ? path.Remove(0) : path;
    }
    
    public static string GetFramework(string[] frameworks, string framework)
    {
        if (frameworks.Contains(framework)) return framework;
        var ver = framework.Substring(framework.LastIndexOf('.') - 1, 3);
        var versionInt = ToVersionInt(framework);
        var frameworkName = framework.Replace(ver, string.Empty);
        var fs = frameworks.Where(n => n.StartsWith(frameworkName)).ToList();
        if (fs.Any())
        {
            foreach (var sf in from sf in fs let sVersionInt = ToVersionInt(sf) where sVersionInt <= versionInt select sf)
            {
                return sf;
            }
        }

        if (!framework.StartsWith("net")) return string.Empty;
        var Standards = frameworks.Where(n => n.StartsWith(".NETStandard")).ToList();
        if (!Standards.Any()) return string.Empty;
        Standards.Sort((x, y) => ToVersionInt(x).CompareTo(ToVersionInt(y)));
        return Standards.Last();

        int ToVersionInt(string versionString)
        {
            return int.Parse(versionString.Substring(versionString.LastIndexOf('.') - 1, 3).Replace(".", string.Empty));
        }
    }
    
    public void DownLoadAndLoadToml(string version = "0.17.0")
    {
        CheckAndAdd(("Tomlyn", version, typeof(NextPatcher).Assembly.GetFramework().GetNugetName()));
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
        LogSource.LogDebug($"Resolve {assemblyName.Name} {assemblyName.FullName}");
        if (assemblyName.Version != null 
            && !assemblyName.Name.IsNullOrWhiteSpace()
            && (assemblyName.Name?? string.Empty).StartsWith(nameof(System)) 
            && assemblyName.Version.Major != 6
            )
        {
            var netPath = Path.Combine(Instance.Creator.Get($"dotnet-{assemblyName.Version.Major}.0.0"), assemblyName.Name + ".dll");
            if (File.Exists(netPath))
            {
                LogSource.LogDebug($"is Net {assemblyName} {assemblyName.Version}");
                return Assembly.LoadFile(netPath);
            }
            else
            {

                var DLLStream = Instance.GetFormNetZip(assemblyName.Version.Major, assemblyName.Name!);
                if (DLLStream != Stream.Null)
                {
                    var fileStream = File.Create(netPath);
                    DLLStream.CopyTo(fileStream);
                    fileStream.Close();
                    DLLStream.Close();
                    return Assembly.LoadFile(netPath);
                }
            }
        }

        var path = Path.Combine(Instance.Creator.Get("Dependents"), assemblyName.Name + ".dll");
        if (File.Exists(path))
            return Assembly.LoadFile(path);

        if (UserAutoDownloadNoFind)
        {
            using var NewStream = Instance.Download(assemblyName.Name ?? string.Empty, assemblyName.Version?.ToString() ?? string.Empty,
                typeof(NextPatcher).Assembly.GetFramework().GetNugetName());
        }
        
        return null;
    }
}