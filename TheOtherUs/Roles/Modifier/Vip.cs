using System;
using System.Collections.Generic;
using TheOtherUs.Utilities;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Vip : RoleBase
{
    public bool showColor = true;
    public List<PlayerControl> vip = [];

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        vip = [];
        showColor = CustomOptionHolder.modifierVipShowColor.getBool();
    }
}