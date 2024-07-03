#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hazel;
using InnerNet;

namespace TheOtherUs.Helper;

[Harmony]
[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal class RPCListener : RegisterAttribute
{
    private static readonly List<RPCListener> _allListeners = [];
    private readonly CustomRPC RPCId;
    public Action<MessageReader> OnRPC = null!;

    public RPCListener(CustomRPC rpc)
    {
        RPCId = rpc;
        OnRPC += n => Info($"{RPCId} {n.Tag} {n.Length}");
    }

    [Register]
    public static void Register(List<MethodInfo> methods)
    {
        methods.Do(n =>
        {
            var listener = n.GetCustomAttribute<RPCListener>();
            if (listener == null) return;
            listener.OnRPC += reader => n.Invoke(null, [reader]);
            Info($"add listener {n.Name} {n.GetType().Name}");
            _allListeners.Add(listener);
        });
    }

    [HarmonyPatch(typeof(InnerNetClient._HandleGameDataInner_d__41),
        nameof(InnerNetClient._HandleGameDataInner_d__41.MoveNext))]
    [HarmonyPrefix]
    private static void InnerNet_ReaderPath(InnerNetClient._HandleGameDataInner_d__41 __instance, ref bool __result)
    {
        if (_allListeners.Count <= 0) return;
        var innerNetClient = __instance.__4__this;
        var reader = __instance.reader;

        if (__instance.__1__state != 0) return;

        var HandleReader = MessageReader.Get(reader);
        HandleReader.Position = 0;
        var tag = reader.Tag;
        if (tag != 2)
            goto recycle;
        var objetId = HandleReader.ReadPackedUInt32();
        var callId = HandleReader.ReadByte();
        if (_allListeners.All(n => n.RPCId != (CustomRPC)callId))
            goto recycle;
        try
        {
            _allListeners.Where(n => n.RPCId == (CustomRPC)callId).Do(n => n.OnRPC.Invoke(HandleReader));
            __result = false;
            Info("Listener");
        }
        catch (Exception e)
        {
            Exception(e);
        }

        finally
        {
            HandleReader.Recycle();
        }

        return;
        recycle:
        HandleReader.Recycle();
    }
}