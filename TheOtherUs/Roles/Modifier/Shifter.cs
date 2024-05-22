using System;
using TheOtherUs.Modules;
using TheOtherUs.Utilities;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Shifter : RoleBase
{
    private ResourceSprite buttonSprite = new("ShiftButton.png");
    public PlayerControl currentTarget;

    public PlayerControl futureShift;
    public PlayerControl shifter;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; } = typeof(Shifter);

    public override void ClearAndReload()
    {
        shifter = null;
        currentTarget = null;
        futureShift = null;
    }
}