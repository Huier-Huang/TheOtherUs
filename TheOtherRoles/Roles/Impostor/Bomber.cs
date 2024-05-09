using System;
using System.Collections.Generic;
using Hazel;
using TheOtherRoles.Objects;
using TheOtherRoles.Roles.Modifier;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Bomber : RoleBase
{
    public PlayerControl bomber;
    public Color color = Palette.ImpostorRed;

    public Bomb bomb;
    public bool isPlanted;
    public bool isActive;
    public float destructionTime = 20f;
    public float destructionRange = 2f;
    public float hearRange = 30f;
    public float defuseDuration = 3f;
    public float bombCooldown = 15f;
    public float bombActiveAfter = 3f;

    private ResourceSprite buttonSprite = new ("Bomb_Button_Plant.png");

    public CustomButton defuseButton;
    public CustomButton bomberButton;

    public void clearBomb(bool flag = true)
    {
        if (bomb != null)
        {
            Object.Destroy(bomb.bomb);
            Object.Destroy(bomb.background);
            bomb = null;
        }

        isPlanted = false;
        isActive = false;
        if (flag) SoundEffectsManager.stop("bombFuseBurning");
    }

    public override void ClearAndReload()
    {
        clearBomb(false);
        bomber = null;
        bomb = null;
        isPlanted = false;
        isActive = false;
        destructionTime = CustomOptionHolder.bomberBombDestructionTime.getFloat();
        destructionRange = CustomOptionHolder.bomberBombDestructionRange.getFloat() / 10;
        hearRange = CustomOptionHolder.bomberBombHearRange.getFloat() / 10;
        defuseDuration = CustomOptionHolder.bomberDefuseDuration.getFloat();
        bombCooldown = CustomOptionHolder.bomberBombCooldown.getFloat();
        bombActiveAfter = CustomOptionHolder.bomberBombActiveAfter.getFloat();
        Bomb.clearBackgroundSprite();
    }
    public override void ButtonCreate(HudManager _hudManager)
    {
        // Bomber button
        bomberButton = new CustomButton(
            () =>
            {
                if (Helpers.checkMuderAttempt(bomber, bomber) != MurderAttemptResult.BlankKill)
                {
                    var pos = CachedPlayer.LocalPlayer.transform.position;
                    var buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    var writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.Control.NetId,
                        (byte)CustomRPC.PlaceBomb);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.placeBomb(buff);

                    SoundEffectsManager.play("trapperTrap");
                }

                bomberButton.Timer = bomberButton.MaxTimer;
                isPlanted = true;
            },
            () =>
            {
                return bomber != null && bomber == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () => { return CachedPlayer.LocalPlayer.Control.CanMove && !isPlanted; },
            () => { bomberButton.Timer = bomberButton.MaxTimer; },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F,
            true,
            destructionTime,
            () =>
            {
                bomberButton.Timer = bomberButton.MaxTimer;
                bomberButton.isEffectActive = false;
                bomberButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            }
        );
        defuseButton = new CustomButton(
            () => { defuseButton.HasEffect = true; },
            () =>
            {
                defuseButton.PositionOffset = Get<Shifter>().shifterShiftButton.HasButton() ? new Vector3(0f, 2f, 0f) : new Vector3(0f, 1f, 0f);
                return bomb != null && Bomb.canDefuse && !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (defuseButton.isEffectActive && !Bomb.canDefuse)
                {
                    defuseButton.Timer = 0f;
                    defuseButton.isEffectActive = false;
                }

                return CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () =>
            {
                defuseButton.Timer = 0f;
                defuseButton.isEffectActive = false;
            },
            Bomb.getDefuseSprite(),
            new Vector3(0f, 1f, 0),
            _hudManager,
            null,
            true,
            defuseDuration,
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.DefuseBomb, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.defuseBomb();

                defuseButton.Timer = 0f;
                Bomb.canDefuse = false;
            },
            true
        );
    }
    public override void ResetCustomButton()
    {
        bomberButton.MaxTimer = bombCooldown;
        defuseButton.EffectDuration = defuseDuration;
        bomberButton.EffectDuration = destructionTime + bombActiveAfter;

    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    
}