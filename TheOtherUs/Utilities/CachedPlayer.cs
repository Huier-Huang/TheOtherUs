using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheOtherUs.Utilities;

public sealed class CachedPlayer
{
    public static readonly Dictionary<IntPtr, CachedPlayer> PlayerPtrs = new();
    public static readonly List<CachedPlayer> AllPlayers = [];
    public static CachedPlayer LocalPlayer => PlayerControl.LocalPlayer;

    public Transform transform { get; init; }
    public PlayerControl Control { get; init; }
    public PlayerPhysics Physics { get; init; }
    public CustomNetworkTransform NetTransform { get; init; }
    public GameData.PlayerInfo Data { get; set; }
    public byte PlayerId { get; set; }

    private bool Equals(CachedPlayer other)
    {
        return Equals(transform, other.transform) && Equals(Control, other.Control) && Equals(Physics, other.Physics) &&
               Equals(NetTransform, other.NetTransform) && Equals(Data, other.Data) && PlayerId == other.PlayerId;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || (obj is CachedPlayer other && Equals(other));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(transform, Control, Physics, NetTransform);
    }

    public static implicit operator bool(CachedPlayer player)
    {
        return player != null && player.Control;
    }

    public static implicit operator PlayerControl(CachedPlayer player)
    {
        return player.Control;
    }

    public static implicit operator PlayerPhysics(CachedPlayer player)
    {
        return player.Physics;
    }

    public static implicit operator CachedPlayer(PlayerControl player)
    {
        return AllPlayers.FirstOrDefault(n => n.Control == player);
    }

    public static implicit operator CachedPlayer(PlayerPhysics player)
    {
        return AllPlayers.FirstOrDefault(n => n.Physics == player);
    }

    public static bool operator ==(CachedPlayer cache, PlayerControl player)
    {
        return cache != null && player != null && cache.Control == player;
    }

    public static bool operator !=(CachedPlayer cache, PlayerControl player)
    {
        return cache == null || player == null || cache.Control != player;
    }
}

[HarmonyPatch]
public static class CachedPlayerPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Awake))]
    [HarmonyPostfix]
    public static void CachePlayerPatch(PlayerControl __instance)
    {
        if (__instance.notRealPlayer) return;
        var player = new CachedPlayer
        {
            transform = __instance.transform,
            Control = __instance,
            Physics = __instance.MyPhysics,
            NetTransform = __instance.NetTransform
        };
        CachedPlayer.AllPlayers.Add(player);
        CachedPlayer.PlayerPtrs[__instance.Pointer] = player;
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnDestroy))]
    [HarmonyPostfix]
    public static void RemoveCachedPlayerPatch(PlayerControl __instance)
    {
        if (__instance.notRealPlayer) return;
        CachedPlayer.AllPlayers.RemoveAll(p => p.Control.Pointer == __instance.Pointer);
        CachedPlayer.PlayerPtrs.Remove(__instance.Pointer);
    }

    [HarmonyPatch(typeof(GameData), nameof(GameData.Deserialize))]
    [HarmonyPostfix]
    public static void AddCachedDataOnDeserialize()
    {
        foreach (var cachedPlayer in CachedPlayer.AllPlayers)
        {
            cachedPlayer.Data = cachedPlayer.Control.Data;
            cachedPlayer.PlayerId = cachedPlayer.Control.PlayerId;
        }
    }

    [HarmonyPatch(typeof(GameData), nameof(GameData.AddPlayer))]
    [HarmonyPostfix]
    public static void AddCachedDataOnAddPlayer()
    {
        foreach (var cachedPlayer in CachedPlayer.AllPlayers)
        {
            cachedPlayer.Data = cachedPlayer.Control.Data;
            cachedPlayer.PlayerId = cachedPlayer.Control.PlayerId;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Deserialize))]
    [HarmonyPostfix]
    public static void SetCachedPlayerId(PlayerControl __instance)
    {
        CachedPlayer.PlayerPtrs[__instance.Pointer].PlayerId = __instance.PlayerId;
    }
}