using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Sidekick : RoleBase
{
    public PlayerControl sidekick;
    public Color color = new Color32(0, 180, 235, byte.MaxValue);

    public PlayerControl currentTarget;

    public bool wasTeamRed;
    public bool wasImpostor;
    public bool wasSpy;

    public float cooldown = 30f;
    public bool canUseVents = true;
    public bool canKill = true;
    public bool promotesToJackal = true;
    public bool hasImpostorVision;

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

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}