using System;
using System.Collections.Generic;
using TheOtherUs.Objects;
using TheOtherUs.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Radar : RoleBase
{
    public PlayerControl ClosestPlayer;
    public Color color = new Color32(255, 0, 128, byte.MaxValue);
    public List<Arrow> localArrows = [];
    public PlayerControl radar;
    public bool showArrows = true;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


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
}