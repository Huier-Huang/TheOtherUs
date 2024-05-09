using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class PrivateInvestigator : RoleBase
{
    public PlayerControl privateInvestigator;
    public Color color = new Color32(77, 77, 255, byte.MaxValue);
    public PlayerControl watching;
    public PlayerControl currentTarget;
    
    private ResourceSprite buttonSprite = new ("Watch.png");


    public bool seeFlashColor;


    public override void ClearAndReload()
    {
        privateInvestigator = null;
        watching = null;
        currentTarget = null;
        seeFlashColor = CustomOptionHolder.privateInvestigatorSeeColor.getBool();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}