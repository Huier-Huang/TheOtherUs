using System;
using System.Collections.Generic;
using Hazel;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Amnisiac : RoleBase
{
    public PlayerControl amnisiac;

    private CustomButton amnisiacRememberButton;
    public CustomOption amnisiacResetRole;
    public CustomOption amnisiacShowArrows;

    public CustomOption amnisiacSpawnRate;

    private readonly ResourceSprite buttonSprite = new("Remember.png");
    public Color color = new(0.5f, 0.7f, 1f, 1f);
    public List<Arrow> localArrows = [];
    public List<PoolablePlayer> poolIcons = [];
    public bool resetRole;

    public bool showArrows = true;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        amnisiac = null;
        showArrows = amnisiacShowArrows;
        resetRole = amnisiacResetRole;
        if (localArrows != null)
            foreach (var arrow in localArrows)
                if (arrow?.arrow != null)
                    Object.Destroy(arrow.arrow);
        localArrows = [];
    }

    public override void OptionCreate()
    {
        amnisiacSpawnRate = new CustomOption(616, "Amnesiac".ColorString(color), CustomOptionHolder.rates, null, true);
        amnisiacShowArrows = new CustomOption(617, "Show Arrows To Dead Bodies", true, amnisiacSpawnRate);
        amnisiacResetRole = new CustomOption(618, "Reset Role When Taken", true, amnisiacSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        amnisiacRememberButton = new CustomButton(
            () =>
            {
                foreach (var collider2D in Physics2D.OverlapCircleAll(
                             CachedPlayer.LocalPlayer.Control.GetTruePosition(),
                             CachedPlayer.LocalPlayer.Control.MaxReportDistance, Constants.PlayersOnlyMask))
                    if (collider2D.tag == "DeadBody")
                    {
                        var component = collider2D.GetComponent<DeadBody>();
                        if (!component || component.Reported) continue;
                        var truePosition = CachedPlayer.LocalPlayer.Control.GetTruePosition();
                        var truePosition2 = component.TruePosition;
                        if (!(Vector2.Distance(truePosition2, truePosition) <=
                              CachedPlayer.LocalPlayer.Control.MaxReportDistance) ||
                            !CachedPlayer.LocalPlayer.Control.CanMove ||
                            PhysicsHelpers.AnythingBetween(truePosition, truePosition2,
                                Constants.ShipAndObjectsMask, false)) continue;
                        var playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                            CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.AmnisiacTakeRole,
                            SendOption.Reliable);
                        writer.Write(playerInfo.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.amnisiacTakeRole(playerInfo.PlayerId);
                        break;
                    }
            },
            () =>
            {
                return amnisiac != null && amnisiac == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () => _hudManager.ReportButton.graphic.color == Palette.EnabledColor &&
                  CachedPlayer.LocalPlayer.Control.CanMove,
            () => { amnisiacRememberButton.Timer = 0f; },
            buttonSprite,
            CustomButton.ButtonPositions.lowerRowRight, //brb
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        amnisiacRememberButton.MaxTimer = 0f;
    }
}