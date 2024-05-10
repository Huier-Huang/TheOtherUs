using System;
using System.Collections.Generic;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Torch : RoleBase
{
    public List<PlayerControl> torch = [];
    public float vision = 1;

    public override void ClearAndReload()
    {
        torch = [];
        vision = CustomOptionHolder.modifierTorchVision;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}