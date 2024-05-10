using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Objects;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

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

    public Color color = new Color32(0, 180, 235, byte.MaxValue);

    public float cooldown = 30f;
    public float createSidekickCooldown = 30f;
    public PlayerControl currentTarget;
    public float duration = 5f;

    //public static Color color = new Color32(224, 197, 219, byte.MaxValue);
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

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    public bool isInvisable { get; set; }

    public Vector3 getSwooperSwoopVector()
    {
        return CustomButton.ButtonPositions.upperRowLeft; //brb
    }

    public void removeCurrentJackal()
    {
        if (formerJackals.All(x => x.PlayerId != jackal.PlayerId)) formerJackals.Add(jackal);
        jackal = null;
        currentTarget = null;
        fakeSidekick = null;
        cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
        createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.getFloat();
    }

    public override void ClearAndReload()
    {
        jackal = null;
        currentTarget = null;
        fakeSidekick = null;
        isInvisable = false;
        cooldown = CustomOptionHolder.jackalKillCooldown.getFloat();
        createSidekickCooldown = CustomOptionHolder.jackalCreateSidekickCooldown.getFloat();
        canUseVents = CustomOptionHolder.jackalCanUseVents.getBool();
        canSabotage = CustomOptionHolder.jackalCanUseSabo.getBool();
        canCreateSidekick = CustomOptionHolder.jackalCanCreateSidekick.getBool();
        jackalPromotedFromSidekickCanCreateSidekick =
            CustomOptionHolder.jackalPromotedFromSidekickCanCreateSidekick.getBool();
        canCreateSidekickFromImpostor = CustomOptionHolder.jackalCanCreateSidekickFromImpostor.getBool();
        killFakeImpostor = CustomOptionHolder.jackalKillFakeImpostor.getBool();
        swoopCooldown = CustomOptionHolder.swooperCooldown.getFloat();
        duration = CustomOptionHolder.swooperDuration.getFloat();
        formerJackals.Clear();
        hasImpostorVision = CustomOptionHolder.jackalAndSidekickHaveImpostorVision.getBool();
        wasTeamRed = wasImpostor = wasSpy = false;
        chanceSwoop = CustomOptionHolder.jackalChanceSwoop.getSelection() / 10f;
        canSwoop = ListHelper.rnd.NextDouble() < chanceSwoop;
        canSwoop2 = false;
    }
}