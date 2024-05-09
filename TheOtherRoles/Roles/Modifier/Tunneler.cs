using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Tunneler : RoleBase
{
    public PlayerControl tunneler;
    public Color color = new Color32(48, 21, 89, byte.MaxValue);


    public override void ClearAndReload()
    {
        tunneler = null;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}