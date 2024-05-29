using System;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Jester : RoleBase
{
    public static readonly RoleInfo roleInfo = new()
    {
        Name = nameof(Jester),
        Color = new Color32(236, 98, 165, byte.MaxValue),
        Description = "Get voted out",
        GetRole = Get<Jester>,
        IntroInfo = "Get voted out",
        RoleClassType = typeof(Jester),
        RoleId = RoleId.Jester,
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main
    };

    public bool canCallEmergency = true;
    public bool canVent;
    public Color color = new Color32(236, 98, 165, byte.MaxValue);
    public bool hasImpostorVision;
    public PlayerControl jester;
    public CustomOption jesterCanCallEmergency;
    public CustomOption jesterCanVent;
    public CustomOption jesterHasImpostorVision;

    public CustomOption jesterSpawnRate;

    public bool triggerJesterWin;

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override Type RoleType { get; protected set; } = roleInfo.RoleClassType;

    public override void ClearAndReload()
    {
        jester = null;
        triggerJesterWin = false;
        canCallEmergency = jesterCanCallEmergency.getBool();
        canVent = jesterCanVent.getBool();
        hasImpostorVision = jesterHasImpostorVision.getBool();
    }

    public override void OptionCreate()
    {
        jesterSpawnRate = new CustomOption(60, "Jester".ColorString(color), CustomOptionHolder.rates, null, true);
        jesterCanCallEmergency = new CustomOption(61, "Jester Can Call Emergency Meeting", true, jesterSpawnRate);
        jesterCanVent = new CustomOption(1901, "Jester Can Hide In Vent", true, jesterSpawnRate);
        jesterHasImpostorVision = new CustomOption(62, "Jester Has Impostor Vision", false, jesterSpawnRate);
    }
}