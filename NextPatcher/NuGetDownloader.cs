using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NextPatcher;

public class NuGetDownloader(string Id, string version) : IDisposable
{
    public HttpClient? Client = new();
    public const string ApiRootUrl = "https://api.nuget.org/v3-flatcontainer";
    public const string InfoRootUrl = "https://api.nuget.org/v3/registration5-gz-semver2";
    public string LowerId => _id.ToLowerInvariant();
    public string LowerVersion => _Version.ToLowerInvariant();
    public string _id = Id;
    public string _Version = version;

    public async void GetLatestVersion()
    {
        Client ??= new HttpClient();
        var url = $"{InfoRootUrl}/{LowerId}/index.json";
        var json = await Client.GetStringAsync(url);
        var document = JsonDocument.Parse(json);
        var version = document.RootElement
            .GetProperty("items")
            .EnumerateArray()
            .First()
            .GetProperty("upper")
            .GetString();
        _Version = version ?? string.Empty;
        NextPatcher.LogSource.LogInfo($"Get {LowerId} LatestVersion {version}");
    }

    public async Task<Stream> Download()
    {
        Client ??= new HttpClient();
        if (_Version == string.Empty) GetLatestVersion();
        var url = $"{ApiRootUrl}/{LowerId}/{LowerVersion}/{LowerId}.{LowerVersion}.nupkg";
        NextPatcher.LogSource.LogInfo(url);
        return await Client.GetStreamAsync(url);
    }
    
    public void Dispose()
    {
        Client?.Dispose();
    }
}