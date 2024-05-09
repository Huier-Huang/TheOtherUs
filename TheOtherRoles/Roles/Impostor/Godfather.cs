using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Godfather : RoleBase
{
    public PlayerControl godfather;
    public Color color = Palette.ImpostorRed;

    public override void ClearAndReload()
    {
        godfather = null;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    
}