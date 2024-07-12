using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class SecurityGuard : RoleBase
{
    public int camPrice = 2;

    public bool cantMove = true;
    public int charges = 1;

    public float cooldown = 30f;
    public float duration = 10f;

    public ResourceSprite animatedVentSealedSprite = new("AnimatedVentSealed.png", 185f);
    public ResourceSprite camSprite = new(onGetSprite: sprite =>
    {
        if (sprite.ReturnSprite != null)
            return;
        sprite.ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings[ImageNames.CamsButton]
            .Image;
    });
    public ResourceSprite closeVentButtonSprite = new("CloseVentButton.png");
    public ResourceSprite fungleVentSealedSprite = new("FungleVentSealed.png", 160f);
    public ResourceSprite logSprite = new(onGetSprite: sprite =>
    {
        if (sprite.ReturnSprite != null)
            return;
        sprite.ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings[ImageNames.DoorLogsButton]
            .Image;
    });
    
    public ResourceSprite staticVentSealedSprite = new("StaticVentSealed.png", 160f);
    public ResourceSprite submergedCentralLowerVentSealedSprite = new("CentralLowerBlocked.png", 145f);
    public ResourceSprite submergedCentralUpperVentSealedSprite = new("CentralUpperBlocked.png", 145f);
    public ResourceSprite placeCameraButtonSprite = new("PlaceCameraButton.png");

    public int maxCharges = 5;
    public Minigame minigame;
    public int placedCameras;
    public int rechargedTasks = 3;
    public int rechargeTasksNumber = 3;
    public int remainingScrews = 7;
    public PlayerControl securityGuard;
    public int totalScrews = 7;
    public int ventPrice = 1;
    public Vent ventTarget;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(195, 178, 95, byte.MaxValue),
        GetRole = Get<SecurityGuard>,
        CreateRoleController = n => new SecurityGuardController(n),
        DescriptionText = "Seal vents and place cameras",
        IntroInfo = "Seal vents and place cameras",
        Name = nameof(SecurityGuard),
        RoleClassType = typeof(SecurityGuard),
        RoleId = RoleId.SecurityGuard,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class SecurityGuardController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<SecurityGuard>();
    }
    
    public override CustomRoleOption roleOption { get; set; }


    public override void ClearAndReload()
    {
        securityGuard = null;
        ventTarget = null;
        minigame = null;
        duration = CustomOptionHolder.securityGuardCamDuration;
        maxCharges = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamMaxCharges);
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamRechargeTasksNumber);
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamRechargeTasksNumber);
        charges = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamMaxCharges) / 2;
        placedCameras = 0;
        cooldown = CustomOptionHolder.securityGuardCooldown;
        totalScrews = remainingScrews = Mathf.RoundToInt(CustomOptionHolder.securityGuardTotalScrews);
        camPrice = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamPrice);
        ventPrice = Mathf.RoundToInt(CustomOptionHolder.securityGuardVentPrice);
        cantMove = CustomOptionHolder.securityGuardNoMove;
    }
}