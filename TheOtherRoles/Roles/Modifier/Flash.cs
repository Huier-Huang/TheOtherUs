using System;

namespace TheOtherRoles.Roles.Modifier;

public sealed class Flash : RoleBase
{
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        base.ClearAndReload();
    }
}