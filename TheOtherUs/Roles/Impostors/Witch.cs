using System.Collections.Generic;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Witch : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("SpellButton.png");
    public bool canSpellAnyone;
    public float cooldown = 30f;
    public float cooldownAddition = 10f;
    public float currentCooldownAddition;
    public PlayerControl currentTarget;

    public List<PlayerControl> futureSpelled = [];
    public float spellCastingDuration = 2f;
    public PlayerControl spellCastingTarget;

    private ResourceSprite spelledOverlaySprite = new("SpellButtonMeeting.png", 225f);
    public bool triggerBothCooldowns = true;
    public bool VoteSavesTargets = true;
    public PlayerControl witch;
    public CustomOption witchAdditionalCooldown;
    public CustomOption witchCanSpellAnyone;
    public CustomOption witchCooldown;

    public CustomButton witchSpellButton;
    public CustomOption witchSpellCastingDuration;
    public CustomOption witchTriggerBothCooldowns;
    public CustomOption witchVoteSavesTargets;
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Witch),
        RoleClassType = typeof(Witch),
        Color = Palette.ImpostorRed,
        RoleId = RoleId.Witch,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Impostor,
        GetRole = Get<Witch>,
        IntroInfo = "Cast a spell upon your foes",
        DescriptionText = "Cast a spell upon your foes",
        CreateRoleController = player => new WitchController(player)
    };
    public class WitchController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Witch>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        witch = null;
        futureSpelled = [];
        currentTarget = spellCastingTarget = null;
        cooldown = witchCooldown;
        cooldownAddition = witchAdditionalCooldown;
        currentCooldownAddition = 0f;
        canSpellAnyone = witchCanSpellAnyone;
        spellCastingDuration = witchSpellCastingDuration;
        triggerBothCooldowns = witchTriggerBothCooldowns;
        VoteSavesTargets = witchVoteSavesTargets;
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Witch Spell button
        witchSpellButton = new CustomButton(
            () =>
            {
                if (currentTarget == null) return;
                /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                    Helpers.checkWatchFlash(currentTarget);*/
                spellCastingTarget = currentTarget;
                SoundEffectsManager.play("witchSpell");
            },
            () => witch != null && witch == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, witchSpellButton, "");
                if (!witchSpellButton.isEffectActive || spellCastingTarget == currentTarget)
                    return LocalPlayer.Control.CanMove && currentTarget != null;
                spellCastingTarget = null;
                witchSpellButton.Timer = 0f;
                witchSpellButton.isEffectActive = false;

                return LocalPlayer.Control.CanMove && currentTarget != null;
            },
            () =>
            {
                ButtonHelper.showTargetNameOnButton(null, Get<Arsonist>().arsonistButton, "SPELL");
                witchSpellButton.Timer = witchSpellButton.MaxTimer;
                witchSpellButton.isEffectActive = false;
                spellCastingTarget = null;
            },
            buttonSprite,
            DefButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F,
            true,
            spellCastingDuration,
            () =>
            {
                if (spellCastingTarget == null) return;
                /*var attempt = Helpers.checkMuderAttempt(witch, spellCastingTarget);
                if (attempt == MurderAttemptResult.PerformKill)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.SetFutureSpelled,
                        SendOption.Reliable);
                    writer.Write(currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.setFutureSpelled(currentTarget.PlayerId);
                }

                if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill)
                {
                    currentCooldownAddition += cooldownAddition;
                    witchSpellButton.MaxTimer = cooldown + currentCooldownAddition;
                    PlayerControlFixedUpdatePatch
                        .miniCooldownUpdate(); // Modifies the MaxTimer if the witch is the mini
                    witchSpellButton.Timer = witchSpellButton.MaxTimer;
                    if (triggerBothCooldowns)
                    {
                        var multiplier = Get<Mini>().mini != null && LocalPlayer.Control.Is<Mini>()
                            ? Get<Mini>().isGrownUp() ? 0.66f : 2f
                            : 1f;
                        witch.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown *
                                          multiplier;
                    }
                }
                else
                {
                    witchSpellButton.Timer = 0f;
                }*/

                spellCastingTarget = null;
            }
        );
    }

    public override void ResetCustomButton()
    {
        witchSpellButton.MaxTimer = cooldown;
        witchSpellButton.EffectDuration = spellCastingDuration;
    }
}