using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Slueth : RoleBase
{
    public PlayerControl slueth;
    public Color color = new Color32(48, 21, 89, byte.MaxValue);
    public List<PlayerControl> reported = [];

    public override void ClearAndReload()
    {
        slueth = null;
        reported = [];
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}