using System;
using TheOtherRoles.Objects;
using System.Collections.Generic;
using UnityEngine;
using Hazel;
using TheOtherRoles.Modules.Options;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Eraser : RoleBase
{
    public PlayerControl eraser;
    public Color color = Palette.ImpostorRed;

    public List<byte> alreadyErased = new();

    public List<PlayerControl> futureErased = new();
    public PlayerControl currentTarget;
    public float cooldown = 30f;
    public bool canEraseAnyone;

    public CustomOption eraserSpawnRate;
    public CustomOption eraserCooldown;
    public CustomOption eraserCanEraseAnyone;
    private CustomButton eraserButton;


    private ResourceSprite buttonSprite = new ("EraserButton.png");

    public override void ClearAndReload()
    {
        eraser = null;
        futureErased = new List<PlayerControl>();
        currentTarget = null;
        cooldown = eraserCooldown.getFloat();
        canEraseAnyone = eraserCanEraseAnyone.getBool();
        alreadyErased = new List<byte>();
    }
    public override void OptionCreate()
    {
        eraserSpawnRate = new CustomOption(230, "Eraser".ColorString(color), CustomOptionHolder.rates, null, true);
        eraserCooldown = new CustomOption(231, "Eraser Cooldown", 30f, 10f, 120f, 5f, eraserSpawnRate);
        eraserCanEraseAnyone = new CustomOption(232, "Eraser Can Erase Anyone", false, eraserSpawnRate);
    }
    public override void ButtonCreate(HudManager _hudManager)
    {
        // Eraser erase button
        eraserButton = new CustomButton(
            () =>
            {
                if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);
                eraserButton.MaxTimer += 10;
                eraserButton.Timer = eraserButton.MaxTimer;

                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.SetFutureErased, SendOption.Reliable);
                writer.Write(currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setFutureErased(currentTarget.PlayerId);
                SoundEffectsManager.play("eraserErase");
            },
            () =>
            {
                return eraser != null && eraser == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, eraserButton, "ERASE");
                return CachedPlayer.LocalPlayer.Control.CanMove && currentTarget != null;
            },
            () => { eraserButton.Timer = eraserButton.MaxTimer; },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );
    }
    public override void ResetCustomButton()
    {
        eraserButton.MaxTimer = cooldown;
    }
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    
}