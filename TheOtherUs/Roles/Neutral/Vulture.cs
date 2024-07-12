using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherUs.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Vulture : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("VultureButton.png");
    public bool canUseVents = true;
    public float cooldown = 30f;
    public int eatenBodies;
    public int eatNumberToWin = 4;
    public List<Arrow> localArrows = [];
    public bool showArrows = true;
    public bool triggerVultureWin;
    public PlayerControl vulture;
    public CustomOption vultureCanUseVents;
    public CustomOption vultureCooldown;

    public CustomButton vultureEatButton;
    public CustomOption vultureNumberToWin;
    public CustomOption vultureShowArrows;

    public override CustomRoleOption roleOption { get; set; }

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Vulture),
        RoleClassType = typeof(Vulture),
        RoleId = RoleId.Vulture,
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Vulture>,
        Color = new Color32(139, 69, 19, byte.MaxValue),
        IntroInfo = "Eat corpses to win",
        DescriptionText = "Eat dead bodies",
        CreateRoleController = player => new VultureController(player)
    };
    
    public class VultureController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Vulture>();
    }


    public override void ClearAndReload()
    {
        vulture = null;
        eatNumberToWin = Mathf.RoundToInt(vultureNumberToWin);
        eatenBodies = 0;
        cooldown = vultureCooldown;
        triggerVultureWin = false;
        canUseVents = vultureCanUseVents;
        showArrows = vultureShowArrows;
        if (localArrows != null)
            foreach (var arrow in localArrows.Where(arrow => arrow?.arrow != null))
                Object.Destroy(arrow.arrow);
        localArrows = [];
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        vultureCooldown = roleOption.AddChild("Vulture Cooldown", new FloatOptionSelection(15f, 10f, 60f, 2.5f));
        vultureNumberToWin =
            roleOption.AddChild("Number Of Corpses Needed To Be Eaten", new FloatOptionSelection(4f, 1f, 10f, 1f));
        vultureCanUseVents = roleOption.AddChild( "Vulture Can Use Vents", new BoolOptionSelection());
        vultureShowArrows = roleOption.AddChild( "Show Arrows Pointing Towards The Corpses", new BoolOptionSelection());
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Vulture Eat
        vultureEatButton = new CustomButton(
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
                        writer.Write(vulture.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        /*RPCProcedure.cleanBody(playerInfo.PlayerId, vulture.PlayerId);*/

                        cooldown = vultureEatButton.Timer = vultureEatButton.MaxTimer;
                        SoundEffectsManager.play("vultureEat");
                        break;
                    }
            },
            () => vulture != null && vulture == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () => _hudManager.ReportButton.graphic.color == Palette.EnabledColor &&
                  LocalPlayer.Control.CanMove,
            () => { vultureEatButton.Timer = vultureEatButton.MaxTimer; },
            buttonSprite,
            DefButtonPositions.lowerRowCenter,
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        vultureEatButton.MaxTimer = cooldown;
    }
}