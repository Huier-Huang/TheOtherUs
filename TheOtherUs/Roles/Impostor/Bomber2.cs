using System;
using Hazel;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Impostor;

[RegisterRole]
public class Bomber2 : RoleBase
{
    public Color alertColor = Palette.ImpostorRed;

    public bool bombActive;
    public float bombDelay = 10f;

    public PlayerControl bomber2;
    //public CustomOption bomber2HotPotatoMode;

    private CustomButton bomber2BombButton;
    public CustomOption bomber2BombCooldown;
    public CustomOption bomber2Delay;
    private CustomButton bomber2KillButton;

    public CustomOption bomber2SpawnRate;
    public CustomOption bomber2Timer;
    public float bombTimer = 10f;


    private readonly ResourceSprite buttonSprite = new("Bomber2.png");
    public Color color = Palette.ImpostorRed;

    public float cooldown = 30f;

    // public static bool hotPotatoMode = false;
    public PlayerControl currentBombTarget = null;
    public PlayerControl currentTarget = null;
    public bool hasAlerted = false;
    public PlayerControl hasBomb;
    public int timeLeft = 0;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        bomber2 = null;
        bombActive = false;
        cooldown = bomber2BombCooldown.getFloat();
        bombDelay = bomber2Delay.getFloat();
        bombTimer = bomber2Timer.getFloat();
    }

    public override void OptionCreate()
    {
        bomber2SpawnRate =
            new CustomOption(8840, "Bomber [BETA]".ColorString(color), CustomOptionHolder.rates, null, true);
        bomber2BombCooldown = new CustomOption(8841, "Bomber2 Cooldown", 30f, 25f, 60f, 2.5f,
            bomber2SpawnRate);
        bomber2Delay = new CustomOption(8842, "Bomb Delay", 10f, 1f, 20f, 0.5f, bomber2SpawnRate);
        bomber2Timer = new CustomOption(8843, "Bomb Timer", 10f, 5f, 30f, 5f, bomber2SpawnRate);
        //bomber2HotPotatoMode = new CustomOption(2526236, "Hot Potato Mode", false, bomber2SpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        bomber2BombButton = new CustomButton(
            () =>
            {
                /* On Use */
                if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);
                var bombWriter = AmongUsClient.Instance.StartRpcImmediately(
                    CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                bombWriter.Write(currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(bombWriter);
                RPCProcedure.giveBomb(currentTarget.PlayerId);
                bomber2.killTimer = bombTimer + bombDelay;
                bomber2BombButton.Timer = bomber2BombButton.MaxTimer;
            },
            () =>
            {
                /* Can See */
                return bomber2 != null && bomber2 == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                /* On Click */
                return currentTarget && CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () =>
            {
                /* On Meeting End */
                bomber2BombButton.Timer = bomber2BombButton.MaxTimer;
                bomber2BombButton.isEffectActive = false;
                bomber2BombButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                hasBomb = null;
            },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.V
        );

        bomber2KillButton = new CustomButton(
            () =>
            {
                /* On Use */
                if (currentBombTarget == bomber2)
                {
                    var killWriter = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                        SendOption.Reliable);
                    killWriter.Write(bomber2.Data.PlayerId);
                    killWriter.Write(hasBomb.Data.PlayerId);
                    killWriter.Write(0);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.uncheckedMurderPlayer(bomber2.Data.PlayerId, hasBomb.Data.PlayerId, 0);

                    var bombWriter1 = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                    bombWriter1.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(bombWriter1);
                    RPCProcedure.giveBomb(byte.MaxValue);
                    return;
                }

                if (Helpers.checkAndDoVetKill(currentBombTarget)) return;
                if (Helpers.checkMuderAttemptAndKill(hasBomb, currentBombTarget) ==
                    MurderAttemptResult.SuppressKill) return;
                var bombWriter = AmongUsClient.Instance.StartRpcImmediately(
                    CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                bombWriter.Write(byte.MaxValue);
                AmongUsClient.Instance.FinishRpcImmediately(bombWriter);
                RPCProcedure.giveBomb(byte.MaxValue);
            },
            () =>
            {
                /* Can See */
                return bomber2 != null && hasBomb == CachedPlayer.LocalPlayer.Control &&
                       bombActive && !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                /* Can Click */
                return currentBombTarget && CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () =>
            {
                /* On Meeting End */
            },
            buttonSprite,
            //          0, -0.06f, 0
            new Vector3(-4.5f, 1.5f, 0),
            _hudManager,
            KeyCode.B
        );
    }

    public override void ResetCustomButton()
    {
        bomber2KillButton.MaxTimer = 0f;
        bomber2KillButton.Timer = 0f;
        bomber2BombButton.MaxTimer = cooldown;
        bomber2BombButton.EffectDuration = bombDelay + bombTimer;
    }
}