using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Shifter : RoleBase
{
    public PlayerControl shifter;

    public PlayerControl futureShift;
    public PlayerControl currentTarget;

    private ResourceSprite buttonSprite = new ("ShiftButton.png");

    public override void ClearAndReload()
    {
        shifter = null;
        currentTarget = null;
        futureShift = null;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; } = typeof(Shifter);
}