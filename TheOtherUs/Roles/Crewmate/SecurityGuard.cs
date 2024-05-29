using System;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmate;

[RegisterRole]
public class SecurityGuard : RoleBase
{
    private ResourceSprite animatedVentSealedSprite;
    public int camPrice = 2;

    private ResourceSprite camSprite = new(onGetSprite: sprite =>
    {
        if (sprite.ReturnSprite != null)
            return;
        sprite.ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings[ImageNames.CamsButton]
            .Image;
    });

    public bool cantMove = true;
    public int charges = 1;

    private ResourceSprite closeVentButtonSprite = new("CloseVentButton.png");
    public Color color = new Color32(195, 178, 95, byte.MaxValue);

    public float cooldown = 30f;
    public float duration = 10f;
    private ResourceSprite fungleVentSealedSprite = new("FungleVentSealed.png", 160f);

    private float lastPPU;

    private ResourceSprite logSprite = new(onGetSprite: sprite =>
    {
        if (sprite.ReturnSprite != null)
            return;
        sprite.ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings[ImageNames.DoorLogsButton]
            .Image;
    });

    public int maxCharges = 5;
    public Minigame minigame;
    private ResourceSprite placeCameraButtonSprite = new("PlaceCameraButton.png");
    public int placedCameras;
    public int rechargedTasks = 3;
    public int rechargeTasksNumber = 3;
    public int remainingScrews = 7;
    public PlayerControl securityGuard;
    private ResourceSprite staticVentSealedSprite = new("StaticVentSealed.png", 160f);
    private ResourceSprite submergedCentralLowerVentSealedSprite = new("CentralLowerBlocked.png", 145f);
    private ResourceSprite submergedCentralUpperVentSealedSprite = new("CentralUpperBlocked.png", 145f);
    public int totalScrews = 7;
    public int ventPrice = 1;
    public Vent ventTarget;

    public SecurityGuard()
    {
        animatedVentSealedSprite = new ResourceSprite("AnimatedVentSealed.png", onGetSprite: sprite =>
        {
            var ppu = 185f;
            if (SubmergedCompatibility.IsSubmerged) ppu = 120f;
            lastPPU = ppu;

            sprite._pixel = ppu;
        });
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        securityGuard = null;
        ventTarget = null;
        minigame = null;
        duration = CustomOptionHolder.securityGuardCamDuration.getFloat();
        maxCharges = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamMaxCharges.getFloat());
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamRechargeTasksNumber.getFloat());
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamRechargeTasksNumber.getFloat());
        charges = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamMaxCharges.getFloat()) / 2;
        placedCameras = 0;
        cooldown = CustomOptionHolder.securityGuardCooldown.getFloat();
        totalScrews = remainingScrews = Mathf.RoundToInt(CustomOptionHolder.securityGuardTotalScrews.getFloat());
        camPrice = Mathf.RoundToInt(CustomOptionHolder.securityGuardCamPrice.getFloat());
        ventPrice = Mathf.RoundToInt(CustomOptionHolder.securityGuardVentPrice.getFloat());
        cantMove = CustomOptionHolder.securityGuardNoMove.getBool();
    }
}