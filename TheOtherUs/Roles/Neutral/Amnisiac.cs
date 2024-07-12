using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherUs.Objects;
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

    private readonly ResourceSprite buttonSprite = new("Remember.png");
    public List<Arrow> localArrows = [];
    public List<PoolablePlayer> poolIcons = [];
    public bool resetRole;

    public bool showArrows = true;
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Amnisiac),
        RoleClassType = typeof(Amnisiac),
        Color= new Color(0.5f, 0.7f, 1f, 1f),
        RoleId = RoleId.Amnisiac,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Neutral,
        GetRole = Get<Amnisiac>,
        IntroInfo = "Steal roles from the dead",
        DescriptionText = "Steal roles from the dead",
        CreateRoleController = player => new AmnisiacController(player)
    };
    public class AmnisiacController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Amnisiac>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        amnisiacShowArrows = roleOption.AddChild("Show Arrows To Dead Bodies", new BoolOptionSelection());
        amnisiacResetRole = roleOption.AddChild("Reset Role When Taken", new BoolOptionSelection());
    }

    public override void ClearAndReload()
    {
        amnisiac = null;
        showArrows = amnisiacShowArrows;
        resetRole = amnisiacResetRole;
        if (localArrows != null)
            foreach (var arrow in localArrows.Where(arrow => arrow?.arrow != null))
                Object.Destroy(arrow.arrow);
        localArrows = [];
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        amnisiacRememberButton = new CustomButton(
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
                            LocalPlayer.Control.NetId, (byte)CustomRPC.AmnisiacTakeRole,
                            SendOption.Reliable);
                        writer.Write(playerInfo.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        /*RPCProcedure.amnisiacTakeRole(playerInfo.PlayerId);*/
                        break;
                    }
            },
            () => amnisiac != null && amnisiac == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () => _hudManager.ReportButton.graphic.color == Palette.EnabledColor &&
                  LocalPlayer.Control.CanMove,
            () => { amnisiacRememberButton.Timer = 0f; },
            buttonSprite,
            DefButtonPositions.lowerRowRight, //brb
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        amnisiacRememberButton.MaxTimer = 0f;
    }
}