using System;
using System.Collections.Generic;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Multitasker : RoleBase
{
    public List<PlayerControl> multitasker = new();

    public override void ClearAndReload()
    {
        multitasker = new List<PlayerControl>();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}