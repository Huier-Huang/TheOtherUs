using Hazel;
using InnerNet;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;

namespace TheOtherUs.Objects;

[Harmony]
public class NetObjectManager : ManagerBase<NetObjectManager>
{
    public Il2Generic.List<InnerNetObject> _allNetObjects => AmongUsClient.Instance.allObjects;
    public Il2Generic.Dictionary<uint, InnerNetObject> _allFastNetObjects => AmongUsClient.Instance.allObjectsFast;
    public uint LastCount
    {
        get => AmongUsClient.Instance.NetIdCnt;
        set => AmongUsClient.Instance.NetIdCnt = value;
    }

    /*[HarmonyPatch(typeof(InnerNetClient), nameof(AmongUsClient.Awake)), HarmonyPostfix]
    public static void AmongUsAwake(AmongUsClient __instance)
    {
        __instance.NonAddressableSpawnableObjects[__instance.NonAddressableSpawnableObjects.Length] = RPCNetObject.Instance;
        if (!__instance.allObjects.Contains(RPCNetObject.Instance))
        {
            __instance.AddNetObject(RPCNetObject.Instance);
            __instance.Spawn(RPCNetObject.Instance, -3);
        }
    }*/
}

[RegisterInIl2Cpp]
public class RPCNetObject : InnerNetObject
{
#nullable enable
    private static RPCNetObject? _instance;
    public static RPCNetObject Instance => _instance ??= Main.Instance.AddComponent<RPCNetObject>().DontDestroyOnLoad();

    public RPCNetObject()
    {
        SpawnId = 255;
    }
    
    public override void HandleRpc(byte callId, MessageReader reader)
    {
    }
}