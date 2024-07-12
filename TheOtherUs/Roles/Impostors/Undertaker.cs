using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Undertaker : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("UndertakerDragButton.png");
    public bool canDragAndVent;
    public DeadBody deadBodyDraged;

    public float dragingDelaiAfterKill;

    public bool isDraging;
    public PlayerControl undertaker;
    public CustomOption undertakerCanDragAndVent;

    public CustomButton undertakerDragButton;
    public CustomOption undertakerDragingDelaiAfterKill;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Undertaker),
        RoleClassType = typeof(Undertaker),
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        RoleId = RoleId.Undertaker,
        Color = Palette.ImpostorRed,
        GetRole = Get<Undertaker>,
        IntroInfo = "Kill everyone and leave no traces",
        DescriptionText = "Drag up dead bodies to hide them",
        CreateRoleController = player => new UndertakerController(player)
    };
    
    public class UndertakerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Undertaker>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ButtonCreate(HudManager _hudManager)
    {
        undertakerDragButton = new CustomButton(
            () =>
            {
                if (deadBodyDraged == null)
                {
                    foreach (var collider2D in Physics2D.OverlapCircleAll(
                                 LocalPlayer.Control.GetTruePosition(),
                                 LocalPlayer.Control.MaxReportDistance, Constants.PlayersOnlyMask))
                        if (collider2D.tag == "DeadBody")
                        {
                            var deadBody = collider2D.GetComponent<DeadBody>();
                            if (deadBody && !deadBody.Reported)
                            {
                                var playerPosition = LocalPlayer.Control.GetTruePosition();
                                var deadBodyPosition = deadBody.TruePosition;
                                if (!(Vector2.Distance(deadBodyPosition, playerPosition) <=
                                      LocalPlayer.Control.MaxReportDistance) ||
                                    !LocalPlayer.Control.CanMove ||
                                    PhysicsHelpers.AnythingBetween(playerPosition, deadBodyPosition,
                                        Constants.ShipAndObjectsMask, false) || isDraging) continue;
                                var playerInfo = GameData.Instance.GetPlayerById(deadBody.ParentId);
                                var writer = AmongUsClient.Instance.StartRpcImmediately(
                                    LocalPlayer.Control.NetId, (byte)CustomRPC.DragBody,
                                    SendOption.Reliable);
                                writer.Write(playerInfo.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                /*RPCProcedure.dragBody(playerInfo.PlayerId);*/
                                deadBodyDraged = deadBody;
                                break;
                            }
                        }
                }
                else
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.DropBody, SendOption.Reliable);
                    writer.Write(LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    deadBodyDraged = null;
                }
            },
            () => undertaker != null &&
                  undertaker == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                if (deadBodyDraged != null) return true;

                foreach (var collider2D in Physics2D.OverlapCircleAll(
                             LocalPlayer.Control.GetTruePosition(),
                             LocalPlayer.Control.MaxReportDistance, Constants.PlayersOnlyMask))
                    if (collider2D.tag == "DeadBody")
                    {
                        var deadBody = collider2D.GetComponent<DeadBody>();
                        var deadBodyPosition = deadBody.TruePosition;
                        deadBodyPosition.x -= 0.2f;
                        deadBodyPosition.y -= 0.2f;
                        return LocalPlayer.Control.CanMove &&
                               Vector2.Distance(LocalPlayer.Control.GetTruePosition(),
                                   deadBodyPosition) < 0.80f;
                    }

                return false;
            },
            //() => { return ((__instance.ReportButton.renderer.color == Palette.EnabledColor && CachedPlayer.LocalPlayer.Control.CanMove) || Undertaker.deadBodyDraged != null); },
            () => { },
            buttonSprite,
            DefButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.F,
            true,
            0f,
            () => { }
        );
    }

    public override void ClearAndReload()
    {
        undertaker = null;
        isDraging = false;
        canDragAndVent = undertakerCanDragAndVent;
        deadBodyDraged = null;
        dragingDelaiAfterKill = undertakerDragingDelaiAfterKill;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        undertakerDragingDelaiAfterKill = roleOption.AddChild("Draging Delay After Kill", new FloatOptionSelection(0f, 0f,
            15, 1f));
        undertakerCanDragAndVent =
            roleOption.AddChild( "Can Vent While Dragging", new BoolOptionSelection());
    }

    public override void ResetCustomButton()
    {
        undertakerDragButton.MaxTimer = 0f;
    }
}