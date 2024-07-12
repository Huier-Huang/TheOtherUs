using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
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
    
    public static string CurrentFrameworkName => typeof(NextPatcher).Assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName ?? string.Empty;
    public static string FrameworkStartName => CurrentFrameworkName.Split(" ")[0];
    public static string FrameworkVersion => CurrentFrameworkName.Split(" ")[1];

    public static string NugetFrameworkVersionName =>
        FrameworkStartName.Replace(".", string.Empty).ToLower() + FrameworkVersion;

    public NextPatcher()
    {
        LogSource = Log;
        Instance = this;
        Creator = new DirectoryCreator(Paths.GameRootPath, "Dependents", "dotnet-8.0.0", "NextScripts");
        Creator.Create();
        DownLoadAndLoadToml();
        /*WriteAndSaveDotnet();*/
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
    
    public Stream Download(string name, string version, string framework = "net8.0")
    {
        using var downloader = new NuGetDownloader(name, version);
        var get = new NugetZipGet(downloader);
        var frameworks = get.GetFrameworks();
        var GetFramework = NextPatcher.GetFramework(frameworks, framework);
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
        CheckAndAdd(("Tomlyn", version, NugetFrameworkVersionName));
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
        if (assemblyName.Version?.Major == 8)
        {
            var net8Path = Path.Combine(Instance.Creator.Get("dotnet-8.0.0"), assemblyName.Name + ".dll");
            if (File.Exists(net8Path))
            {
                LogSource.LogDebug($"is Net8 {assemblyName}");
                return Assembly.LoadFile(net8Path);
            }
        }

        var path = Path.Combine(Instance.Creator.Get("Dependents"), assemblyName.Name + ".dll");
        
        return File.Exists(path) ? Assembly.LoadFile(path) : null;
    }
}