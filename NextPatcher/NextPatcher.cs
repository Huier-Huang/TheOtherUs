using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Preloader.Core.Patching;
using Cpp2IL.Core.Extensions;


namespace NextPatcher;

[PatcherPluginInfo("NextPatcher","NextPatcher","1.0.0")]
public class NextPatcher : BasePatcher
{
    public static ManualLogSource LogSource = null!;
    public const string FileName = "Dependents.toml";
    public static NextPatcher Instance = null!;
    
    public DirectoryCreator Creator { get; set; }

    public NextPatcher()
    {
        LogSource = Log;
        Instance = this;
        Creator = new DirectoryCreator(Paths.GameRootPath, "Dependents", "dotnet-8.0.0");
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
        return fileRoot == string.Empty ? string.Empty : $"https://dotnetbuilds.azureedge.net/public/{type}/{version}/{fileRoot}-{version}-{platform}-{arch}{ex}";
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

public class DirectoryCreator(string Root, params string[] Dirs)
{
    public readonly HashSet<string> Directors = [];
    private readonly List<string> Creates = Dirs.ToList();

    public DirectoryCreator Add(string dir)
    {
        Creates.Add(dir);
        return this;
    }
    
    public void Create()
    {
        foreach (var dir in Creates)
        {
            try
            {
                var current = Root;
                foreach (var d in dir.Replace('\\', '/').Split('/'))
                {
                    current = Path.Combine(current, d);
                    if (Directory.Exists(current)) continue;
                    Directory.CreateDirectory(current);
                    Directors.Add(current);
                }
            }
            catch (Exception e)
            {
                NextPatcher.LogSource.LogError(e);
            }

            Directors.Add(Path.Combine(Root, dir));
        }
    }

    public string Get(string Name)
    {
        return Directors.FirstOrDefault(n => n.EndsWith(Name) || n.EndsWith($"{Name}/")) ?? string.Empty;
    }
}

public class NuGetDownloader(string Id, string version) : IDisposable
{
    public HttpClient? Client = new();
    public const string ApiRootUrl = "https://api.nuget.org/v3-flatcontainer";
    public string LowerId => _id.ToLowerInvariant();
    public string LowerVersion => _Version.ToLowerInvariant();
    public string _id = Id;
    public string _Version = version;

    public async Task<Stream> Download()
    {
        Client ??= new HttpClient();
        var url = $"{ApiRootUrl}/{LowerId}/{LowerVersion}/{LowerId}.{LowerVersion}.nupkg";
        NextPatcher.LogSource.LogInfo(url);
        return await Client.GetStreamAsync(url);
    }
    
    public void Dispose()
    {
        Client?.Dispose();
    }
}