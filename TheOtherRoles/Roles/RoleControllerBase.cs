using System;
using Hazel;
using Il2CppSystem.Collections.Generic;

namespace TheOtherRoles.Roles;

public abstract class RoleControllerBase : IDisposable
{
    protected RoleControllerBase(PlayerControl player)
    {
        Player = player;
        _RoleManager._AllControllerBases.Add(this);
    }

    public abstract RoleBase _RoleBase { get; set; }

    public PlayerControl Player { get; protected set; }

    public virtual void Dispose()
    {
        _RoleBase = null;
        Player = null;
        _RoleManager._AllControllerBases.Remove(this);
    }

    public virtual void OnRpc(MessageReader reader)
    {
    }

    [RPCListener(CustomRPC.RoleRPC)]
    public static void OnRoleRpc(MessageReader reader)
    {
        var player = reader.ReadPlayer();
        var RoleName = reader.ReadString();
        var role = player.GetRole(RoleName);
        if (!player.TryGetController(role, out var controller)) return;
        controller.OnRpc(reader);
    }

    public FastRpcWriter startRpc()
    {
        var rpcWrite = FastRpcWriter.StartNewRpcWriter(CustomRPC.RoleRPC, GameData.Instance);
        rpcWrite.Write(CachedPlayer.LocalPlayer);
        rpcWrite.Write(_RoleBase.ClassName);
        return rpcWrite;
    }

    public virtual bool SetShowRoleTeam(IntroCutscene __instance, ref List<PlayerControl> yourTeamPlayers)
    {
        return false;
    }

    public virtual bool SetShowRoleInfo(IntroCutscene __instance)
    {
        return false;
    }

    public virtual void Update(HudManager __instance)
    {
    }
}