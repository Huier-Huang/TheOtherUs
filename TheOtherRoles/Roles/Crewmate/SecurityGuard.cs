using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class SecurityGuard : RoleBase
{
    public PlayerControl securityGuard;
    public Color color = new Color32(195, 178, 95, byte.MaxValue);

    public float cooldown = 30f;
    public int remainingScrews = 7;
    public int totalScrews = 7;
    public int ventPrice = 1;
    public int camPrice = 2;
    public int placedCameras;
    public float duration = 10f;
    public int maxCharges = 5;
    public int rechargeTasksNumber = 3;
    public int rechargedTasks = 3;
    public int charges = 1;
    public bool cantMove = true;
    public Vent ventTarget;
    public Minigame minigame;

    private ResourceSprite closeVentButtonSprite = new ("CloseVentButton.png");
    private ResourceSprite placeCameraButtonSprite = new ("PlaceCameraButton.png");
    private ResourceSprite animatedVentSealedSprite;
    private ResourceSprite staticVentSealedSprite = new ("StaticVentSealed.png",160f);
    private ResourceSprite fungleVentSealedSprite = new ("FungleVentSealed.png", 160f);
    private ResourceSprite submergedCentralUpperVentSealedSprite = new ("CentralUpperBlocked.png", 145f);
    private ResourceSprite submergedCentralLowerVentSealedSprite = new ("CentralLowerBlocked.png", 145f);
    private ResourceSprite camSprite = new (onGetSprite: sprite =>
    {
        if (sprite.ReturnSprite != null)
            return;
        sprite.ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.CamsButton]
            .Image;
    } );
    private ResourceSprite logSprite = new(onGetSprite: sprite =>
    {
        if (sprite.ReturnSprite != null)
            return;
        sprite.ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.DoorLogsButton]
            .Image;
    } );
    
    private float lastPPU;

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

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}