using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Indomitable : RoleBase
{
    public PlayerControl indomitable;
    public Color color = new Color32(0, 247, 255, byte.MaxValue);


    public override void ClearAndReload()
    {
        indomitable = null;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}