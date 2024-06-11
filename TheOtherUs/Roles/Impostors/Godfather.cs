using System;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Godfather : RoleBase
{
    public Color color = Palette.ImpostorRed;
    public PlayerControl godfather;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        godfather = null;
    }
}