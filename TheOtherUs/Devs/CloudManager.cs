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

    public T Get<T>() where T : CloudBase => (T)List.FirstOrDefault(n => n is T);
    
    private CloudBase Add(CloudBase @base)
    {
        List.Add(@base);
        return @base;
    }
    
    public record CloudInfo(string name, string ip, int port);
}