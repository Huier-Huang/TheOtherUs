using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;

namespace TheOtherUs.Modules;

public class DependentDownload : ManagerBase<DependentDownload>
{
    public static string DllPath => NextPatcher.NextPatcher.Instance.Creator.Get( "Dependents");
    private readonly HttpClient _client = new();
    private readonly Dictionary<string, List<string>> Map = new();
    private string CurrentUrl;


    public DependentDownload()
    {
        if (!Directory.Exists(DllPath))
            Directory.CreateDirectory(DllPath);
    }
    

    public async void CheckDependent(string fileName, string url = "")
    {
        if (AppDomain.CurrentDomain.GetAssemblies().Any(n =>
                n.GetName().Name == fileName.Replace(Path.GetExtension(fileName), string.Empty)))
            return;

        var filePath = Path.Combine(DllPath, fileName);

        if (url == string.Empty || File.Exists(filePath))
            goto End;

        var stream = await DownloadDependent(url);
        var file = File.Open(filePath, FileMode.OpenOrCreate);
        await stream.CopyToAsync(file);
        stream.Close();
        file.Close();

        End: ;
    }

    public void DownLoadDependentMap(string mapUrl, bool useFast = true)
    {
        CurrentUrl = useFast ? mapUrl.GithubUrl() : mapUrl;
        Info($"CurrentUrl:{CurrentUrl}");
        var MapUrl = Path.Combine(CurrentUrl, "DependentMap.txt");
        var mapStream = _client.GetStreamAsync(MapUrl).Result;
        mapStream.StartRead(Read, out _);
    }

    public void DownLoadDependentFormMap(string option)
    {
        var DLLs = Map[option];
        foreach (var dll in DLLs) CheckDependent(dll, Path.Combine(CurrentUrl, dll));
    }

    public void Read(string s, int i)
    {
        if (s.IsNullOrWhiteSpace())
            return;

        var data = s.Split(":");
        var option = data[0];

        var list = data[1].Contains(',') ? data[1].Split(",").ToList() : [data[1]];

        Map[option] = list;
    }

    public Task<Stream> DownloadDependent(string url)
    {
        Info($"Download Url{url}");
        var stream = _client.GetStreamAsync(url).Result;
        return Task.FromResult(stream);
    }
}