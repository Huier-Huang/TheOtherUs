using System;
using System.Collections.Generic;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Sunglasses : RoleBase
{
    public List<PlayerControl> sunglasses = [];
    public int vision = 1;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        sunglasses = [];
        vision = CustomOptionHolder.modifierSunglassesVision.getSelection() + 1;
    }
}