using System;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Mini : RoleBase
{
    public const float defaultColliderRadius = 0.2233912f;
    public const float defaultColliderOffset = 0.3636057f;
    public float ageOnMeetingStart = 0f;

    public float growingUpDuration = 400f;
    public bool isGrowingUpInMeeting = true;
    public PlayerControl mini;
    public DateTime timeOfGrowthStart = DateTime.UtcNow;
    public DateTime timeOfMeetingStart = DateTime.UtcNow;
    public bool triggerMiniLose;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Mini),
        RoleClassType = typeof(Mini),
        Color= Color.yellow,
        RoleId = RoleId.Mini,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Mini>,
        IntroInfo = "No one will harm you",
        DescriptionText = "No one will harm you",
        CreateRoleController = player => new MiniController(player)
    };
    public class MiniController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Mini>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        mini = null;
        triggerMiniLose = false;
        growingUpDuration = CustomOptionHolder.modifierMiniGrowingUpDuration;
        isGrowingUpInMeeting = CustomOptionHolder.modifierMiniGrowingUpInMeeting;
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