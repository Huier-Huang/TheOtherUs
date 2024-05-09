using System;
using TheOtherRoles.Modules.Options;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Jester : RoleBase
{
    public PlayerControl jester;
    public Color color = new Color32(236, 98, 165, byte.MaxValue);

    public bool triggerJesterWin;
    public bool canCallEmergency = true;
    public bool canVent;
    public bool hasImpostorVision;

    public CustomOption jesterSpawnRate;
    public CustomOption jesterCanCallEmergency;
    public CustomOption jesterCanVent;
    public CustomOption jesterHasImpostorVision;

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

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override Type RoleType { get; protected set; } = roleInfo.RoleClassType;
}