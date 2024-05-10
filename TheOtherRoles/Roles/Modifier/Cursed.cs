using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Cursed : RoleBase
{
    public Color crewColor = new Color32(0, 247, 255, byte.MaxValue);
    public PlayerControl cursed;
    public Color impColor = Palette.ImpostorRed;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        cursed = null;
    }
}