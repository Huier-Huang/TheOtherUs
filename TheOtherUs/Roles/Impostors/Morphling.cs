using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Morphling : RoleBase
{
    public float cooldown = 30f;

    public PlayerControl currentTarget;
    public float duration = 10f;
    public PlayerControl morphling;

    private CustomButton morphlingButton;
    public CustomOption morphlingCooldown;
    public CustomOption morphlingDuration;
    
    private readonly ResourceSprite morphSprite = new("MorphButton.png");
    public PlayerControl morphTarget;
    public float morphTimer;
    public PlayerControl sampledTarget;

    private readonly ResourceSprite sampleSprite = new("SampleButton.png");

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Morphling),
        RoleClassType = typeof(Morphling),
        Color = Palette.ImpostorRed,
        GetRole = Get<Morphling>,
        RoleId = RoleId.Morphling,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        IntroInfo = "Change your look",
        DescriptionText = "Change your look to not get caught",
        CreateRoleController = player => new MorphlingController(player)
    };
    
    public class MorphlingController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Morphling>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public void resetMorph()
    {
        morphTarget = null;
        morphTimer = 0f;
        if (morphling == null) return;
        /*morphling.setDefaultLook();*/
    }

    public override void ClearAndReload()
    {
        resetMorph();
        morphling = null;
        currentTarget = null;
        sampledTarget = null;
        morphTarget = null;
        morphTimer = 0f;
        cooldown = morphlingCooldown;
        duration = morphlingDuration;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        morphlingCooldown = roleOption.AddChild("Morphling Cooldown", new FloatOptionSelection(30f, 10f, 60f, 2.5f));
        morphlingDuration = roleOption.AddChild("Morph Duration",  new FloatOptionSelection(10f, 1f, 20f, 0.5f));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        morphlingButton = new CustomButton(
            () =>
            {
                if (sampledTarget != null)
                {
                    /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                    Helpers.checkWatchFlash(currentTarget);*/
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.MorphlingMorph,
                        SendOption.Reliable);
                    writer.Write(sampledTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    /*RPCProcedure.morphlingMorph(sampledTarget.PlayerId);*/
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
            () => morphling != null && morphling == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                if (sampledTarget == null)
                    ButtonHelper.showTargetNameOnButton(currentTarget, morphlingButton, "SAMPLE");
                return (currentTarget || sampledTarget) && !ButtonHelper.isCommsActive() &&
                       LocalPlayer.Control.CanMove && !ButtonHelper.MushroomSabotageActive();
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
            DefButtonPositions.upperRowLeft,
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