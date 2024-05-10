using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Follower : RoleBase
{
    public PlayerControl follower;
    public PlayerControl currentTarget;
    public Color color = Palette.ImpostorRed;
    public List<Arrow> localArrows = [];
    public bool getsAssassin;
    public bool chatTarget = true;
    public bool chatTarget2 = true;

    public override void ClearAndReload()
    {
        if (localArrows != null)
            foreach (var arrow in localArrows.Where(arrow => arrow?.arrow != null))
                Object.Destroy(arrow.arrow);
        localArrows = [];
        follower = null;
        currentTarget = null;
        chatTarget = true;
        chatTarget2 = true;
        getsAssassin = CustomOptionHolder.modifierAssassinCultist;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}