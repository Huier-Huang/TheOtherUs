using System;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Vampire : RoleBase
{
    public PlayerControl bitten;

    private readonly ResourceSprite buttonSprite = new("VampireButton.png");
    public bool canKillNearGarlics = true;
    public float cooldown = 30f;

    public PlayerControl currentTarget;

    public float delay = 10f;
    public CustomButton garlicButton;
    public bool GarlicButton;
    private readonly ResourceSprite garlicButtonSprite = new("GarlicButton.png");
    public bool garlicsActive = true;
    public bool localPlacedGarlic;
    public bool targetNearGarlic;
    public PlayerControl vampire;
    public CustomOption vampireCanKillNearGarlics;
    public CustomOption vampireCooldown;
    public CustomOption vampireGarlicButton;

    public CustomButton vampireKillButton;
    public CustomOption vampireKillDelay;


    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Vampire),
        RoleClassType = typeof(Vampire),
        RoleId = RoleId.Vampire,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Vampire>,
        Color = Palette.ImpostorRed,
        IntroInfo = "Kill the Crewmates with your bites",
        DescriptionText = "Bite your enemies",
        CreateRoleController = player => new VampireController(player)
    };
    
    public class VampireController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Vampire>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        vampire = null;
        bitten = null;
        targetNearGarlic = false;
        localPlacedGarlic = false;
        currentTarget = null;
        delay = vampireKillDelay;
        cooldown = vampireCooldown;
        canKillNearGarlics = vampireCanKillNearGarlics;
        GarlicButton = vampireGarlicButton;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        vampireKillDelay =
            roleOption.AddChild("Vampire Kill Delay", new FloatOptionSelection(10f, 1f, 20f, 1f));
        vampireCooldown =
            roleOption.AddChild("Vampire Cooldown", new FloatOptionSelection(30f, 10f, 60f, 2.5f));
        vampireGarlicButton = roleOption.AddChild("Enable Garlic", new BoolOptionSelection());
        vampireCanKillNearGarlics = vampireGarlicButton.AddChild("Vampire Can Kill Near Garlics", new BoolOptionSelection());
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        vampireKillButton = new CustomButton(
            () =>
            {
                /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                /*var murder = Helpers.checkMuderAttempt(vampire, currentTarget);#1#
                if (murder == MurderAttemptResult.PerformKill)
                {
                    if (targetNearGarlic)
                    {
                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                            LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                            SendOption.Reliable);
                        writer.Write(vampire.PlayerId);
                        writer.Write(currentTarget.PlayerId);
                        writer.Write(byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        /*RPCProcedure.uncheckedMurderPlayer(vampire.PlayerId, currentTarget.PlayerId,
                            byte.MaxValue);#1#

                        vampireKillButton.HasEffect = false; // Block effect on this click
                        vampireKillButton.Timer = vampireKillButton.MaxTimer;
                    }
                    else
                    {
                        bitten = currentTarget;
                        // Notify players about bitten
                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                            LocalPlayer.Control.NetId, (byte)CustomRPC.VampireSetBitten,
                            SendOption.Reliable);
                        writer.Write(bitten.PlayerId);
                        writer.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        /*RPCProcedure.vampireSetBitten(bitten.PlayerId, 0);#1#

                        var lastTimer = (byte)delay;
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(delay,
                            new Action<float>(p =>
                            {
                                // Delayed action
                                if (p <= 1f)
                                {
                                    var timer = (byte)vampireKillButton.Timer;
                                    if (timer != lastTimer)
                                    {
                                        lastTimer = timer;
                                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                                            LocalPlayer.Control.NetId,
                                            (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                                        writer.Write(LocalPlayer.PlayerId);
                                        /*writer.Write((byte)RPCProcedure.GhostInfoTypes.VampireTimer);#1#
                                        writer.Write(timer);
                                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    }
                                }

                                if (p == 1f)
                                {
                                    /#1#/ Perform kill if possible and reset bitten (regardless whether the kill was successful or not)
                                    var res = Helpers.checkMurderAttemptAndKill(vampire, bitten,
                                        showAnimation: false);
                                    if (res == MurderAttemptResult.PerformKill)
                                    {
                                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                                            LocalPlayer.Control.NetId,
                                            (byte)CustomRPC.VampireSetBitten, SendOption.Reliable);
                                        writer.Write(byte.MaxValue);
                                        writer.Write(byte.MaxValue);
                                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                                        RPCProcedure.vampireSetBitten(byte.MaxValue, byte.MaxValue);
                                    }#1#
                                }
                            })));
                        SoundEffectsManager.play("vampireBite");

                        vampireKillButton.HasEffect = true; // Trigger effect on this click
                    }
                }
                /*else if (murder == MurderAttemptResult.BlankKill)
                {
                    vampireKillButton.Timer = vampireKillButton.MaxTimer;
                    vampireKillButton.HasEffect = false;
                }
                else if (murder == MurderAttemptResult.BodyGuardKill)
                {
                    Helpers.checkMuderAttemptAndKill(vampire, currentTarget);
                }#1#
                else
                {
                    vampireKillButton.HasEffect = false;
                }*/
            },
            () => vampire != null && vampire == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, vampireKillButton,
                    targetNearGarlic ? "KILL" : "BITE");
                if (targetNearGarlic && canKillNearGarlics)
                {
                    vampireKillButton.actionButton.graphic.sprite = _hudManager.KillButton.graphic.sprite;
                    vampireKillButton.showButtonText = true;
                }
                else
                {
                    vampireKillButton.actionButton.graphic.sprite = buttonSprite;
                    vampireKillButton.showButtonText = false;
                }

                return currentTarget != null && LocalPlayer.Control.CanMove &&
                       (!targetNearGarlic || canKillNearGarlics);
            },
            () =>
            {
                vampireKillButton.Timer = vampireKillButton.MaxTimer;
                vampireKillButton.isEffectActive = false;
                vampireKillButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            buttonSprite,
            DefButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.Q,
            false,
            0f,
            () => { vampireKillButton.Timer = vampireKillButton.MaxTimer; }
        );

        garlicButton = new CustomButton(
            () =>
            {
                localPlacedGarlic = true;
                var pos = LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var writer = AmongUsClient.Instance.StartRpc(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.PlaceGarlic);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                /*RPCProcedure.placeGarlic(buff);*/
                SoundEffectsManager.play("garlic");
            },
            () => GarlicButton && !localPlacedGarlic && !LocalPlayer.IsDead,
            () => GarlicButton && LocalPlayer.Control.CanMove &&
                  !localPlacedGarlic,
            () => { },
            garlicButtonSprite,
            new Vector3(0, -0.06f, 0),
            _hudManager,
            null,
            true
        );
    }

    public override void ResetCustomButton()
    {
        vampireKillButton.MaxTimer = cooldown;
        vampireKillButton.EffectDuration = delay;
    }
}