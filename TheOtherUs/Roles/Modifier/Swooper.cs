using System;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Swooper : RoleBase
{
    public Color color = new Color32(224, 197, 219, byte.MaxValue);
    public PlayerControl swooper;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        swooper = null;
    }
}