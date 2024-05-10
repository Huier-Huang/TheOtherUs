using System;
using System.Collections.Generic;
using Hazel;
using TheOtherRoles.Objects;
using TheOtherRoles.Options;
using TheOtherRoles.Patches;
using TheOtherRoles.Roles.Modifier;
using TheOtherRoles.Roles.Neutral;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Witch : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("SpellButton.png");
    public bool canSpellAnyone;
    public Color color = Palette.ImpostorRed;
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

    public CustomOption witchSpawnRate;

    public CustomButton witchSpellButton;
    public CustomOption witchSpellCastingDuration;
    public CustomOption witchTriggerBothCooldowns;
    public CustomOption witchVoteSavesTargets;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        witch = null;
        futureSpelled = [];
        currentTarget = spellCastingTarget = null;
        cooldown = witchCooldown.getFloat();
        cooldownAddition = witchAdditionalCooldown.getFloat();
        currentCooldownAddition = 0f;
        canSpellAnyone = witchCanSpellAnyone.getBool();
        spellCastingDuration = witchSpellCastingDuration.getFloat();
        triggerBothCooldowns = witchTriggerBothCooldowns.getBool();
        VoteSavesTargets = witchVoteSavesTargets.getBool();
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Witch Spell button
        witchSpellButton = new CustomButton(
            () =>
            {
                if (currentTarget != null)
                {
                    if (Helpers.checkAndDoVetKill(currentTarget)) return;
                    Helpers.checkWatchFlash(currentTarget);
                    spellCastingTarget = currentTarget;
                    SoundEffectsManager.play("witchSpell");
                }
            },
            () =>
            {
                return witch != null && witch == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, witchSpellButton, "");
                if (!witchSpellButton.isEffectActive || spellCastingTarget == currentTarget)
                    return CachedPlayer.LocalPlayer.Control.CanMove && currentTarget != null;
                spellCastingTarget = null;
                witchSpellButton.Timer = 0f;
                witchSpellButton.isEffectActive = false;

                return CachedPlayer.LocalPlayer.Control.CanMove && currentTarget != null;
            },
            () =>
            {
                ButtonHelper.showTargetNameOnButton(null, Get<Arsonist>().arsonistButton, "SPELL");
                witchSpellButton.Timer = witchSpellButton.MaxTimer;
                witchSpellButton.isEffectActive = false;
                spellCastingTarget = null;
            },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F,
            true,
            spellCastingDuration,
            () =>
            {
                if (spellCastingTarget == null) return;
                var attempt = Helpers.checkMuderAttempt(witch, spellCastingTarget);
                if (attempt == MurderAttemptResult.PerformKill)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.SetFutureSpelled,
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
                        var multiplier = Get<Mini>().mini != null && CachedPlayer.LocalPlayer.Control.Is<Mini>()
                            ? Get<Mini>().isGrownUp() ? 0.66f : 2f
                            : 1f;
                        witch.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown *
                                          multiplier;
                    }
                }
                else
                {
                    witchSpellButton.Timer = 0f;
                }

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