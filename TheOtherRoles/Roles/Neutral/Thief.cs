using System;
using System.Collections.Generic;
using Hazel;
using TheOtherRoles.Objects;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Thief : RoleBase
{
    public bool canKillSheriff;
    public bool canStealWithGuess;
    public bool canUseVents;
    public Color color = new Color32(71, 99, 45, byte.MaxValue);

    public float cooldown = 30f;
    public PlayerControl currentTarget;
    public PlayerControl formerThief;

    public bool hasImpostorVision;

    public bool suicideFlag; // Used as a flag for suicide
    public PlayerControl thief;
    public CustomOption thiefCanKillSheriff;
    public CustomOption thiefCanStealWithGuess;
    public CustomOption thiefCanUseVents;
    public CustomOption thiefCooldown;
    public CustomOption thiefHasImpVision;

    public CustomButton thiefKillButton;

    public List<RoleInfo> ThiefKillList = [Sheriff.roleInfo, Get<Jackal>().RoleInfo, Get<Sidekick>().RoleInfo];

    public CustomOption thiefSpawnRate;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; } = typeof(Thief);

    public override void ClearAndReload()
    {
        thief = null;
        suicideFlag = false;
        currentTarget = null;
        formerThief = null;
        hasImpostorVision = thiefHasImpVision.getBool();
        cooldown = thiefCooldown.getFloat();
        canUseVents = thiefCanUseVents.getBool();
        canKillSheriff = thiefCanKillSheriff.getBool();
        canStealWithGuess = thiefCanStealWithGuess.getBool();
    }

    public bool isFailedThiefKill(PlayerControl target, PlayerControl killer, RoleInfo targetRole)
    {
        return killer == thief && !target.Data.Role.IsImpostor && !ThiefKillList.Contains(targetRole);
    }

    public override void OptionCreate()
    {
        thiefSpawnRate = new CustomOption(400, "Thief".ColorString(color), CustomOptionHolder.rates, null, true);
        thiefCooldown = new CustomOption(401, "Thief Cooldown", 30f, 5f, 120f, 5f, thiefSpawnRate);
        thiefCanKillSheriff = new CustomOption(402, "Thief Can Kill Sheriff", true, thiefSpawnRate);
        thiefHasImpVision = new CustomOption(403, "Thief Has Impostor Vision", true, thiefSpawnRate);
        thiefCanUseVents = new CustomOption(404, "Thief Can Use Vents", true, thiefSpawnRate);
        thiefCanStealWithGuess =
            new CustomOption(405, "Thief Can Guess To Steal A Role (If Guesser)", false, thiefSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        thiefKillButton = new CustomButton(
            () =>
            {
                var target = currentTarget;
                var result = Helpers.checkMuderAttempt(thief, target);
                if (result == MurderAttemptResult.BlankKill)
                {
                    thiefKillButton.Timer = thiefKillButton.MaxTimer;
                    return;
                }

                if (suicideFlag)
                {
                    // Suicide
                    var writer2 = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                        SendOption.Reliable);
                    writer2.Write(thief.PlayerId);
                    writer2.Write(thief.PlayerId);
                    writer2.Write(0);
                    RPCProcedure.uncheckedMurderPlayer(thief.PlayerId, thief.PlayerId, 0);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);
                    thief.clearAllTasks();
                }

                // Steal role if survived.
                if (!thief.Data.IsDead && result == MurderAttemptResult.PerformKill)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.ThiefStealsRole,
                        SendOption.Reliable);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.thiefStealsRole(target.PlayerId);
                }

                // Kill the victim (after becoming their role - so that no win is triggered for other teams)
                if (result == MurderAttemptResult.PerformKill)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                        SendOption.Reliable);
                    writer.Write(thief.PlayerId);
                    writer.Write(target.PlayerId);
                    writer.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.uncheckedMurderPlayer(thief.PlayerId, target.PlayerId, byte.MaxValue);
                }
            },
            () =>
            {
                return thief != null && CachedPlayer.LocalPlayer.Control == thief &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () => { return currentTarget != null && CachedPlayer.LocalPlayer.Control.CanMove; },
            () => { thiefKillButton.Timer = thiefKillButton.MaxTimer; },
            _hudManager.KillButton.graphic.sprite,
            CustomButton.ButtonPositions.upperRowRight,
            _hudManager,
            KeyCode.Q
        );
    }

    public override void ResetCustomButton()
    {
        thiefKillButton.MaxTimer = cooldown;
    }
}