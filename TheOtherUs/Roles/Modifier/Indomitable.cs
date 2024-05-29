using System;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Indomitable : RoleBase
{
    public Color color = new Color32(0, 247, 255, byte.MaxValue);
    public PlayerControl indomitable;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        indomitable = null;
    }
}