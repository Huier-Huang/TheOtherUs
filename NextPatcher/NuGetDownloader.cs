using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace NextPatcher;

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