using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Jackal : RoleBase, Invisable
{
    public ResourceSprite buttonSprite = new("SidekickButton.png");
    public ResourceSprite buttonSprite2 = new("Swoop.png");
    public bool canCreateSidekick = true;
    public bool canCreateSidekickFromImpostor = true;
    public bool canSabotage;
    public bool canSwoop;
    public bool canSwoop2;
    public bool canUseVents = true;
    public float chanceSwoop;

    public float cooldown = 30f;
    public float createSidekickCooldown = 30f;
    public PlayerControl currentTarget;
    public float duration = 5f;
    
    public PlayerControl fakeSidekick;
    public List<PlayerControl> formerJackals = [];
    public bool hasImpostorVision;
    public PlayerControl jackal;
    public bool jackalPromotedFromSidekickCanCreateSidekick = true;
    public bool killFakeImpostor;
    public float swoopCooldown = 30f;
    public float swoopTimer = 0f;
    public bool wasImpostor;
    public bool wasSpy;
    public bool wasTeamRed;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Jackal),
        RoleClassType = typeof(Jackal),
        Color= new Color32(0, 180, 235, byte.MaxValue),
        RoleId = RoleId.Jackal,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Neutral,
        GetRole = Get<Jackal>,
        IntroInfo = "Kill all Crewmates and <color=#FF1919FF>Impostors</color> to win",
        DescriptionText = "Kill everyone",
        CreateRoleController = player => new JackalController(player)
    };
    public class JackalController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Jackal>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public bool isInvisable { get; set; }

    public Vector3 getSwooperSwoopVector()
    {
        return DefButtonPositions.upperRowLeft; //brb
    }

    public void removeCurrentJackal()
    {
        if (formerJackals.All(x => x.PlayerId != jackal.PlayerId)) formerJackals.Add(jackal);
        jackal = null;
        currentTarget = null;
        fakeSidekick = null;
        cooldown = CustomOptionHolder.jackalKillCooldown;
        createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown;
    }

    public override void ClearAndReload()
    {
        jackal = null;
        currentTarget = null;
        fakeSidekick = null;
        isInvisable = false;
        cooldown = CustomOptionHolder.jackalKillCooldown;
        createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown;
        canUseVents = CustomOptionHolder.jackalCanUseVents;
        canSabotage = CustomOptionHolder.jackalCanUseSabo;
        canCreateSidekick = CustomOptionHolder.jackalCanCreateSidekick;
        jackalPromotedFromSidekickCanCreateSidekick =
            CustomOptionHolder.jackalPromotedFromSidekickCanCreateSidekick;
        canCreateSidekickFromImpostor = CustomOptionHolder.jackalCanCreateSidekickFromImpostor;
        killFakeImpostor = CustomOptionHolder.jackalKillFakeImpostor;
        swoopCooldown = CustomOptionHolder.swooperCooldown;
        duration = CustomOptionHolder.swooperDuration;
        formerJackals.Clear();
        hasImpostorVision = CustomOptionHolder.jackalAndSidekickHaveImpostorVision;
        wasTeamRed = wasImpostor = wasSpy = false;
        chanceSwoop = CustomOptionHolder.jackalChanceSwoop.Selection / 10f;
        canSwoop = ListHelper.rnd.NextDouble() < chanceSwoop;
        canSwoop2 = false;
    }
}