using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Blackmailer : RoleBase
{
    public static CustomOption blackmailerCooldown;

    public static CustomButton blackmailerButton;
    public bool alreadyShook = false;

    public readonly ResourceSprite overlaySprite = new ResourceSprite("BlackmailerOverlay.png", 100f);
    public readonly ResourceSprite BlackmailLetterSprite = new("BlackmailerLetter.png");
    public readonly ResourceSprite blackmailButtonSprite = new("BlackmailerBlackmailButton.png");
    public PlayerControl blackmailed;
    public Color blackmailedColor = Palette.White;
    public PlayerControl blackmailer;
    public float cooldown = 30f;
    public PlayerControl currentTarget;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Blackmailer),
        RoleClassType = typeof(Blackmailer),
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        RoleId = RoleId.Blackmailer,
        Color = Palette.ImpostorRed,
        DescriptionText = "Blackmail those who seek to hurt you",
        IntroInfo = "Blackmail those who seek to hurt you",
        GetRole = Get<Blackmailer>,
        CreateRoleController = n => new BlackmailerController(n)
    };
    
    public class BlackmailerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Blackmailer>();
    }
    public override CustomRoleOption roleOption { get; set; }
    
    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        blackmailerCooldown = roleOption.AddChild("Blackmail Cooldown", new FloatOptionSelection(30f, 5f, 120f, 5f));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        blackmailerButton = new CustomButton(
            () =>
            {
                // Action when Pressed
                if (currentTarget == null) return;
                /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);*/
                var writer = AmongUsClient.Instance.StartRpcImmediately(
                    LocalPlayer.Control.NetId, (byte)CustomRPC.BlackmailPlayer,
                    SendOption.Reliable);
                writer.Write(currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                /*RPCProcedure.blackmailPlayer(currentTarget.PlayerId);*/
                blackmailerButton.Timer = blackmailerButton.MaxTimer;
            },
            () => blackmailer != null &&
                  blackmailer == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                // Could Use
                var text = "BLACKMAIL";
                if (blackmailed != null) text = blackmailed.Data.PlayerName;
                ButtonHelper.showTargetNameOnButtonExplicit(currentTarget, blackmailerButton,
                    text); //Show target name under button if setting is true
                return currentTarget != null && LocalPlayer.Control.CanMove;
            },
            () => { blackmailerButton.Timer = blackmailerButton.MaxTimer; },
            blackmailButtonSprite,
            DefButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.F,
            true,
            0f,
            () => { },
            false,
            "Blackmail"
        );
    }

    public override void ResetCustomButton()
    {
        blackmailerButton.MaxTimer = cooldown;
    }


    public override void ClearAndReload()
    {
        blackmailer = null;
        currentTarget = null;
        blackmailed = null;
        cooldown = blackmailerCooldown;
    }
}