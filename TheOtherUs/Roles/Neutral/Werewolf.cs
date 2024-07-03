using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Werewolf : RoleBase
{
    public static readonly RoleInfo roleInfo = new()
    {
        Name = nameof(Werewolf),
        RoleId = RoleId.Werewolf,
        Color = new Color32(79, 56, 21, byte.MaxValue),
        DescriptionText = "Rampage and kill everyone",
        IntroInfo = "Rampage and kill everyone",
        RoleClassType = typeof(Werewolf),
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Werewolf>,
        CreateRoleController = player => new WerewolfController(player)
    };
    
    public class WerewolfController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Werewolf>();
    }

    public ResourceSprite buttonSprite = new("Rampage.png");
    public bool canKill;
    public bool canUseVents;
    public PlayerControl currentTarget;
    public bool hasImpostorVision;

    // Kill Button 
    public float killCooldown = 3f;

    // Rampage Button
    public float rampageCooldown = 30f;
    public float rampageDuration = 5f;
    public PlayerControl werewolf;
    public CustomButton werewolfKillButton;
    public CustomOption werewolfKillCooldown;

    public CustomButton werewolfRampageButton;
    public CustomOption werewolfRampageCooldown;
    public CustomOption werewolfRampageDuration;
    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;

    public override CustomRoleOption roleOption { get; set; }
    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public Vector3 RampageVector = new(-2.7f, -0.06f, 0);


    public override void ClearAndReload()
    {
        werewolf = null;
        currentTarget = null;
        canUseVents = false;
        canKill = false;
        hasImpostorVision = false;
        rampageCooldown = werewolfRampageCooldown;
        rampageDuration = werewolfRampageDuration;
        killCooldown = werewolfKillCooldown;
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Werewolf Kill
        werewolfKillButton = new CustomButton(
            () =>
            {
                /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                if (Helpers.checkMuderAttemptAndKill(werewolf, currentTarget) ==
                    MurderAttemptResult.SuppressKill) return;*/

                werewolfKillButton.Timer = werewolfKillButton.MaxTimer;
                currentTarget = null;
            },
            () => werewolf != null && werewolf == LocalPlayer.Control &&
                  !LocalPlayer.IsDead && canKill,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, werewolfKillButton, "KILL");
                return currentTarget && LocalPlayer.Control.CanMove;
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
            () => werewolf != null && werewolf == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () => LocalPlayer.Control.CanMove,
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
            DefButtonPositions.lowerRowRight, //brb
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
}