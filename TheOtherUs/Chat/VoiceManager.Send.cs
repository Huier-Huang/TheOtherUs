using System.Collections.Generic;
using TheOtherUs.Helper.RPC;

namespace TheOtherUs.Chat;

public partial class VoiceManager
{
    private List<RPCConnectProject> _rpcS = [];
    
    public bool TrySendStream(VoiceClient client)
    {
        return true;
    }
}