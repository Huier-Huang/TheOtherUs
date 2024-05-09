using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Blind : RoleBase
{
    public static PlayerControl blind;
    public static Color color = new Color32(48, 21, 89, byte.MaxValue);


    public override void ClearAndReload()
    {
        blind = null;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}