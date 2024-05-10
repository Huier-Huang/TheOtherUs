using System;
using TheOtherRoles.Objects;
using TheOtherRoles.Roles.Modifier;
using TheOtherRoles.Roles.Neutral;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Sheriff : RoleBase
{
    public static readonly RoleInfo roleInfo = new()
    {
        Name = nameof(sheriff),
        Color = new Color32(248, 205, 70, byte.MaxValue),
        Description = "Shoot the Impostors",
        IntroInfo = "Shoot the <color=#FF1919FF>Impostors</color>",
        RoleId = RoleId.Sheriff,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Sheriff>
    };

    public bool canKillAmnesiac;
    public bool canKillArsonist;
    public bool canKillJester;
    public bool canKillLawyer;
    public bool canKillNeutrals;
    public bool canKillProsecutor;
    public bool canKillPursuer;
    public bool canKillThief;
    public bool canKillVulture;

    public float cooldown = 30f;

    public PlayerControl currentTarget;

    public PlayerControl formerDeputy; // Needed for keeping handcuffs + shifting
    public PlayerControl formerSheriff; // When deputy gets promoted...
    public int misfireKills; // Self: 0, Target: 1, Both: 2
    public PlayerControl sheriff;
    public CustomOption sheriffCanKillAmnesiac;
    public CustomOption sheriffCanKillArsonist;
    public CustomOption sheriffCanKillJester;
    public CustomOption sheriffCanKillLawyer;
    public CustomOption sheriffCanKillNeutrals;
    public CustomOption sheriffCanKillProsecutor;
    public CustomOption sheriffCanKillPursuer;
    public CustomOption sheriffCanKillThief;
    public CustomOption sheriffCanKillVulture;
    public CustomOption sheriffCooldown;

    public CustomButton sheriffKillButton;
    public CustomOption sheriffMisfireKills;


    public CustomOption sheriffSpawnRate;
    public bool spyCanDieToSheriff;

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override Type RoleType { get; protected set; } = typeof(Sheriff);

    public void replaceCurrentSheriff(PlayerControl deputy)
    {
        if (!formerSheriff) formerSheriff = sheriff;
        sheriff = deputy;
        currentTarget = null;
        cooldown = sheriffCooldown;
    }

    public override void ClearAndReload()
    {
        sheriff = null;
        currentTarget = null;
        formerDeputy = null;
        formerSheriff = null;
        misfireKills = sheriffMisfireKills.getSelection();
        cooldown = sheriffCooldown;
        canKillNeutrals = sheriffCanKillNeutrals;
        canKillArsonist = sheriffCanKillArsonist;
        canKillLawyer = sheriffCanKillLawyer;
        canKillJester = sheriffCanKillJester;
        canKillPursuer = sheriffCanKillPursuer;
        canKillVulture = sheriffCanKillVulture;
        canKillThief = sheriffCanKillThief;
        canKillAmnesiac = sheriffCanKillAmnesiac;
        canKillProsecutor = sheriffCanKillProsecutor;
        spyCanDieToSheriff = CustomOptionHolder.spyCanDieToSheriff;
    }

    public override void OptionCreate()
    {
        sheriffSpawnRate = new CustomOption(100, "Sheriff".ColorString(roleInfo.Color), CustomOptionHolder.rates, null,
            true);
        sheriffCooldown =
            new CustomOption(101, "Sheriff Cooldown", 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
        sheriffMisfireKills = new CustomOption(2101, "Misfire Kills",
            new[] { "Self", "Target", "Both" }, sheriffSpawnRate);
        sheriffCanKillNeutrals =
            new CustomOption(102, "Sheriff Can Kill Neutrals", false, sheriffSpawnRate);
        sheriffCanKillJester = new CustomOption(2104,
            "Sheriff Can Kill " + "Jester".ColorString(GetColor<Jester>()), false, sheriffCanKillNeutrals);
        sheriffCanKillProsecutor = new CustomOption(2105,
            "Sheriff Can Kill " + "Prosecutor".ColorString(GetColor<Jester>()), false, sheriffCanKillNeutrals);
        sheriffCanKillAmnesiac = new CustomOption(210278,
            "Sheriff Can Kill " + "Amnesiac".ColorString(GetColor<Amnisiac>()), false, sheriffCanKillNeutrals);
        sheriffCanKillArsonist = new CustomOption(2102,
            "Sheriff Can Kill " + "Arsonist".ColorString(GetColor<Arsonist>()), false, sheriffCanKillNeutrals);
        sheriffCanKillVulture = new CustomOption(2107,
            "Sheriff Can Kill " + "Vulture".ColorString(GetColor<Vulture>()), false, sheriffCanKillNeutrals);
        sheriffCanKillLawyer = new CustomOption(2103,
            "Sheriff Can Kill " + "Lawyer".ColorString(GetColor<Lawyer>()), false, sheriffCanKillNeutrals);
        sheriffCanKillThief = new CustomOption(210277,
            "Sheriff Can Kill " + "Thief".ColorString(GetColor<Thief>()), false, sheriffCanKillNeutrals);
        sheriffCanKillPursuer = new CustomOption(2106,
            "Sheriff Can Kill " + "Pursuer".ColorString(GetColor<Pursuer>()), false, sheriffCanKillNeutrals);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        sheriffKillButton = new CustomButton(
            () =>
            {
                if (Helpers.checkAndDoVetKill(currentTarget)) return;
                var murderAttemptResult = Helpers.checkMuderAttempt(sheriff, currentTarget);
                switch (murderAttemptResult)
                {
                    case MurderAttemptResult.SuppressKill:
                        return;
                    case MurderAttemptResult.PerformKill:
                    {
                        var targetId = PlayerControl.LocalPlayer.PlayerId;
                        if
                        (
                            !currentTarget.Is<Mini>() || (Get<Mini>().isGrownUp()
                                                          &&
                                                          (currentTarget.Data.Role.IsImpostor ||
                                                           currentTarget.GetRole() is Jackal or Sidekick or Werewolf))
                                                      ||
                                                      (spyCanDieToSheriff && currentTarget.Is<Spy>())
                                                      ||
                                                      (
                                                          canKillNeutrals
                                                          &&
                                                          (
                                                              (currentTarget.Is<Arsonist>() && canKillArsonist) ||
                                                              (currentTarget.Is<Jester>() && canKillJester) ||
                                                              (currentTarget.Is<Vulture>() && canKillVulture) ||
                                                              (currentTarget.Is<Lawyer>() && canKillLawyer &&
                                                               !Get<Lawyer>().isProsecutor) ||
                                                              (currentTarget.Is<Thief>() && canKillThief) ||
                                                              (currentTarget.Is<Amnisiac>() && canKillAmnesiac) ||
                                                              (currentTarget.Is<Lawyer>() && canKillProsecutor &&
                                                               Get<Lawyer>().isProsecutor) ||
                                                              (currentTarget.Is<Pursuer>() && canKillPursuer)
                                                          )
                                                      ))
                            targetId = currentTarget.PlayerId;
                        else
                            switch (misfireKills)
                            {
                                case 0:
                                    targetId = CachedPlayer.LocalPlayer.PlayerId;
                                    break;
                                case 1:
                                    targetId = currentTarget.PlayerId;
                                    break;
                                case 2:
                                {
                                    targetId = currentTarget.PlayerId;

                                    var killWriter2 = FastRpcWriter.StartNewRpcWriter(CustomRPC.UncheckedMurderPlayer)
                                        .Write(sheriff.Data.PlayerId)
                                        .Write(CachedPlayer.LocalPlayer.PlayerId)
                                        .Write(byte.MaxValue);
                                    killWriter2.RPCSend();
                                    RPCProcedure.uncheckedMurderPlayer(sheriff.Data.PlayerId,
                                        CachedPlayer.LocalPlayer.PlayerId, byte.MaxValue);
                                    break;
                                }
                            }

                        var killWriter = FastRpcWriter.StartNewRpcWriter(CustomRPC.UncheckedMurderPlayer)
                            .Write(sheriff.Data.PlayerId)
                            .Write(targetId)
                            .Write(byte.MaxValue);
                        killWriter.RPCSend();
                        RPCProcedure.uncheckedMurderPlayer(sheriff.Data.PlayerId, targetId, byte.MaxValue);
                        break;
                    }
                    case MurderAttemptResult.BodyGuardKill:
                        Helpers.checkMuderAttemptAndKill(sheriff, currentTarget);
                        break;
                }

                sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                currentTarget = null;
            },
            () => sheriff != null && CachedPlayer.LocalPlayer.Control.Is<Sheriff>() &&
                  !CachedPlayer.LocalPlayer.Data.IsDead,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, sheriffKillButton, "KILL");
                return currentTarget && CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer; },
            _hudManager.KillButton.graphic.sprite,
            CustomButton.ButtonPositions.upperRowRight,
            _hudManager,
            KeyCode.Q
        );
    }

    public override void ResetCustomButton()
    {
        sheriffKillButton.MaxTimer = cooldown;
    }
}