using System;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Sidekick : RoleBase
{
    public bool canKill = true;
    public bool canUseVents = true;
    public Color color = new Color32(0, 180, 235, byte.MaxValue);

    public float cooldown = 30f;

    public PlayerControl currentTarget;
    public bool hasImpostorVision;
    public bool promotesToJackal = true;
    public PlayerControl sidekick;
    public bool wasImpostor;
    public bool wasSpy;

    public bool wasTeamRed;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        sidekick = null;
        currentTarget = null;
        cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
        canUseVents = CustomOptionHolder.sidekickCanUseVents.getBool();
        canKill = CustomOptionHolder.sidekickCanKill.getBool();
        promotesToJackal = CustomOptionHolder.sidekickPromotesToJackal.getBool();
        hasImpostorVision = CustomOptionHolder.jackalAndSidekickHaveImpostorVision.getBool();
        wasTeamRed = wasImpostor = wasSpy = false;
    }
}