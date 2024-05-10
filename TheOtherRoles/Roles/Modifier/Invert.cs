using System;
using System.Collections.Generic;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Invert : RoleBase
{
    public List<PlayerControl> invert = [];
    public int meetings = 3;

    public override void ClearAndReload()
    {
        invert = [];
        meetings = (int)CustomOptionHolder.modifierInvertDuration.getFloat();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}