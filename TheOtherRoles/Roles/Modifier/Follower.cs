using System;
using System.Collections.Generic;
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
    public List<Arrow> localArrows = new();
    public bool getsAssassin;
    public bool chatTarget = true;
    public bool chatTarget2 = true;

    public override void ClearAndReload()
    {
        if (localArrows != null)
            foreach (var arrow in localArrows)
                if (arrow?.arrow != null)
                    Object.Destroy(arrow.arrow);
        localArrows = new List<Arrow>();
        follower = null;
        currentTarget = null;
        chatTarget = true;
        chatTarget2 = true;
        getsAssassin = CustomOptionHolder.modifierAssassinCultist.getBool();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}