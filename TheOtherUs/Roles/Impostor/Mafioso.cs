using System;
using UnityEngine;

namespace TheOtherUs.Roles.Impostor;

[RegisterRole]
public class Mafioso : RoleBase
{
    public Color color = Palette.ImpostorRed;
    public PlayerControl mafioso;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        mafioso = null;
    }
}