using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Medium : RoleBase
{
    public float chanceAdditionalInfo;

    public float cooldown = 30f;
    public List<Tuple<DeadPlayer, Vector3>> deadBodies = [];
    public float duration = 3f;
    public List<Tuple<DeadPlayer, Vector3>> futureDeadBodies = [];
    public PlayerControl medium;
    public DateTime meetingStartTime = DateTime.UtcNow;
    public bool oneTimeUse;

    private ResourceSprite question = new("MediumButton.png");
    public List<SpriteRenderer> souls = [];

    private ResourceSprite soulSprite = new("Soul.png", 500f);
    public DeadPlayer soulTarget;
    public DeadPlayer target;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(98, 120, 115, byte.MaxValue),
        GetRole = Get<Medium>,
        CreateRoleController = n => new MediumRoleController(n),
        DescriptionText = "Question the souls",
        IntroInfo = "Question the souls of the dead to gain information",
        Name = nameof(Medium),
        RoleClassType = typeof(Medium),
        RoleId = RoleId.Medium,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class MediumRoleController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Medium>();
    }
    
    
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        medium = null;
        target = null;
        soulTarget = null;
        deadBodies = [];
        futureDeadBodies = [];
        souls = [];
        meetingStartTime = DateTime.UtcNow;
        cooldown = CustomOptionHolder.mediumCooldown;
        duration = CustomOptionHolder.mediumDuration;
        oneTimeUse = CustomOptionHolder.mediumOneTimeUse;
        chanceAdditionalInfo = CustomOptionHolder.mediumChanceAdditionalInfo / 10f;
    }

    public string getInfo(PlayerControl target, PlayerControl killer)
    {
        /*var msg = "";

        var infos = new List<SpecialMediumInfo>();
        // collect fitting death info types.
        // suicides:
        if (killer == target)
        {
            if (target == Sheriff.sheriff || target == Sheriff.formerSheriff)
                infos.Add(SpecialMediumInfo.SheriffSuicide);
            if (target == Lovers.lover1 || target == Lovers.lover2) infos.Add(SpecialMediumInfo.PassiveLoverSuicide);
            if (target == Thief.thief) infos.Add(SpecialMediumInfo.ThiefSuicide);
            if (target == Warlock.warlock) infos.Add(SpecialMediumInfo.WarlockSuicide);
        }
        else
        {
            if (target == Lovers.lover1 || target == Lovers.lover2) infos.Add(SpecialMediumInfo.ActiveLoverDies);
            if (target.Data.Role.IsImpostor && killer.Data.Role.IsImpostor && Thief.formerThief != killer)
                infos.Add(SpecialMediumInfo.ImpostorTeamkill);
        }

        if (target == Sidekick.sidekick &&
            (killer == Jackal.jackal || Jackal.formerJackals.Any(x => x.PlayerId == killer.PlayerId)))
            infos.Add(SpecialMediumInfo.JackalKillsSidekick);
        if (target == Lawyer.lawyer && killer == Lawyer.target) infos.Add(SpecialMediumInfo.LawyerKilledByClient);
        if (this.target.wasCleaned) infos.Add(SpecialMediumInfo.BodyCleaned);

        if (infos.Count > 0)
        {
            var selectedInfo = infos[ListHelper.Random(infos.Count)];
            switch (selectedInfo)
            {
                case SpecialMediumInfo.SheriffSuicide:
                    msg = "Yikes, that Sheriff shot backfired.";
                    break;
                case SpecialMediumInfo.WarlockSuicide:
                    msg = "MAYBE I cursed the person next to me and killed myself. Oops.";
                    break;
                case SpecialMediumInfo.ThiefSuicide:
                    msg = "I tried to steal the gun from their pocket, but they were just happy to see me.";
                    break;
                case SpecialMediumInfo.ActiveLoverDies:
                    msg = "I wanted to get out of this toxic relationship anyways.";
                    break;
                case SpecialMediumInfo.PassiveLoverSuicide:
                    msg = "The love of my life died, thus with a kiss I die.";
                    break;
                case SpecialMediumInfo.LawyerKilledByClient:
                    msg = "My client killed me. Do I still get paid?";
                    break;
                case SpecialMediumInfo.JackalKillsSidekick:
                    msg = "First they sidekicked me, then they killed me. At least I don't need to do tasks anymore.";
                    break;
                case SpecialMediumInfo.ImpostorTeamkill:
                    msg = "I guess they confused me for the Spy, is there even one?";
                    break;
                case SpecialMediumInfo.BodyCleaned:
                    msg = "Is my dead body some kind of art now or... aaand it's gone.";
                    break;
            }
        }
        else
        {
            var randomNumber = ListHelper.Random(4);
            var typeOfColor = Helpers.isLighterColor(this.target.killerIfExisting) ? "lighter" : "darker";
            var timeSinceDeath = (float)(meetingStartTime - this.target.timeOfDeath).TotalMilliseconds;
            var roleString = TheOtherUs.RoleInfo.GetRolesString(this.target.player, false);
            if (randomNumber == 0)
            {
                if (!roleString.Contains("Impostor") && !roleString.Contains("Crewmate"))
                    msg = "If my role hasn't been saved, there's no " + roleString + " in the game anymore.";
                else
                    msg = "I am a " + roleString + " without an other role.";
            }
            else if (randomNumber == 1)
            {
                msg = "I'm not sure, but I guess a " + typeOfColor + " color killed me.";
            }
            else if (randomNumber == 2)
            {
                msg = "If I counted correctly, I died " + Math.Round(timeSinceDeath / 1000) +
                      "s before the next meeting started.";
            }
            else
            {
                msg = "It seems like my killer is the " +
                      TheOtherUs.RoleInfo.GetRolesString(this.target.killerIfExisting, false, false, true) + ".";
            }
        }

        if (ListHelper.rnd.NextDouble() < chanceAdditionalInfo)
        {
            var count = 0;
            var condition = "";
            var alivePlayersList = PlayerControl.AllPlayerControls.ToArray().Where(pc => !pc.Data.IsDead);
            switch (ListHelper.Random(3))
            {
                case 0:
                    count = alivePlayersList.Count(pc => pc.Data.Role.IsImpostor ||
                                                         new List<TheOtherUs.RoleInfo>
                                                             {
                                                                 TheOtherUs.RoleInfo.jackal,
                                                                 TheOtherUs.RoleInfo.sidekick,
                                                                 TheOtherUs.RoleInfo.sheriff,
                                                                 TheOtherUs.RoleInfo.thief
                                                             }
                                                             .Contains(TheOtherUs.RoleInfo
                                                                 .getRoleInfoForPlayer(pc, false).FirstOrDefault()));
                    condition = "killer" + (count == 1 ? "" : "s");
                    break;
                case 1:
                    count = alivePlayersList.Where(Helpers.roleCanUseVents).Count();
                    condition = "player" + (count == 1 ? "" : "s") + " who can use vents";
                    break;
                case 2:
                    count = alivePlayersList
                        .Count(pc => Helpers.isNeutral(pc) && pc != Jackal.jackal && pc != Sidekick.sidekick &&
                                     pc != Thief.thief);
                    condition = "player" + (count == 1 ? "" : "s") + " who " + (count == 1 ? "is" : "are") +
                                " neutral but cannot kill";
                    break;
                case 3:
                    //count = alivePlayersList.Where(pc =>
                    break;
            }

            msg += $"\nWhen you asked, {count} " + condition + (count == 1 ? " was" : " were") + " still alive";
        }

        return this.target.player.Data.PlayerName + "'s Soul:\n" + msg;*/
        return string.Empty;
    }

    private enum SpecialMediumInfo
    {
        SheriffSuicide,
        ThiefSuicide,
        ActiveLoverDies,
        PassiveLoverSuicide,
        LawyerKilledByClient,
        JackalKillsSidekick,
        ImpostorTeamkill,
        SubmergedO2,
        WarlockSuicide,
        BodyCleaned
    }
}