using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Mafioso : RoleBase
{
    public PlayerControl mafioso;
    public Color color = Palette.ImpostorRed;

    public override void ClearAndReload()
    {
        mafioso = null;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}