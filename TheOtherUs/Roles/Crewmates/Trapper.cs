using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Trapper : RoleBase
{
    public bool anonymousMap;
    public int charges = 1;

    public float cooldown = 30f;
    public int infoType; // 0 = Role, 1 = Good/Evil, 2 = Name
    public int maxCharges = 5;
    public List<PlayerControl> playersOnMap = [];
    public int rechargedTasks = 3;
    public int rechargeTasksNumber = 3;

    private ResourceSprite trapButtonSprite = new("Trapper_Place_Button.png");
    public int trapCountToReveal = 2;
    public float trapDuration = 5f;
    public PlayerControl trapper;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Trapper),
        RoleClassType = typeof(Trapper),
        GetRole = Get<Trapper>,
        Color = new Color32(110, 57, 105, byte.MaxValue),
        DescriptionText = "Place traps",
        IntroInfo = "Place traps to find the Impostors",
        RoleId = RoleId.Trapper,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main,
        CreateRoleController = n => new TrapperController(n)
    };
    
    public class TrapperController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Trapper>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        trapper = null;
        cooldown = CustomOptionHolder.trapperCooldown;
        maxCharges = Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges);
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber);
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber);
        charges = Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges) / 2;
        trapCountToReveal = Mathf.RoundToInt(CustomOptionHolder.trapperTrapNeededTriggerToReveal);
        playersOnMap = [];
        anonymousMap = CustomOptionHolder.trapperAnonymousMap;
        infoType = CustomOptionHolder.trapperInfoType;
        trapDuration = CustomOptionHolder.trapperTrapDuration;
    }
}