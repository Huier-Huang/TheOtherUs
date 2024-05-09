using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Janitor : RoleBase
{
    public PlayerControl janitor;
    public Color color = Palette.ImpostorRed;

    public float cooldown = 30f;

    private ResourceSprite buttonSprite = new ("CleanButton.png");

    public override void ClearAndReload()
    {
        janitor = null;
        cooldown = CustomOptionHolder.janitorCooldown.getFloat();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    
}