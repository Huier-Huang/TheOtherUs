using System;
using System.Collections.Generic;
using TheOtherUs.Utilities;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Invert : RoleBase
{
    public List<PlayerControl> invert = [];
    public int meetings = 3;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        invert = [];
        meetings = (int)CustomOptionHolder.modifierInvertDuration.getFloat();
    }
}