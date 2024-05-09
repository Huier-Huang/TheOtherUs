using System;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Tiebreaker : RoleBase
{
    public PlayerControl tiebreaker;

    public bool isTiebreak;

    public override void ClearAndReload()
    {
        tiebreaker = null;
        isTiebreak = false;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}