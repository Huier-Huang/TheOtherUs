using System;
using System.Collections.Generic;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Vip : RoleBase
{
    public List<PlayerControl> vip = [];
    public bool showColor = true;

    public override void ClearAndReload()
    {
        vip = [];
        showColor = CustomOptionHolder.modifierVipShowColor.getBool();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}