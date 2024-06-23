using System;
using System.Collections.Generic;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Seer : RoleBase
{
    public static readonly RoleInfo roleInfo = new()
    {
        RoleType = CustomRoleType.Main,
        Color = new Color32(97, 178, 108, byte.MaxValue),
        RoleClassType = typeof(Seer),
        CreateRoleController = n => new SeerController(n),
        Name = nameof(Seer),
        RoleId = RoleId.Seer,
        RoleTeam = RoleTeam.Crewmate,
        IntroInfo = "You will see players die",
        DescriptionText = "You will see players die",
        GetRole = Get<Seer>
    };
    
    public class SeerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Seer>();
    }

    public List<Vector3> deadBodyPositions = [];
    public bool limitSoulDuration;
    public int mode;
    public PlayerControl seer;
    public CustomOption seerLimitSoulDuration;
    public CustomOption seerMode;
    public CustomOption seerSoulDuration;

    public float soulDuration = 15f;

    private ResourceSprite soulSprite = new("Soul.png", 500f);

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        seer = null;
        deadBodyPositions = [];
        limitSoulDuration = seerLimitSoulDuration;
        soulDuration = seerSoulDuration;
        mode = seerMode;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        seerMode = roleOption.AddChild("Seer Mode", new StringOptionSelection(["Show Death Flash + Souls", "Show Death Flash", "Show Souls"])
        );
        seerLimitSoulDuration = roleOption.AddChild("Seer Limit Soul Duration", new BoolOptionSelection(false));
        seerSoulDuration = seerLimitSoulDuration.AddChild("Seer Soul Duration", new FloatOptionSelection(15f, 0f, 120f, 5f));
    }
}