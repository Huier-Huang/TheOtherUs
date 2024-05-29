using System;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmate;

[RegisterRole]
public class Veteren : RoleBase
{
    public bool alertActive;

    public float alertDuration = 3f;

    private ResourceSprite buttonSprite = new("Alert.png");
    public Color color = new Color32(255, 77, 0, byte.MaxValue);
    public float cooldown = 30f;
    public PlayerControl veteren;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        veteren = null;
        alertActive = false;
        alertDuration = CustomOptionHolder.veterenAlertDuration.getFloat();
        cooldown = CustomOptionHolder.veterenCooldown.getFloat();
    }
}