using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace TheOtherRoles.Helper;

public static class RoleHelper
{
    public static readonly CustomRoleManager _RoleManager = CustomRoleManager.Instance;

    public static bool Is<T>(this PlayerControl player) where T : RoleBase
    {
        return _RoleManager.PlayerAndRoles[Get<T>()].Contains(player);
    }

    public static bool Is<T>(this byte playerId) where T : RoleBase
    {
        return Is<T>(playerId.GetPlayer());
    }

    public static bool Is<T>(this GameData.PlayerInfo playerInfo) where T : RoleBase
    {
        return Is<T>(playerInfo.PlayerId);
    }

    public static bool Is(this PlayerControl player, RoleTeam team)
    {
        return player.GetRoles().Any(n => n.RoleInfo.RoleTeam == team);
    }

    public static Color GetColor<T>() where T : RoleBase
    {
        return Roles.RoleInfo.AllRoleInfo.FirstOrDefault(n => n.RoleClassType == typeof(T))!.Color;
    }

    public static RoleBase GetRole(this PlayerControl player)
    {
        return _RoleManager.PlayerAndRoles.FirstOrDefault(n => n.Value.Contains(player)).Key;
    }

    public static RoleBase GetRole(this RoleId id)
    {
        return _RoleManager._RoleBases.FirstOrDefault(n => n.RoleInfo.RoleId == id);
    }

    public static RoleBase GetRole(this PlayerControl player, string RoleClassName)
    {
        return player.GetRoles().FirstOrDefault(n => n.ClassName == RoleClassName);
    }

    public static RoleBase GetMainRole(this PlayerControl player)
    {
        var roles = player.GetRoles().ToList();
        return roles.FirstOrDefault(n => n.RoleInfo.RoleType == CustomRoleType.Main) ??
               roles.FirstOrDefault(n => n.RoleInfo.RoleType == CustomRoleType.MainAndModifier);
    }

    public static IEnumerable<RoleBase> GetRoles(this PlayerControl player)
    {
        return _RoleManager.PlayerAndRoles.Where(n => n.Value.Contains(player)).Select(n => n.Key).ToList();
    }

    public static T Get<T>() where T : RoleBase
    {
        return _RoleManager._RoleBases.FirstOrDefault(n => n is T) as T;
    }

    public static T Get<T>(Type type) where T : RoleBase
    {
        return _RoleManager._RoleBases.FirstOrDefault(n => n.RoleType == type) as T;
    }

    public static RoleBase Get(Type type)
    {
        return _RoleManager._RoleBases.FirstOrDefault(n => n.RoleType == type);
    }

    public static void shiftRole(this PlayerControl player1, PlayerControl player2)
    {
        var role1 = player1.GetRole();
        var role2 = player2.GetRole();

        _RoleManager.ShifterRole(player1, role2);
        _RoleManager.ShifterRole(player2, role1);
    }

    public static PlayerControl GetPlayer(this byte playerId)
    {
        return CachedPlayer.AllPlayers.FirstOrDefault(n => n.PlayerId == playerId);
    }

    public static List<PlayerControl> GetTeamPlayers(RoleTeam team)
    {
        return _RoleManager.PlayerAndRoles.Where(n => n.Key.RoleInfo.RoleTeam == team).SelectMany(n => n.Value)
            .ToList();
    }
#nullable enable
    public static bool TryGetControllers(this PlayerControl player, out List<RoleControllerBase> roleControllers)
    {
        roleControllers = _RoleManager._AllControllerBases.Where(n => n.Player == player).ToList();
        return roleControllers.Any();
    }

    public static bool TryGetController(this PlayerControl player, RoleBase @base,
        [MaybeNullWhen(false)] out RoleControllerBase roleController)
    {
        var controller = roleController =
            _RoleManager._AllControllerBases.FirstOrDefault(n => n.Player == player && n._RoleBase == @base);
        return controller == null;
    }

    public static bool TryGetController<T>(this PlayerControl player,
        [MaybeNullWhen(false)] out RoleControllerBase roleController)
    {
        var controller = roleController =
            _RoleManager._AllControllerBases.FirstOrDefault(n => n.Player == player && n._RoleBase is T);
        return controller == null;
    }
}