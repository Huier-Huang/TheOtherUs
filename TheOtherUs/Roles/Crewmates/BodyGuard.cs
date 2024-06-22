using Hazel;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class BodyGuard : RoleBase
{
    public PlayerControl currentTarget;
    private readonly ResourceSprite guardButtonSprite = new("Shield.png");
    public PlayerControl guarded;
    public bool guardFlash;
    public bool reset = true;
    public bool usedGuard;
    
    public class BodyGuardController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase { get; protected set; } = Get<BodyGuard>();
    }

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(145, 102, 64, byte.MaxValue),
        GetRole = Get<BodyGuard>,
        CreateRoleController = n => new BodyGuardController(n),
        DescriptionText = "Protect someone with your own life",
        IntroInfo = "Protect someone with your own life",
        Name = nameof(BodyGuard),
        RoleClassType = typeof(BodyGuard),
        RoleId = RoleId.BodyGuard,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public override CustomRoleOption roleOption { get; set; }

    public void resetGuarded()
    {
        currentTarget = guarded = null;
        usedGuard = false;
    }

    public override void ClearAndReload()
    {
        guardFlash = bodyGuardFlash;
        reset = bodyGuardResetTargetAfterMeeting;
        guarded = null;
        usedGuard = false;
    }

    public CustomOption bodyGuardFlash;

    public CustomButton bodyGuardGuardButton;
    public CustomOption bodyGuardResetTargetAfterMeeting;
    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        bodyGuardResetTargetAfterMeeting = roleOption.AddChild("Reset Target After Meeting", new BoolOptionSelection());
        bodyGuardFlash = roleOption.AddChild("Show Flash On Death", new BoolOptionSelection());
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        bodyGuardGuardButton = new CustomButton(
            () =>
            {
                if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);
                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.BodyGuardGuardPlayer, SendOption.Reliable);
                writer.Write(currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.bodyGuardGuardPlayer(currentTarget.PlayerId);
            },
            RoleIsAlive<BodyGuard>,
            () =>
            {
                if (!usedGuard)
                    ButtonHelper.showTargetNameOnButton(currentTarget, bodyGuardGuardButton, "Guard");
                return CachedPlayer.LocalPlayer.Control.CanMove && currentTarget != null &&
                       !usedGuard;
            },
            () =>
            {
                if (reset) resetGuarded();
            },
            guardButtonSprite,
            DefButtonPositions.lowerRowRight, //brb
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        bodyGuardGuardButton.MaxTimer = 0f;
    }
}