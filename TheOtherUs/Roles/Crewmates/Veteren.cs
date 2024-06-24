using System;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Veteren : RoleBase
{
    public bool alertActive;

    public float alertDuration = 3f;

    private ResourceSprite buttonSprite = new("Alert.png");
    public Color color = new Color32(255, 77, 0, byte.MaxValue);
    public float cooldown = 30f;
    public PlayerControl veteren;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Veteren),
        Color = new Color32(255, 77, 0, byte.MaxValue),
        RoleClassType = typeof(Veteren),
        GetRole = Get<Veteren>,
        RoleId = RoleId.Veteren,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main,
        DescriptionText = "Protect yourself from others",
        IntroInfo = "Protect yourself from other",
        CreateRoleController = n=> new VeterenController(n)
    };
    
    public class VeterenController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Veteren>();
    }
    public override CustomRoleOption roleOption { get; set; }


    public override void ClearAndReload()
    {
        veteren = null;
        alertActive = false;
        alertDuration = CustomOptionHolder.veterenAlertDuration;
        cooldown = CustomOptionHolder.veterenCooldown;
    }
}