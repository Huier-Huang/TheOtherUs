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
    public CustomOption jesterCanCallEmergency;
    public CustomOption jesterCanVent;

    public bool triggerJesterWin;

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        triggerJesterWin = false;
        canCallEmergency = jesterCanCallEmergency;
        canVent = jesterCanVent;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this, enableVision: true);
        jesterCanCallEmergency = new CustomOption("Jester Can Call Emergency Meeting", roleOption, new BoolOptionSelection());
        jesterCanVent = new CustomOption( "Jester Can Hide In Vent", roleOption, new BoolOptionSelection());
    }
}