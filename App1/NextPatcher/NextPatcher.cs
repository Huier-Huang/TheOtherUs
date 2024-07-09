using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Preloader.Core.Patching;
using Cpp2IL.Core.Extensions;


namespace NextPatcher;

[PatcherPluginInfo("NextPatcher","NextPatcher","1.0.0")]
public class NextPatcher : BasePatcher
{
    public static readonly string NET8Path = Path.Combine(Paths.GameRootPath, "dotnet-net8");
    public static readonly string DependentPath = Path.Combine(Paths.GameRootPath, "Dependent");
    public static readonly HashSet<string> Files = [];
    public static ManualLogSource LogSource = null!;
    public const string FileName = "Dependents.toml";
    public static NextPatcher Instance = null!;
    
    public DirectoryCreator Creator { get; set; }

    public NextPatcher()
    {
        Instance = this;
        
        Creator = new DirectoryCreator(Paths.GameRootPath, "Dependents");
        Creator.Create();
        
        if (AppDomain.CurrentDomain.GetAssemblies().All(n => n.GetName().Name != "Tomlyn"))
            DownLoadAndLoadToml();
        
        
        
        Creator.Create();
    }
    
    public override void Initialize()
    {
        LogSource = Log;
        foreach (var file in Directory.GetFiles(DependentPath).Where(n => n.EndsWith(".dll")))
        {
            Files.Add(file);
        }
        
        
        AppDomain.CurrentDomain.AssemblyResolve += LocalResolve;
    }

    public async void DownLoadAndLoadToml(string version = "0.17.0", string Framework = "net7.0")
    {
        using var download = new NuGetDownloader("Tomlyn", Version.Parse(version)); 
        var stream = download.Get();
        if (stream == null) return;
        using var file = new ZipArchive(stream, ZipArchiveMode.Read);
        var entry = file.GetEntry($@"lib\{Framework}\Tomlyn.dll");
        if (entry == null) return;
        var path = Path.Combine(Creator.Get("Dependents"), "Tomlyn.dll");
        if (File.Exists(path))
            File.Delete(path);

        await using var NewStream = File.Create(path);
        await stream.CopyToAsync(NewStream);
        Assembly.Load(stream.ReadBytes());
    }
    
    internal static Assembly? LocalResolve(object? sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);
        if (assemblyName.Version!.Major == 8 && assemblyName.Name!.StartsWith(nameof(System)))
        {
            LogSource.LogInfo($"is Net8 {assemblyName}");
            return Assembly.LoadFile(Path.Combine(NET8Path, assemblyName.Name + ".dll"));
        }
        LogSource.LogInfo($"Resolve {assemblyName}");

        var file = Files.FirstOrDefault(n => n.EndsWith(assemblyName.Name + ".dll"));
        return file != null ? Assembly.LoadFile(file) : null;
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

public class NuGetDownloader(string Id, Version version) : IDisposable
{
    public HttpClient? Client = new();
    public const string ApiRootUrl = "https://api.nuget.org/v3-flatcontainer";
    public string LowerId => _id.ToLowerInvariant();
    public string LowerVersion = version.ToString().ToLowerInvariant();
    private Stream? DownloadStream;
    public string _id = Id;
    public Version _Version = version;
    public HttpStatusCode CurrentCode;

    public async void Download()
    {
        Client ??= new HttpClient();
        var url = $"{ApiRootUrl}/{LowerId}/{LowerVersion}/{LowerId}.{LowerVersion}.nupkg";
        var message = await Client.GetAsync(url);
        CurrentCode = message.StatusCode;
        if (CurrentCode != HttpStatusCode.OK) return;
        DownloadStream = await message.Content.ReadAsStreamAsync();
    }

    public Stream? Get()
    {
        if (DownloadStream == null)
            Download();

        return DownloadStream;
    }
    
    public void Dispose()
    {
        Client?.Dispose();
        DownloadStream?.Dispose();
    }
}