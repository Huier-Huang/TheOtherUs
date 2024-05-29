using System;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Tunneler : RoleBase
{
    public Color color = new Color32(48, 21, 89, byte.MaxValue);
    public PlayerControl tunneler;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        tunneler = null;
    }
}