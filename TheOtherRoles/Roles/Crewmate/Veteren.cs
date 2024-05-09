using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Veteren : RoleBase
{
    public PlayerControl veteren;
    public Color color = new Color32(255, 77, 0, byte.MaxValue);

    public float alertDuration = 3f;
    public float cooldown = 30f;

    public bool alertActive;

    private ResourceSprite buttonSprite = new ("Alert.png");


    public override void ClearAndReload()
    {
        veteren = null;
        alertActive = false;
        alertDuration = CustomOptionHolder.veterenAlertDuration.getFloat();
        cooldown = CustomOptionHolder.veterenCooldown.getFloat();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}