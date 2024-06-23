using TheOtherUs.Objects;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Sheriff : RoleBase
{
    public static readonly RoleInfo roleInfo = new()
    {
        Name = nameof(sheriff),
        Color = new Color32(248, 205, 70, byte.MaxValue),
        DescriptionText = "Shoot the Impostors",
        IntroInfo = "Shoot the <color=#FF1919FF>Impostors</color>",
        RoleId = RoleId.Sheriff,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Sheriff>,
        RoleClassType = typeof(Sheriff),
        CreateRoleController = n => new SheriffController(n)
    };
    
    public class SheriffController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Sheriff>();
    }

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

    public bool spyCanDieToSheriff;

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override CustomRoleOption roleOption { get; set; }
    

    public override void ClearAndReload()
    {
        sheriff = null;
        currentTarget = null;
        formerDeputy = null;
        formerSheriff = null;
        misfireKills = sheriffMisfireKills;
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
        roleOption = new CustomRoleOption(this);
        sheriffCooldown = roleOption.AddChild("Sheriff Cooldown",   new FloatOptionSelection(30f, 10f, 60f, 2.5f));
        sheriffMisfireKills = roleOption.AddChild("Misfire Kills",
            new StringOptionSelection(["Self", "Target", "Both"]));
        sheriffCanKillNeutrals = roleOption.AddChild("Sheriff Can Kill Neutrals", new BoolOptionSelection(false));
        sheriffCanKillJester = sheriffCanKillNeutrals.AddChild(
            "Sheriff Can Kill " + "Jester".ColorString(GetColor<Jester>()), new BoolOptionSelection(false));
        sheriffCanKillProsecutor = sheriffCanKillNeutrals.AddChild(
            "Sheriff Can Kill " + "Prosecutor".ColorString(GetColor<Jester>()), new BoolOptionSelection(false));
        sheriffCanKillAmnesiac = sheriffCanKillNeutrals.AddChild(
            "Sheriff Can Kill " + "Amnesiac".ColorString(GetColor<Amnisiac>()), new BoolOptionSelection(false));
        sheriffCanKillArsonist = sheriffCanKillNeutrals.AddChild(
            "Sheriff Can Kill " + "Arsonist".ColorString(GetColor<Arsonist>()), new BoolOptionSelection(false));
        sheriffCanKillVulture = sheriffCanKillNeutrals.AddChild(
            "Sheriff Can Kill " + "Vulture".ColorString(GetColor<Vulture>()), new BoolOptionSelection(false));
        sheriffCanKillLawyer = sheriffCanKillNeutrals.AddChild(
            "Sheriff Can Kill " + "Lawyer".ColorString(GetColor<Lawyer>()), new BoolOptionSelection(false));
        sheriffCanKillThief = sheriffCanKillNeutrals.AddChild(
            "Sheriff Can Kill " + "Thief".ColorString(GetColor<Thief>()), new BoolOptionSelection(false));
        sheriffCanKillPursuer = sheriffCanKillNeutrals.AddChild(
            "Sheriff Can Kill " + "Pursuer".ColorString(GetColor<Pursuer>()), new BoolOptionSelection(false));
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
                  !CachedPlayer.LocalPlayer.NetPlayerInfo.IsDead,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, sheriffKillButton, "KILL");
                return currentTarget && CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer; },
            _hudManager.KillButton.graphic.sprite,
            DefButtonPositions.upperRowRight,
            _hudManager,
            KeyCode.Q
        );
    }

    public override void ResetCustomButton()
    {
        sheriffKillButton.MaxTimer = cooldown;
    }
}