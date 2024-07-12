using System.Linq;

namespace TheOtherUs.Devs;

public sealed class CloudManager : ListManager<CloudManager, CloudBase>
{
    public CloudBase StartCloud(CloudInfo info, bool isHttp = true) => Add(
        isHttp ?
        new HttpCloud(info.ip, info.port) { cloudInfo = info }
        : 
        new SocketCloud(info.ip, info.port) { cloudInfo = info }
        );

    public CloudBase Get(string ip, int port, bool isHttp = true)
    {
        var cloud = List.FirstOrDefault(n => n.cloudInfo.ip == ip & n.cloudInfo.port == port);
        var info = isHttp ? "http" : "sokcet";
        if (cloud == null)
            StartCloud(new CloudInfo($":{ip}|{port}|{info};", ip, port));
        return cloud;
    }

    public T Get<T>() where T : CloudBase => (T)List.FirstOrDefault(n => n is T);
    
    private CloudBase Add(CloudBase @base)
    {
        List.Add(@base);
        return @base;
    }
    
    public record CloudInfo(string name, string ip, int port);
}