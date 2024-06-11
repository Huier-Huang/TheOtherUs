using System;
using System.Linq;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class BountyHunter : RoleBase
{
    public Arrow arrow;
    public float arrowUpdateIntervall = 10f;

    public float arrowUpdateTimer;
    public PlayerControl bounty;
    public float bountyDuration = 30f;
    public PlayerControl bountyHunter;
    public CustomOption bountyHunterArrowUpdateIntervall;
    public CustomOption bountyHunterBountyDuration;
    public CustomOption bountyHunterPunishmentTime;
    public CustomOption bountyHunterReducedCooldown;
    public CustomOption bountyHunterShowArrow;

    public CustomOption bountyHunterSpawnRate;
    public float bountyKillCooldown;
    public float bountyUpdateTimer;
    public Color color = Palette.ImpostorRed;
    public TextMeshPro cooldownText;
    public float punishmentTime = 15f;
    public bool showArrow = true;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void OptionCreate()
    {
        bountyHunterSpawnRate =
            new CustomOption(320, "Bounty Hunter".ColorString(color), CustomOptionHolder.rates, null, true);
        bountyHunterBountyDuration = new CustomOption(321, "Duration After Which Bounty Changes", 60f, 10f, 180f, 10f,
            bountyHunterSpawnRate);
        bountyHunterReducedCooldown = new CustomOption(322, "Cooldown After Killing Bounty", 2.5f, 0f, 30f, 2.5f,
            bountyHunterSpawnRate);
        bountyHunterPunishmentTime = new CustomOption(323, "Additional Cooldown After Killing Others", 20f, 0f, 60f,
            2.5f, bountyHunterSpawnRate);
        bountyHunterShowArrow =
            new CustomOption(324, "Show Arrow Pointing Towards The Bounty", true, bountyHunterSpawnRate);
        bountyHunterArrowUpdateIntervall =
            new CustomOption(325, "Arrow Update Intervall", 15f, 2.5f, 60f, 2.5f, bountyHunterShowArrow);
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
        foreach (var p in MapOptions.playerIcons.Values.Where(p => p != null && p.gameObject != null))
            p.gameObject.SetActive(false);


        bountyDuration = bountyHunterBountyDuration.getFloat();
        bountyKillCooldown = bountyHunterReducedCooldown.getFloat();
        punishmentTime = bountyHunterPunishmentTime.getFloat();
        showArrow = bountyHunterShowArrow.getBool();
        arrowUpdateIntervall = bountyHunterArrowUpdateIntervall.getFloat();
    }
}