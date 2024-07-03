using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

public class Cleaner : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("CleanButton.png");
    public PlayerControl cleaner;
    private CustomButton cleanerCleanButton;
    public CustomOption cleanerCooldown;

    public float cooldown = 30f;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Cleaner),
        RoleClassType = typeof(Cleaner),
        RoleId = RoleId.Cleaner,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Cleaner>,
        Color = Palette.ImpostorRed,
        DescriptionText = "Clean up dead bodies",
        IntroInfo = "Kill everyone and leave no traces",
        CreateRoleController = n => new CleanerController(n)
    };
    
    public class CleanerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Cleaner>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        cleaner = null;
        cooldown = cleanerCooldown;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        cleanerCooldown = roleOption.AddChild("Cleaner Cooldown", new FloatOptionSelection(30f, 10f, 60f, 2.5f));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Cleaner Clean
        cleanerCleanButton = new CustomButton(
            () =>
            {
                foreach (var collider2D in Physics2D.OverlapCircleAll(
                             LocalPlayer.Control.GetTruePosition(),
                             LocalPlayer.Control.MaxReportDistance, Constants.PlayersOnlyMask))
                    if (collider2D.tag == "DeadBody")
                    {
                        var component = collider2D.GetComponent<DeadBody>();
                        if (!component || component.Reported) continue;
                        var truePosition = LocalPlayer.Control.GetTruePosition();
                        var truePosition2 = component.TruePosition;
                        if (!(Vector2.Distance(truePosition2, truePosition) <=
                              LocalPlayer.Control.MaxReportDistance) ||
                            !LocalPlayer.Control.CanMove ||
                            PhysicsHelpers.AnythingBetween(truePosition, truePosition2,
                                Constants.ShipAndObjectsMask, false)) continue;
                        var playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                            LocalPlayer.Control.NetId, (byte)CustomRPC.CleanBody,
                            SendOption.Reliable);
                        writer.Write(playerInfo.PlayerId);
                        writer.Write(cleaner.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        /*RPCProcedure.cleanBody(playerInfo.PlayerId, cleaner.PlayerId);*/

                        cleaner.killTimer = cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer;
                        SoundEffectsManager.play("cleanerClean");
                        break;
                    }
            },
            () =>
            {
                return cleaner != null && cleaner == LocalPlayer.Control &&
                       !LocalPlayer.IsDead;
            },
            () =>
            {
                return _hudManager.ReportButton.graphic.color == Palette.EnabledColor &&
                       LocalPlayer.Control.CanMove;
            },
            () => { cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer; },
            buttonSprite,
            DefButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );

        // Cleaner Clean
    }

    public override void ResetCustomButton()
    {
        cleanerCleanButton.MaxTimer = cooldown;
    }
}