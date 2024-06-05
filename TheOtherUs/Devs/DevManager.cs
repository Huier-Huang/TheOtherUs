using System.Collections.Generic;
using System.Linq;

namespace TheOtherUs.Devs;

public sealed class DevManager : ListManager<DevManager, IDev>
{
    #nullable enable
    public IDev? localDev => List.FirstOrDefault(n => n.Is(CachedPlayer.LocalPlayer));

    internal DevManager Register(IDev dev)
    {
        List.Add(dev);
        return this;
    }
}