using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Janitor : RoleBase
{
    private ResourceSprite buttonSprite = new("CleanButton.png");
    public Color color = Palette.ImpostorRed;

    public float cooldown = 30f;
    public PlayerControl janitor;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        janitor = null;
        cooldown = CustomOptionHolder.janitorCooldown.getFloat();
    }
}