using System;
using System.Collections.Generic;
using TheOtherRoles.Modules.Options;
using TheOtherRoles.Objects;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Werewolf : RoleBase
{
    public PlayerControl werewolf;
    public PlayerControl currentTarget;
    public Color color = new Color32(79, 56, 21, byte.MaxValue);

    // Kill Button 
    public float killCooldown = 3f;

    // Rampage Button
    public float rampageCooldown = 30f;
    public float rampageDuration = 5f;
    public bool canUseVents;
    public bool canKill;
    public bool hasImpostorVision;

    public ResourceSprite buttonSprite = new ("Rampage.png");

    public CustomOption werewolfSpawnRate;
    public CustomOption werewolfRampageCooldown;
    public CustomOption werewolfRampageDuration;
    public CustomOption werewolfKillCooldown;

    public CustomButton werewolfRampageButton;
    public CustomButton werewolfKillButton;


    public static Vector3 getRampageVector()
    {
        return new Vector3(-2.7f, -0.06f, 0);
    }

    public override void ClearAndReload()
    {
        werewolf = null;
        currentTarget = null;
        canUseVents = false;
        canKill = false;
        hasImpostorVision = false;
        rampageCooldown = werewolfRampageCooldown.getFloat();
        rampageDuration = werewolfRampageDuration.getFloat();
        killCooldown = werewolfKillCooldown.getFloat();
    }
    public override void ButtonCreate(HudManager _hudManager)
    {
        // Werewolf Kill
        werewolfKillButton = new CustomButton(
            () =>
            {
                if (Helpers.checkAndDoVetKill(currentTarget)) return;
                if (Helpers.checkMuderAttemptAndKill(werewolf, currentTarget) ==
                    MurderAttemptResult.SuppressKill) return;

                werewolfKillButton.Timer = werewolfKillButton.MaxTimer;
                currentTarget = null;
            },
            () => werewolf != null && werewolf == CachedPlayer.LocalPlayer.Control &&
                  !CachedPlayer.LocalPlayer.Data.IsDead && canKill,
            () =>
            { 
                ButtonHelper.showTargetNameOnButton(currentTarget, werewolfKillButton, "KILL");
                return currentTarget && CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () => { werewolfKillButton.Timer = werewolfKillButton.MaxTimer; },
            _hudManager.KillButton.graphic.sprite,
            new Vector3(0, 1f, 0),
            _hudManager,
            KeyCode.Q
        );

        werewolfRampageButton = new CustomButton(
            () =>
            {
                canKill = true;
                hasImpostorVision = true;
                werewolfKillButton.Timer = 0f;
            },
            () => werewolf != null && werewolf == CachedPlayer.LocalPlayer.Control &&
                  !CachedPlayer.LocalPlayer.Data.IsDead,
            () => CachedPlayer.LocalPlayer.Control.CanMove,
            () =>
            {
                /* On Meeting End */
                werewolfRampageButton.Timer = werewolfRampageButton.MaxTimer;
                werewolfRampageButton.isEffectActive = false;
                werewolfRampageButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                canKill = false;
                //  Werewolf.canUseVents = false;
                hasImpostorVision = false;
            },
            buttonSprite,
            CustomButton.ButtonPositions.lowerRowRight, //brb
            _hudManager,
            KeyCode.G,
            true,
            rampageDuration,
            () =>
            {
                werewolfRampageButton.Timer = werewolfRampageButton.MaxTimer;
                canKill = false;
                hasImpostorVision = false;
            }
        );
    }
    public override void ResetCustomButton()
    {
        werewolfKillButton.MaxTimer = killCooldown;
        werewolfRampageButton.MaxTimer = rampageCooldown;
        werewolfRampageButton.EffectDuration = rampageDuration;
    }

    public static readonly RoleInfo roleInfo = new()
    {
        Name = nameof(Werewolf),
        RoleId = RoleId.Werewolf,
        Color = new Color32(79, 56, 21, byte.MaxValue),
        Description = "Rampage and kill everyone",
        IntroInfo = "Rampage and kill everyone",
        RoleClassType = typeof(Werewolf),
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Werewolf>
    };
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}