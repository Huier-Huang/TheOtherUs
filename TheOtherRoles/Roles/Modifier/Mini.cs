using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Mini : RoleBase
{
    public const float defaultColliderRadius = 0.2233912f;
    public const float defaultColliderOffset = 0.3636057f;
    public float ageOnMeetingStart = 0f;
    public Color color = Color.yellow;

    public float growingUpDuration = 400f;
    public bool isGrowingUpInMeeting = true;
    public PlayerControl mini;
    public DateTime timeOfGrowthStart = DateTime.UtcNow;
    public DateTime timeOfMeetingStart = DateTime.UtcNow;
    public bool triggerMiniLose;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        mini = null;
        triggerMiniLose = false;
        growingUpDuration = CustomOptionHolder.modifierMiniGrowingUpDuration.getFloat();
        isGrowingUpInMeeting = CustomOptionHolder.modifierMiniGrowingUpInMeeting.getBool();
        timeOfGrowthStart = DateTime.UtcNow;
    }

    public float growingProgress()
    {
        var timeSinceStart = (float)(DateTime.UtcNow - timeOfGrowthStart).TotalMilliseconds;
        return Mathf.Clamp(timeSinceStart / (growingUpDuration * 1000), 0f, 1f);
    }

    public bool isGrownUp()
    {
        return growingProgress() == 1f;
    }
}