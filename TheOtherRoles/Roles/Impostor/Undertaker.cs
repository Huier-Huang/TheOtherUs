using System;
using Hazel;
using TheOtherRoles.Objects;
using TheOtherRoles.Options;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Undertaker : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("UndertakerDragButton.png");
    public bool canDragAndVent;
    public Color color = Palette.ImpostorRed;
    public DeadBody deadBodyDraged;

    public float dragingDelaiAfterKill;

    public bool isDraging;
    public PlayerControl undertaker;
    public CustomOption undertakerCanDragAndVent;

    public CustomButton undertakerDragButton;
    public CustomOption undertakerDragingDelaiAfterKill;

    public CustomOption undertakerSpawnRate;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; } = typeof(Undertaker);

    public override void ButtonCreate(HudManager _hudManager)
    {
        undertakerDragButton = new CustomButton(
            () =>
            {
                if (deadBodyDraged == null)
                {
                    foreach (var collider2D in Physics2D.OverlapCircleAll(
                                 CachedPlayer.LocalPlayer.Control.GetTruePosition(),
                                 CachedPlayer.LocalPlayer.Control.MaxReportDistance, Constants.PlayersOnlyMask))
                        if (collider2D.tag == "DeadBody")
                        {
                            var deadBody = collider2D.GetComponent<DeadBody>();
                            if (deadBody && !deadBody.Reported)
                            {
                                var playerPosition = CachedPlayer.LocalPlayer.Control.GetTruePosition();
                                var deadBodyPosition = deadBody.TruePosition;
                                if (!(Vector2.Distance(deadBodyPosition, playerPosition) <=
                                      CachedPlayer.LocalPlayer.Control.MaxReportDistance) ||
                                    !CachedPlayer.LocalPlayer.Control.CanMove ||
                                    PhysicsHelpers.AnythingBetween(playerPosition, deadBodyPosition,
                                        Constants.ShipAndObjectsMask, false) || isDraging) continue;
                                var playerInfo = GameData.Instance.GetPlayerById(deadBody.ParentId);
                                var writer = AmongUsClient.Instance.StartRpcImmediately(
                                    CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.DragBody,
                                    SendOption.Reliable);
                                writer.Write(playerInfo.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.dragBody(playerInfo.PlayerId);
                                deadBodyDraged = deadBody;
                                break;
                            }
                        }
                }
                else
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.DropBody, SendOption.Reliable);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    deadBodyDraged = null;
                }
            },
            () =>
            {
                return undertaker != null &&
                       undertaker == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (deadBodyDraged != null) return true;

                foreach (var collider2D in Physics2D.OverlapCircleAll(
                             CachedPlayer.LocalPlayer.Control.GetTruePosition(),
                             CachedPlayer.LocalPlayer.Control.MaxReportDistance, Constants.PlayersOnlyMask))
                    if (collider2D.tag == "DeadBody")
                    {
                        var deadBody = collider2D.GetComponent<DeadBody>();
                        var deadBodyPosition = deadBody.TruePosition;
                        deadBodyPosition.x -= 0.2f;
                        deadBodyPosition.y -= 0.2f;
                        return CachedPlayer.LocalPlayer.Control.CanMove &&
                               Vector2.Distance(CachedPlayer.LocalPlayer.Control.GetTruePosition(),
                                   deadBodyPosition) < 0.80f;
                    }

                return false;
            },
            //() => { return ((__instance.ReportButton.renderer.color == Palette.EnabledColor && CachedPlayer.LocalPlayer.Control.CanMove) || Undertaker.deadBodyDraged != null); },
            () => { },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft, //brb
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
        canDragAndVent = undertakerCanDragAndVent.getBool();
        deadBodyDraged = null;
        dragingDelaiAfterKill = undertakerDragingDelaiAfterKill.getFloat();
    }

    public override void OptionCreate()
    {
        undertakerSpawnRate =
            new CustomOption(1201, "Undertaker".ColorString(color), CustomOptionHolder.rates, null, true);
        undertakerDragingDelaiAfterKill = new CustomOption(1202, "Draging Delay After Kill", 0f, 0f,
            15, 1f, undertakerSpawnRate);
        undertakerCanDragAndVent =
            new CustomOption(1203, "Can Vent While Dragging", true, undertakerSpawnRate);
    }

    public override void ResetCustomButton()
    {
        undertakerDragButton.MaxTimer = 0f;
    }
}