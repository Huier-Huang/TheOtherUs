using System;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class PrivateInvestigator : RoleBase
{
    private ResourceSprite buttonSprite = new("Watch.png");
    public Color color = new Color32(77, 77, 255, byte.MaxValue);
    public PlayerControl currentTarget;
    public PlayerControl privateInvestigator;


    public bool seeFlashColor;
    public PlayerControl watching;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        privateInvestigator = null;
        watching = null;
        currentTarget = null;
        seeFlashColor = CustomOptionHolder.privateInvestigatorSeeColor.getBool();
    }
}