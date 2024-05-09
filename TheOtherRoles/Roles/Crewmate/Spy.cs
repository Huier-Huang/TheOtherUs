using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Spy : RoleBase
{
    public PlayerControl spy;
    public Color color = Palette.ImpostorRed;

    public bool impostorsCanKillAnyone = true;
    public bool canEnterVents;
    public bool hasImpostorVision;

    public override void ClearAndReload() 
    {
        spy = null;
        impostorsCanKillAnyone = CustomOptionHolder.spyImpostorsCanKillAnyone.getBool();
        canEnterVents = CustomOptionHolder.spyCanEnterVents.getBool();
        hasImpostorVision = CustomOptionHolder.spyHasImpostorVision.getBool();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

}