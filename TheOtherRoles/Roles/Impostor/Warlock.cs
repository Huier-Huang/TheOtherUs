using System;
using Hazel;
using TheOtherRoles.Objects;
using TheOtherRoles.Roles.Modifier;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Warlock : RoleBase
{
    public Color color = Palette.ImpostorRed;

    public float cooldown = 30f;

    public PlayerControl currentTarget;

    private readonly ResourceSprite curseButtonSprite = new("CurseButton.png");
    private readonly ResourceSprite curseKillButtonSprite = new("CurseKillButton.png");
    public PlayerControl curseVictim;
    public PlayerControl curseVictimTarget;
    public float rootTime = 5f;
    public PlayerControl warlock;
    public CustomOption warlockCooldown;
    public CustomButton warlockCurseButton;
    public CustomOption warlockRootTime;

    public CustomOption warlockSpawnRate;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        warlock = null;
        currentTarget = null;
        curseVictim = null;
        curseVictimTarget = null;
        cooldown = warlockCooldown.getFloat();
        rootTime = warlockRootTime.getFloat();
    }

    public void resetCurse()
    {
        warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
        warlockCurseButton.Sprite = curseButtonSprite;
        warlockCurseButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        currentTarget = null;
        curseVictim = null;
        curseVictimTarget = null;
    }

    public override void OptionCreate()
    {
        warlockSpawnRate = new CustomOption(270, "Warlock".ColorString(color), CustomOptionHolder.rates, null, true);
        warlockCooldown = new CustomOption(271, "Warlock Cooldown", 30f, 10f, 60f, 2.5f, warlockSpawnRate);
        warlockRootTime = new CustomOption(272, "Warlock Root Time", 5f, 0f, 15f, 1f, warlockSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Warlock curse
        warlockCurseButton = new CustomButton(
            () =>
            {
                if (curseVictim == null)
                {
                    if (Helpers.checkAndDoVetKill(currentTarget)) return;
                    Helpers.checkWatchFlash(currentTarget);
                    // Apply Curse
                    curseVictim = currentTarget;
                    warlockCurseButton.Sprite = curseKillButtonSprite;
                    warlockCurseButton.Timer = 1f;
                    SoundEffectsManager.play("warlockCurse");

                    // Ghost Info
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.ShareGhostInfo,
                        SendOption.Reliable);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.WarlockTarget);
                    writer.Write(curseVictim.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
                else if (curseVictim != null && curseVictimTarget != null)
                {
                    var murder = Helpers.checkMurderAttemptAndKill(warlock, curseVictimTarget,
                        showAnimation: false);
                    if (murder == MurderAttemptResult.SuppressKill) return;

                    // If blanked or killed
                    if (rootTime > 0)
                    {
                        Get<AntiTeleport>().position = CachedPlayer.LocalPlayer.transform.position;
                        CachedPlayer.LocalPlayer.Control.moveable = false;
                        CachedPlayer.LocalPlayer.NetTransform
                            .Halt(); // Stop current movement so the warlock is not just running straight into the next object
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(rootTime,
                            new Action<float>(p =>
                            {
                                // Delayed action
                                if (p == 1f) CachedPlayer.LocalPlayer.Control.moveable = true;
                            })));
                    }

                    curseVictim = null;
                    curseVictimTarget = null;
                    warlockCurseButton.Sprite = curseButtonSprite;
                    warlock.killTimer = warlockCurseButton.Timer = warlockCurseButton.MaxTimer;

                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.ShareGhostInfo,
                        SendOption.Reliable);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.WarlockTarget);
                    writer.Write(byte.MaxValue); // This will set it to null!
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            },
            () =>
            {
                return warlock != null && warlock == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, warlockCurseButton,
                    curseVictim != null ? "KILL" : "CURSE");
                return ((curseVictim == null && currentTarget != null) ||
                        (curseVictim != null && curseVictimTarget != null)) &&
                       CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () =>
            {
                warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
                warlockCurseButton.Sprite = curseButtonSprite;
                curseVictim = null;
                curseVictimTarget = null;
            },
            curseButtonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        warlockCurseButton.MaxTimer = cooldown;
    }
}