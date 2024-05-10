using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class AntiTeleport : RoleBase
{
    public List<PlayerControl> antiTeleport = [];
    public Vector3 position;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        antiTeleport = [];
        position = Vector3.zero;
    }

    public void setPosition()
    {
        if (position == Vector3.zero)
            return; // Check if this has been set, otherwise first spawn on submerged will fail
        if (antiTeleport.FindAll(x => x.PlayerId == CachedPlayer.LocalPlayer.PlayerId).Count <= 0) return;

        CachedPlayer.LocalPlayer.NetTransform.RpcSnapTo(position);
        if (SubmergedCompatibility.IsSubmerged) SubmergedCompatibility.ChangeFloor(position.y > -7);
    }
}