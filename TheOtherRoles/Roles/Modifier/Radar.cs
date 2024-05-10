using System;
using System.Collections.Generic;
using TheOtherRoles.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Radar : RoleBase
{
    public PlayerControl radar;
    public List<Arrow> localArrows = [];
    public PlayerControl ClosestPlayer;
    public Color color = new Color32(255, 0, 128, byte.MaxValue);
    public bool showArrows = true;


    public override void ClearAndReload()
    {
        radar = null;
        showArrows = true;
        if (localArrows != null)
            foreach (var arrow in localArrows)
                if (arrow?.arrow != null)
                    Object.Destroy(arrow.arrow);
        localArrows = [];
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}