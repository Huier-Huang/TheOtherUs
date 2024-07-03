using System;
using System.Collections.Generic;
using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Thief : RoleBase
{
    public bool canKillSheriff;
    public bool canStealWithGuess;
    public bool canUseVents;

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

    public override CustomRoleOption roleOption { get; set; }

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Thief),
        RoleClassType = typeof(Thief),
        RoleId = RoleId.Thief,
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Thief>,
        Color =  new Color32(71, 99, 45, byte.MaxValue),
        IntroInfo = "Steal a killers role by killing them",
        DescriptionText = "Steal a killers role",
        CreateRoleController = player => new ThiefController(player)
    };
    
    public class ThiefController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Thief>();
    }
    
    public override void ClearAndReload()
    {
        thief = null;
        suicideFlag = false;
        currentTarget = null;
        formerThief = null;
        hasImpostorVision = thiefHasImpVision;
        cooldown = thiefCooldown;
        canUseVents = thiefCanUseVents;
        canKillSheriff = thiefCanKillSheriff;
        canStealWithGuess = thiefCanStealWithGuess;
    }

    public bool isFailedThiefKill(PlayerControl target, PlayerControl killer, RoleInfo targetRole)
    {
        return killer == thief && !target.Data.Role.IsImpostor && !ThiefKillList.Contains(targetRole);
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        thiefCooldown = roleOption.AddChild("Thief Cooldown", new FloatOptionSelection(30f, 5f, 120f, 5f));
        thiefCanKillSheriff =  roleOption.AddChild("Thief Can Kill Sheriff", new BoolOptionSelection());
        thiefHasImpVision = roleOption.AddChild("Thief Has Impostor Vision", new BoolOptionSelection());
        thiefCanUseVents = roleOption.AddChild( "Thief Can Use Vents", new BoolOptionSelection());
        thiefCanStealWithGuess = roleOption.AddChild("Thief Can Guess To Steal A Role (If Guesser)", new BoolOptionSelection(false));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        thiefKillButton = new CustomButton(
            () =>
            {
                /*var target = currentTarget;
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
                        LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                        SendOption.Reliable);
                    writer2.Write(thief.PlayerId);
                    writer2.Write(thief.PlayerId);
                    writer2.Write(0);
                    /*RPCProcedure.uncheckedMurderPlayer(thief.PlayerId, thief.PlayerId, 0);#1#
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);
                    /*thief.clearAllTasks();#1#
                }

                // Steal role if survived.
                if (!thief.Data.IsDead && result == MurderAttemptResult.PerformKill)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.ThiefStealsRole,
                        SendOption.Reliable);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    /*RPCProcedure.thiefStealsRole(target.PlayerId);#1#
                }

                // Kill the victim (after becoming their role - so that no win is triggered for other teams)
                if (result != MurderAttemptResult.PerformKill) return;
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                        SendOption.Reliable);
                    writer.Write(thief.PlayerId);
                    writer.Write(target.PlayerId);
                    writer.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    /*RPCProcedure.uncheckedMurderPlayer(thief.PlayerId, target.PlayerId, byte.MaxValue);#1#
                }*/
            },
            () => thief != null && LocalPlayer.Control == thief &&
                  !LocalPlayer.IsDead,
            () => currentTarget != null && LocalPlayer.Control.CanMove,
            () => { thiefKillButton.Timer = thiefKillButton.MaxTimer; },
            _hudManager.KillButton.graphic.sprite,
            DefButtonPositions.upperRowRight,
            _hudManager,
            KeyCode.Q
        );
    }

    public override void ResetCustomButton()
    {
        thiefKillButton.MaxTimer = cooldown;
    }
}