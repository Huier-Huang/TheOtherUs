using System.Collections.Generic;

namespace TheOtherUs.Chat;

public partial class VoiceManager
{
    private List<RPCConnectProject> _rpcS = [];
    
    public bool TrySendStream(VoiceClient client)
    {
        return true;
    }
}