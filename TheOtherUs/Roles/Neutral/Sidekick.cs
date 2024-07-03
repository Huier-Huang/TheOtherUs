using System;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Sidekick : RoleBase
{
    public bool canKill = true;
    public bool canUseVents = true;

    public float cooldown = 30f;

    public PlayerControl currentTarget;
    public bool hasImpostorVision;
    public bool promotesToJackal = true;
    public PlayerControl sidekick;
    public bool wasImpostor;
    public bool wasSpy;

    public bool wasTeamRed;

    public override CustomRoleOption roleOption { get; set; }

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Sidekick),
        RoleClassType = typeof(Sidekick),
        RoleId = RoleId.Sidekick,
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Sidekick>,
        Color = new Color32(0, 180, 235, byte.MaxValue),
        IntroInfo = "Help your Jackal to kill everyone",
        DescriptionText = "Help your Jackal to kill everyone",
        CreateRoleController = player => new SidekickController(player)
    };
    
    public class SidekickController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Sidekick>();
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        sidekick = null;
        currentTarget = null;
        cooldown = CustomOptionHolder.jackalKillCooldown;
        canUseVents = CustomOptionHolder.sidekickCanUseVents;
        canKill = CustomOptionHolder.sidekickCanKill;
        promotesToJackal = CustomOptionHolder.sidekickPromotesToJackal;
        hasImpostorVision = CustomOptionHolder.jackalAndSidekickHaveImpostorVision;
        wasTeamRed = wasImpostor = wasSpy = false;
    }
}