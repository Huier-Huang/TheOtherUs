using System;
using Hazel;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Impostor;

public class Cleaner : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("CleanButton.png");
    public PlayerControl cleaner;
    private CustomButton cleanerCleanButton;
    public CustomOption cleanerCooldown;

    public CustomOption cleanerSpawnRate;
    public Color color = Palette.ImpostorRed;

    public float cooldown = 30f;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        cleaner = null;
        cooldown = cleanerCooldown.getFloat();
    }

    public override void OptionCreate()
    {
        cleanerSpawnRate = new CustomOption(260, "Cleaner".ColorString(color), CustomOptionHolder.rates, null, true);
        cleanerCooldown = new CustomOption(261, "Cleaner Cooldown", 30f, 10f, 60f, 2.5f, cleanerSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Cleaner Clean
        cleanerCleanButton = new CustomButton(
            () =>
            {
                foreach (var collider2D in Physics2D.OverlapCircleAll(
                             CachedPlayer.LocalPlayer.Control.GetTruePosition(),
                             CachedPlayer.LocalPlayer.Control.MaxReportDistance, Constants.PlayersOnlyMask))
                    if (collider2D.tag == "DeadBody")
                    {
                        var component = collider2D.GetComponent<DeadBody>();
                        if (component && !component.Reported)
                        {
                            var truePosition = CachedPlayer.LocalPlayer.Control.GetTruePosition();
                            var truePosition2 = component.TruePosition;
                            if (Vector2.Distance(truePosition2, truePosition) <=
                                CachedPlayer.LocalPlayer.Control.MaxReportDistance &&
                                CachedPlayer.LocalPlayer.Control.CanMove &&
                                !PhysicsHelpers.AnythingBetween(truePosition, truePosition2,
                                    Constants.ShipAndObjectsMask, false))
                            {
                                var playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                var writer = AmongUsClient.Instance.StartRpcImmediately(
                                    CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.CleanBody,
                                    SendOption.Reliable);
                                writer.Write(playerInfo.PlayerId);
                                writer.Write(cleaner.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.cleanBody(playerInfo.PlayerId, cleaner.PlayerId);

                                cleaner.killTimer = cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer;
                                SoundEffectsManager.play("cleanerClean");
                                break;
                            }
                        }
                    }
            },
            () =>
            {
                return cleaner != null && cleaner == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return _hudManager.ReportButton.graphic.color == Palette.EnabledColor &&
                       CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () => { cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer; },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
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