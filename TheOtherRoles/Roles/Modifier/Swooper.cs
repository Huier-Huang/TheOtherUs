using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Swooper : RoleBase
{
    public PlayerControl swooper;
    public Color color = new Color32(224, 197, 219, byte.MaxValue);


    public override void ClearAndReload()
    {
        swooper = null;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}