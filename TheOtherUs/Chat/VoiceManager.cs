using System.Collections.Generic;
using TheOtherUs.Chat.Components;

namespace TheOtherUs.Chat;

public partial class VoiceManager : ManagerBase<VoiceManager>
{
    private readonly List<VoiceClient> clients = [];
    public IEnumerable<VoiceClient> ActiveClients => clients.FindAll(n => n.active);
    internal VoiceComponent Current;

    public void Register(VoiceClient client)
    {
        if (clients.Contains(client))
            return;
        
        clients.Add(client);
        if (!Current.Started) return;
        client.Register(Current, this);
        client.OnStart();
    }

    internal void Remove(VoiceClient client) => clients.Remove(client);
}