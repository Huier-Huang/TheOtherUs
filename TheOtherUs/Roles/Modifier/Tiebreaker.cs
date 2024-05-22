using System;
using TheOtherUs.Utilities;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Tiebreaker : RoleBase
{
    public bool isTiebreak;
    public PlayerControl tiebreaker;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        tiebreaker = null;
        isTiebreak = false;
    }
}