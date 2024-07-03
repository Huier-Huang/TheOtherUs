using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class TimeMaster : RoleBase
{
    private ResourceSprite buttonSprite = new("TimeShieldButton.png");
    public float cooldown = 30f;
    public bool isRewinding;

    public bool reviveDuringRewind = false;
    public float rewindTime = 3f;

    public bool shieldActive;
    public float shieldDuration = 3f;
    public PlayerControl timeMaster;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(TimeMaster),
        RoleClassType = typeof(TimeMaster),
        GetRole = Get<TimeMaster>,
        Color = new Color32(112, 142, 239, byte.MaxValue),
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Crewmate,
        DescriptionText = "Use your time shield",
        IntroInfo = "Save yourself with your time shield",
        RoleId = RoleId.Tiebreaker,
        CreateRoleController = n => new TimeMasterController(n)
    };
    
    public class TimeMasterController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<TimeMaster>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        timeMaster = null;
        isRewinding = false;
        shieldActive = false;
        rewindTime = CustomOptionHolder.timeMasterRewindTime;
        shieldDuration = CustomOptionHolder.timeMasterShieldDuration;
        cooldown = CustomOptionHolder.timeMasterCooldown;
    }
}