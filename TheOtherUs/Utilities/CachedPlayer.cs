using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using TheOtherUs.CustomCosmetics;
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
    
    public bool CanMove => Control.CanMove; 
    public bool IsDead => Control.Data.IsDead;
    
    #nullable enable
    public CustomHat? _customHat { get; set; }
    public CustomVisor? _CustomVisor { get; set; }
    public CustomNamePlate? _customNamePlate { get; set; }
    #nullable disable

    // GameStates Form TOH
    public static class GameStates
    { 
        public static bool InGame = false;
        
        public static bool AlreadyDied = false;

        /**********Check Game Status***********/
        public static bool HasGameStart => GameManager.Instance.GameHasStarted;
        public static bool IsHost => AmongUsClient.Instance.HostId == AmongUsClient.Instance.ClientId;
        
        public static bool IsNormalGame =>
        GameOptionsManager.Instance.CurrentGameOptions.GameMode is GameModes.Normal or GameModes.NormalFools;
        
        public static bool IsHideNSeek =>
        GameOptionsManager.Instance.CurrentGameOptions.GameMode is GameModes.HideNSeek or GameModes.SeekFools;
        public static bool IsLobby => AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Joined; 
        public static bool IsInGame => InGame; 
        public static bool IsEnded => AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended; 
        public static bool IsNotJoined => AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.NotJoined; 
        public static bool IsOnlineGame => AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame; 
        public static bool IsLocalGame => AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame; 
        public static bool IsFreePlay => AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay; 
        public static bool IsInTask => InGame && !MeetingHud.Instance; 
        public static bool IsMeeting => InGame && MeetingHud.Instance;
        
        public static bool IsVoting => IsMeeting &&
                                       MeetingHud.Instance.state is MeetingHud.VoteStates.Voted
                                           or MeetingHud.VoteStates.NotVoted;
        
        public static bool IsProceeding => IsMeeting && MeetingHud.Instance.state is MeetingHud.VoteStates.Proceeding;

        public static bool IsExilling => ExileController.Instance != null;
        
        public static bool IsCountDown => GameStartManager.InstanceExists &&
                                          GameStartManager.Instance.startState == GameStartManager.StartingStates.Countdown;

        /**********TOP ZOOM.cs***********/ 
        public static bool IsShip => ShipStatus.Instance != null; 
    }

    private string GetShowName()
    {
        return Data.PlayerName;
    }

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