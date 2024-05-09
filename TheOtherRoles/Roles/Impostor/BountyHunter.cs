using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Modules.Options;
using TheOtherRoles.Objects;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class BountyHunter : RoleBase
{
    public PlayerControl bountyHunter;
    public Color color = Palette.ImpostorRed;

    public Arrow arrow;
    public float bountyDuration = 30f;
    public bool showArrow = true;
    public float bountyKillCooldown;
    public float punishmentTime = 15f;
    public float arrowUpdateIntervall = 10f;

    public float arrowUpdateTimer;
    public float bountyUpdateTimer;
    public PlayerControl bounty;
    public TextMeshPro cooldownText;
    
    public CustomOption bountyHunterSpawnRate;
    public CustomOption bountyHunterBountyDuration;
    public CustomOption bountyHunterReducedCooldown;
    public CustomOption bountyHunterPunishmentTime;
    public CustomOption bountyHunterShowArrow;
    public CustomOption bountyHunterArrowUpdateIntervall;

    public override void OptionCreate()
    {

        bountyHunterSpawnRate = new CustomOption(320, "Bounty Hunter".ColorString(color), CustomOptionHolder.rates, null, true);
        bountyHunterBountyDuration = new CustomOption(321, "Duration After Which Bounty Changes", 60f, 10f, 180f, 10f, bountyHunterSpawnRate);
        bountyHunterReducedCooldown = new CustomOption(322, "Cooldown After Killing Bounty", 2.5f, 0f, 30f, 2.5f, bountyHunterSpawnRate);
        bountyHunterPunishmentTime = new CustomOption(323, "Additional Cooldown After Killing Others", 20f, 0f, 60f, 2.5f, bountyHunterSpawnRate);
        bountyHunterShowArrow = new CustomOption(324, "Show Arrow Pointing Towards The Bounty", true, bountyHunterSpawnRate);
        bountyHunterArrowUpdateIntervall = new CustomOption(325, "Arrow Update Intervall", 15f, 2.5f, 60f, 2.5f, bountyHunterShowArrow);
    }

    public override void ClearAndReload()
    {
        arrow = new Arrow(color);
        bountyHunter = null;
        bounty = null;
        arrowUpdateTimer = 0f;
        bountyUpdateTimer = 0f;
        if (arrow != null && arrow.arrow != null) Object.Destroy(arrow.arrow);
        arrow = null;
        if (cooldownText != null && cooldownText.gameObject != null) Object.Destroy(cooldownText.gameObject);
        cooldownText = null;
        foreach (var p in TORMapOptions.playerIcons.Values.Where(p => p != null && p.gameObject != null))
            p.gameObject.SetActive(false);


        bountyDuration = bountyHunterBountyDuration.getFloat();
        bountyKillCooldown = bountyHunterReducedCooldown.getFloat();
        punishmentTime = bountyHunterPunishmentTime.getFloat();
        showArrow = bountyHunterShowArrow.getBool();
        arrowUpdateIntervall = bountyHunterArrowUpdateIntervall.getFloat();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    
}