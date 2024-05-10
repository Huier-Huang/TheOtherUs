using System;
using Hazel;
using TheOtherRoles.Objects;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class BodyGuard : RoleBase
{
    public PlayerControl bodyguard;
    public CustomOption bodyGuardFlash;

    public CustomButton bodyGuardGuardButton;
    public CustomOption bodyGuardResetTargetAfterMeeting;

    public CustomOption bodyGuardSpawnRate;
    public Color color = new Color32(145, 102, 64, byte.MaxValue);
    public PlayerControl currentTarget;
    private readonly ResourceSprite guardButtonSprite = new("Shield.png");
    public PlayerControl guarded;
    public bool guardFlash;
    public bool reset = true;
    public bool usedGuard;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public void resetGuarded()
    {
        currentTarget = guarded = null;
        usedGuard = false;
    }

    public override void ClearAndReload()
    {
        bodyguard = null;
        guardFlash = bodyGuardFlash;
        reset = bodyGuardResetTargetAfterMeeting;
        guarded = null;
        usedGuard = false;
    }

    public override void OptionCreate()
    {
        bodyGuardSpawnRate =
            new CustomOption(8820, "Bodyguard".ColorString(color), CustomOptionHolder.rates, null, true);
        bodyGuardResetTargetAfterMeeting = new CustomOption(8821, "Reset Target After Meeting", true,
            bodyGuardSpawnRate);
        bodyGuardFlash = new CustomOption(8822, Types.Crewmate, "Show Flash On Death", true, bodyGuardSpawnRate);
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
            () => bodyguard != null && bodyguard == CachedPlayer.LocalPlayer.Control &&
                  !CachedPlayer.LocalPlayer.Data.IsDead,
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
            CustomButton.ButtonPositions.lowerRowRight, //brb
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        bodyGuardGuardButton.MaxTimer = 0f;
    }
}