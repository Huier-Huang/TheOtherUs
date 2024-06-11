using System.Collections.Generic;
using Hazel;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Eraser : RoleBase
{
    public List<byte> alreadyErased = [];


    private readonly ResourceSprite buttonSprite = new("EraserButton.png");
    public bool canEraseAnyone;
    public Color color = Palette.ImpostorRed;
    public float cooldown = 30f;
    public PlayerControl currentTarget;
    public PlayerControl eraser;
    private CustomButton eraserButton;
    public CustomOption eraserCanEraseAnyone;
    public CustomOption eraserCooldown;

    public CustomOption eraserSpawnRate;

    public List<PlayerControl> futureErased = [];
    public override RoleInfo RoleInfo { get; protected set; }

    public override void ClearAndReload()
    {
        eraser = null;
        futureErased = [];
        currentTarget = null;
        cooldown = eraserCooldown.getFloat();
        canEraseAnyone = eraserCanEraseAnyone.getBool();
        alreadyErased = [];
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
}