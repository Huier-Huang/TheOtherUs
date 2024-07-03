using System.Linq;

namespace TheOtherUs.Devs;

public sealed class DevManager : ListManager<DevManager, IDev>
{
    #nullable enable
    public IDev? localDev => List.FirstOrDefault(n => n.Is(LocalPlayer));
    

    internal DevManager Register(IDev dev)
    {
        List.Add(dev);
        return this;
    }
    
    public IDev GetGenerationDev()
    {
        return new Dev();
    }

    public DevInfo GetFormServer(string ip, int port)
    {
        var client = CloudManager.Instance.Get(ip, port);
        return new DevInfo();
    }
}

public class DevInfo()
{
    
}