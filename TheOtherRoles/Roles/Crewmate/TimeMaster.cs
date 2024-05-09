using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class TimeMaster : RoleBase
{
    public PlayerControl timeMaster;
    public Color color = new Color32(112, 142, 239, byte.MaxValue);

    public bool reviveDuringRewind = false;
    public float rewindTime = 3f;
    public float shieldDuration = 3f;
    public float cooldown = 30f;

    public bool shieldActive;
    public bool isRewinding;

    private ResourceSprite buttonSprite = new ("TimeShieldButton.png");

    public override void ClearAndReload()
    {
        timeMaster = null;
        isRewinding = false;
        shieldActive = false;
        rewindTime = CustomOptionHolder.timeMasterRewindTime.getFloat();
        shieldDuration = CustomOptionHolder.timeMasterShieldDuration.getFloat();
        cooldown = CustomOptionHolder.timeMasterCooldown.getFloat();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}