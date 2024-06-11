using System;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class TimeMaster : RoleBase
{
    private ResourceSprite buttonSprite = new("TimeShieldButton.png");
    public Color color = new Color32(112, 142, 239, byte.MaxValue);
    public float cooldown = 30f;
    public bool isRewinding;

    public bool reviveDuringRewind = false;
    public float rewindTime = 3f;

    public bool shieldActive;
    public float shieldDuration = 3f;
    public PlayerControl timeMaster;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        timeMaster = null;
        isRewinding = false;
        shieldActive = false;
        rewindTime = CustomOptionHolder.timeMasterRewindTime.getFloat();
        shieldDuration = CustomOptionHolder.timeMasterShieldDuration.getFloat();
        cooldown = CustomOptionHolder.timeMasterCooldown.getFloat();
    }
}