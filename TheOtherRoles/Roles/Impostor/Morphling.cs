using System;
using Hazel;
using TheOtherRoles.Objects;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Morphling : RoleBase
{
    public Color color = Palette.ImpostorRed;

    public float cooldown = 30f;

    public PlayerControl currentTarget;
    public float duration = 10f;
    public PlayerControl morphling;

    private CustomButton morphlingButton;
    public CustomOption morphlingCooldown;
    public CustomOption morphlingDuration;

    public CustomOption morphlingSpawnRate;
    private readonly ResourceSprite morphSprite = new("MorphButton.png");
    public PlayerControl morphTarget;
    public float morphTimer;
    public PlayerControl sampledTarget;

    private readonly ResourceSprite sampleSprite = new("SampleButton.png");

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public void resetMorph()
    {
        morphTarget = null;
        morphTimer = 0f;
        if (morphling == null) return;
        morphling.setDefaultLook();
    }

    public override void ClearAndReload()
    {
        resetMorph();
        morphling = null;
        currentTarget = null;
        sampledTarget = null;
        morphTarget = null;
        morphTimer = 0f;
        cooldown = morphlingCooldown.getFloat();
        duration = morphlingDuration.getFloat();
    }

    public override void OptionCreate()
    {
        morphlingSpawnRate = new CustomOption(20, "Morphling".ColorString(color), CustomOptionHolder.rates, null, true);
        morphlingCooldown = new CustomOption(21, "Morphling Cooldown", 30f, 10f, 60f, 2.5f, morphlingSpawnRate);
        morphlingDuration = new CustomOption(22, "Morph Duration", 10f, 1f, 20f, 0.5f, morphlingSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        morphlingButton = new CustomButton(
            () =>
            {
                if (sampledTarget != null)
                {
                    if (Helpers.checkAndDoVetKill(currentTarget)) return;
                    Helpers.checkWatchFlash(currentTarget);
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.MorphlingMorph,
                        SendOption.Reliable);
                    writer.Write(sampledTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.morphlingMorph(sampledTarget.PlayerId);
                    sampledTarget = null;
                    morphlingButton.EffectDuration = duration;
                    SoundEffectsManager.play("morphlingMorph");
                }
                else if (currentTarget != null)
                {
                    sampledTarget = currentTarget;
                    morphlingButton.Sprite = morphSprite;
                    morphlingButton.EffectDuration = 1f;
                    SoundEffectsManager.play("morphlingSample");

                    // Add poolable player to the button so that the target outfit is shown
                    ButtonHelper.setButtonTargetDisplay(sampledTarget, morphlingButton);
                }
            },
            () => morphling != null && morphling == CachedPlayer.LocalPlayer.Control &&
                  !CachedPlayer.LocalPlayer.Data.IsDead,
            () =>
            {
                if (sampledTarget == null)
                    ButtonHelper.showTargetNameOnButton(currentTarget, morphlingButton, "SAMPLE");
                return (currentTarget || sampledTarget) && !Helpers.isActiveCamoComms() &&
                       CachedPlayer.LocalPlayer.Control.CanMove && !Helpers.MushroomSabotageActive();
            },
            () =>
            {
                morphlingButton.Timer = morphlingButton.MaxTimer;
                morphlingButton.Sprite = sampleSprite;
                morphlingButton.isEffectActive = false;
                morphlingButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                sampledTarget = null;
                ButtonHelper.setButtonTargetDisplay(null);
            },
            sampleSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F,
            true,
            duration,
            () =>
            {
                if (sampledTarget != null) return;
                morphlingButton.Timer = morphlingButton.MaxTimer;
                morphlingButton.Sprite = sampleSprite;
                SoundEffectsManager.play("morphlingMorph");

                // Reset the poolable player
                ButtonHelper.setButtonTargetDisplay(null);
            }
        );
    }

    public override void ResetCustomButton()
    {
        morphlingButton.MaxTimer = cooldown;
        morphlingButton.EffectDuration = duration;
    }
}