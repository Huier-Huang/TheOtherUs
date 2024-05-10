using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Modules.Options;
using TheOtherRoles.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Vulture : RoleBase
{
    public PlayerControl vulture;
    public Color color = new Color32(139, 69, 19, byte.MaxValue);
    public List<Arrow> localArrows = [];
    public float cooldown = 30f;
    public int eatNumberToWin = 4;
    public int eatenBodies;
    public bool triggerVultureWin;
    public bool canUseVents = true;
    public bool showArrows = true;
    private ResourceSprite buttonSprite = new ("VultureButton.png");

    public CustomOption vultureSpawnRate;
    public CustomOption vultureCooldown;
    public CustomOption vultureNumberToWin;
    public CustomOption vultureCanUseVents;
    public CustomOption vultureShowArrows;

    public CustomButton vultureEatButton;

    public override void ClearAndReload()
    {
        vulture = null;
        eatNumberToWin = Mathf.RoundToInt(vultureNumberToWin.getFloat());
        eatenBodies = 0;
        cooldown = vultureCooldown.getFloat();
        triggerVultureWin = false;
        canUseVents = vultureCanUseVents.getBool();
        showArrows = vultureShowArrows.getBool();
        if (localArrows != null)
            foreach (var arrow in localArrows.Where(arrow => arrow?.arrow != null))
                Object.Destroy(arrow.arrow);
        localArrows = [];
    }
    public override void OptionCreate()
    {
        vultureSpawnRate = new CustomOption(340, "Vulture".ColorString(color), CustomOptionHolder.rates, null, true);
        vultureCooldown = new CustomOption(341, "Vulture Cooldown", 15f, 10f, 60f, 2.5f, vultureSpawnRate);
        vultureNumberToWin = new CustomOption(342, "Number Of Corpses Needed To Be Eaten", 4f, 1f, 10f, 1f, vultureSpawnRate);
        vultureCanUseVents = new CustomOption(343, "Vulture Can Use Vents", true, vultureSpawnRate);
        vultureShowArrows = new CustomOption(344, "Show Arrows Pointing Towards The Corpses", true, vultureSpawnRate);
    }
    public override void ButtonCreate(HudManager _hudManager)
    {
        // Vulture Eat
        vultureEatButton = new CustomButton(
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
                                writer.Write(vulture.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.cleanBody(playerInfo.PlayerId, vulture.PlayerId);

                                cooldown = vultureEatButton.Timer = vultureEatButton.MaxTimer;
                                SoundEffectsManager.play("vultureEat");
                                break;
                            }
                        }
                    }
            },
            () =>
            {
                return vulture != null && vulture == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return _hudManager.ReportButton.graphic.color == Palette.EnabledColor &&
                       CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () => { vultureEatButton.Timer = vultureEatButton.MaxTimer; },
            buttonSprite,
            CustomButton.ButtonPositions.lowerRowCenter,
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        vultureEatButton.MaxTimer = cooldown;
    }
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}