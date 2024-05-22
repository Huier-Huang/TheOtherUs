using System.Collections.Generic;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs;

public class CustomOptionHolder
{
    public static string[] rates = ["0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"];

    public static string[] ratesModifier =
        ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"];

    public static string[] presets =
    [
        "Preset 1", "Preset 2", "Preset 3", "Random Preset Skeld", "Random Preset Mira HQ", "Random Preset Polus",
        "Random Preset Airship", "Random Preset Fungle", "Random Preset Submerged"
    ];

    public static CustomOption presetSelection;
    public static CustomOption activateRoles;
    public static CustomOption crewmateRolesCountMin;
    public static CustomOption crewmateRolesCountMax;
    public static CustomOption crewmateRolesFill;
    public static CustomOption neutralRolesCountMin;
    public static CustomOption neutralRolesCountMax;
    public static CustomOption impostorRolesCountMin;
    public static CustomOption impostorRolesCountMax;
    public static CustomOption modifiersCountMin;
    public static CustomOption modifiersCountMax;

    public static CustomOption anyPlayerCanStopStart;

    //public static CustomOption enableCodenameHorsemode;
    //public static CustomOption enableCodenameDisableHorses;
    public static CustomOption enableEventMode;

    public static CustomOption cultistSpawnRate;

    public static CustomOption minerSpawnRate;
    public static CustomOption minerCooldown;
    public static CustomOption mafiaSpawnRate;
    public static CustomOption janitorCooldown;

    public static CustomOption morphlingSpawnRate;
    public static CustomOption morphlingCooldown;
    public static CustomOption morphlingDuration;

    public static CustomOption bomber2SpawnRate;
    public static CustomOption bomber2BombCooldown;
    public static CustomOption bomber2Delay;

    public static CustomOption bomber2Timer;
    //public static CustomOption bomber2HotPotatoMode;

    public static CustomOption undertakerSpawnRate;
    public static CustomOption undertakerDragingDelaiAfterKill;
    public static CustomOption undertakerDragingAfterVelocity;
    public static CustomOption undertakerCanDragAndVent;

    public static CustomOption camouflagerSpawnRate;
    public static CustomOption camouflagerCooldown;
    public static CustomOption camouflagerDuration;

    public static CustomOption vampireSpawnRate;
    public static CustomOption vampireKillDelay;
    public static CustomOption vampireCooldown;
    public static CustomOption vampireGarlicButton;
    public static CustomOption vampireCanKillNearGarlics;

    public static CustomOption poucherSpawnRate;
    public static CustomOption mimicSpawnRate;

    public static CustomOption eraserSpawnRate;
    public static CustomOption eraserCooldown;
    public static CustomOption eraserCanEraseAnyone;

    public static CustomOption guesserSpawnRate;
    public static CustomOption guesserIsImpGuesserRate;
    public static CustomOption guesserNumberOfShots;
    public static CustomOption guesserHasMultipleShotsPerMeeting;
    public static CustomOption guesserShowInfoInGhostChat;
    public static CustomOption guesserKillsThroughShield;
    public static CustomOption guesserEvilCanKillSpy;
    public static CustomOption guesserEvilCanKillCrewmate;
    public static CustomOption guesserSpawnBothRate;
    public static CustomOption guesserCantGuessSnitchIfTaksDone;

    public static CustomOption jesterSpawnRate;
    public static CustomOption jesterCanCallEmergency;
    public static CustomOption jesterCanVent;
    public static CustomOption jesterHasImpostorVision;

    public static CustomOption amnisiacSpawnRate;
    public static CustomOption amnisiacShowArrows;
    public static CustomOption amnisiacResetRole;

    public static CustomOption arsonistSpawnRate;
    public static CustomOption arsonistCooldown;
    public static CustomOption arsonistDuration;

    public static CustomOption jackalSpawnRate;
    public static CustomOption jackalKillCooldown;
    public static CustomOption jackalChanceSwoop;
    public static CustomOption swooperCooldown;
    public static CustomOption swooperDuration;
    public static CustomOption jackalCreateSidekickCooldown;
    public static CustomOption jackalImpostorCanFindSidekick;
    public static CustomOption jackalKillFakeImpostor;
    public static CustomOption jackalCanUseVents;
    public static CustomOption jackalCanUseSabo;
    public static CustomOption jackalCanCreateSidekick;
    public static CustomOption sidekickPromotesToJackal;
    public static CustomOption sidekickCanKill;
    public static CustomOption sidekickCanUseVents;
    public static CustomOption jackalPromotedFromSidekickCanCreateSidekick;
    public static CustomOption jackalCanCreateSidekickFromImpostor;
    public static CustomOption jackalAndSidekickHaveImpostorVision;

    public static CustomOption bountyHunterSpawnRate;
    public static CustomOption bountyHunterBountyDuration;
    public static CustomOption bountyHunterReducedCooldown;
    public static CustomOption bountyHunterPunishmentTime;
    public static CustomOption bountyHunterShowArrow;
    public static CustomOption bountyHunterArrowUpdateIntervall;

    public static CustomOption witchSpawnRate;
    public static CustomOption witchCooldown;
    public static CustomOption witchAdditionalCooldown;
    public static CustomOption witchCanSpellAnyone;
    public static CustomOption witchSpellCastingDuration;
    public static CustomOption witchTriggerBothCooldowns;
    public static CustomOption witchVoteSavesTargets;

    public static CustomOption ninjaSpawnRate;
    public static CustomOption ninjaCooldown;
    public static CustomOption ninjaKnowsTargetLocation;
    public static CustomOption ninjaTraceTime;
    public static CustomOption ninjaTraceColorTime;
    public static CustomOption ninjaInvisibleDuration;

    public static CustomOption blackmailerSpawnRate;
    public static CustomOption blackmailerCooldown;

    public static CustomOption mayorSpawnRate;
    public static CustomOption mayorCanSeeVoteColors;
    public static CustomOption mayorTasksNeededToSeeVoteColors;
    public static CustomOption mayorMeetingButton;
    public static CustomOption mayorMaxRemoteMeetings;
    public static CustomOption mayorChooseSingleVote;

    public static CustomOption portalmakerSpawnRate;
    public static CustomOption portalmakerCooldown;
    public static CustomOption portalmakerUsePortalCooldown;
    public static CustomOption portalmakerLogOnlyColorType;
    public static CustomOption portalmakerLogHasTime;
    public static CustomOption portalmakerCanPortalFromAnywhere;

    public static CustomOption engineerSpawnRate;

    public static CustomOption engineerRemoteFix;

    //public static CustomOption engineerExpertRepairs;
    public static CustomOption engineerResetFixAfterMeeting;
    public static CustomOption engineerNumberOfFixes;
    public static CustomOption engineerHighlightForImpostors;
    public static CustomOption engineerHighlightForTeamJackal;

    public static CustomOption privateInvestigatorSpawnRate;
    public static CustomOption privateInvestigatorSeeColor;

    public static CustomOption sheriffSpawnRate;
    public static CustomOption sheriffMisfireKills;
    public static CustomOption sheriffCooldown;
    public static CustomOption sheriffCanKillNeutrals;
    public static CustomOption sheriffCanKillArsonist;
    public static CustomOption sheriffCanKillLawyer;
    public static CustomOption sheriffCanKillProsecutor;
    public static CustomOption sheriffCanKillJester;
    public static CustomOption sheriffCanKillVulture;
    public static CustomOption sheriffCanKillThief;
    public static CustomOption sheriffCanKillAmnesiac;
    public static CustomOption sheriffCanKillPursuer;
    public static CustomOption deputySpawnRate;

    public static CustomOption deputyNumberOfHandcuffs;
    public static CustomOption deputyHandcuffCooldown;
    public static CustomOption deputyGetsPromoted;
    public static CustomOption deputyKeepsHandcuffs;
    public static CustomOption deputyHandcuffDuration;
    public static CustomOption deputyKnowsSheriff;

    public static CustomOption lighterSpawnRate;
    public static CustomOption lighterModeLightsOnVision;
    public static CustomOption lighterModeLightsOffVision;
    public static CustomOption lighterFlashlightWidth;

    public static CustomOption detectiveSpawnRate;
    public static CustomOption detectiveAnonymousFootprints;
    public static CustomOption detectiveFootprintIntervall;
    public static CustomOption detectiveFootprintDuration;
    public static CustomOption detectiveReportNameDuration;
    public static CustomOption detectiveReportColorDuration;

    public static CustomOption timeMasterSpawnRate;
    public static CustomOption timeMasterCooldown;
    public static CustomOption timeMasterRewindTime;
    public static CustomOption timeMasterShieldDuration;

    public static CustomOption veterenSpawnRate;
    public static CustomOption veterenCooldown;
    public static CustomOption veterenAlertDuration;

    public static CustomOption medicSpawnRate;
    public static CustomOption medicShowShielded;
    public static CustomOption medicShowAttemptToShielded;
    public static CustomOption medicSetOrShowShieldAfterMeeting;
    public static CustomOption medicShowAttemptToMedic;
    public static CustomOption medicSetShieldAfterMeeting;
    public static CustomOption medicBreakShield;
    public static CustomOption medicResetTargetAfterMeeting;

    public static CustomOption swapperSpawnRate;
    public static CustomOption swapperCanCallEmergency;
    public static CustomOption swapperCanFixSabotages;
    public static CustomOption swapperCanOnlySwapOthers;
    public static CustomOption swapperSwapsNumber;
    public static CustomOption swapperRechargeTasksNumber;

    public static CustomOption seerSpawnRate;
    public static CustomOption seerMode;
    public static CustomOption seerSoulDuration;
    public static CustomOption seerLimitSoulDuration;

    public static CustomOption hackerSpawnRate;
    public static CustomOption hackerCooldown;
    public static CustomOption hackerHackeringDuration;
    public static CustomOption hackerOnlyColorType;
    public static CustomOption hackerToolsNumber;
    public static CustomOption hackerRechargeTasksNumber;
    public static CustomOption hackerNoMove;

    public static CustomOption trackerSpawnRate;
    public static CustomOption trackerUpdateIntervall;
    public static CustomOption trackerResetTargetAfterMeeting;
    public static CustomOption trackerCanTrackCorpses;
    public static CustomOption trackerCorpsesTrackingCooldown;

    public static CustomOption trackerCorpsesTrackingDuration;

    /*
    public static CustomOption snitchSpawnRate;
    public static CustomOption snitchLeftTasksForReveal;
    public static CustomOption snitchMode;
    public static CustomOption snitchTargets;
    */
    public static CustomOption snitchSpawnRate;
    public static CustomOption snitchLeftTasksForReveal;
    public static CustomOption snitchSeeMeeting;
    public static CustomOption snitchCanSeeRoles;
    public static CustomOption snitchIncludeNeutralTeam;
    public static CustomOption snitchTeamNeutraUseDifferentArrowColor;

    public static CustomOption spySpawnRate;
    public static CustomOption spyCanDieToSheriff;
    public static CustomOption spyImpostorsCanKillAnyone;
    public static CustomOption spyCanEnterVents;
    public static CustomOption spyHasImpostorVision;

    public static CustomOption tricksterSpawnRate;
    public static CustomOption tricksterPlaceBoxCooldown;
    public static CustomOption tricksterLightsOutCooldown;
    public static CustomOption tricksterLightsOutDuration;

    public static CustomOption cleanerSpawnRate;
    public static CustomOption cleanerCooldown;

    public static CustomOption warlockSpawnRate;
    public static CustomOption warlockCooldown;
    public static CustomOption warlockRootTime;

    public static CustomOption securityGuardSpawnRate;
    public static CustomOption securityGuardCooldown;
    public static CustomOption securityGuardTotalScrews;
    public static CustomOption securityGuardCamPrice;
    public static CustomOption securityGuardVentPrice;
    public static CustomOption securityGuardCamDuration;
    public static CustomOption securityGuardCamMaxCharges;
    public static CustomOption securityGuardCamRechargeTasksNumber;
    public static CustomOption securityGuardNoMove;

    public static CustomOption bodyGuardSpawnRate;
    public static CustomOption bodyGuardFlash;
    public static CustomOption bodyGuardResetTargetAfterMeeting;

    public static CustomOption vultureSpawnRate;
    public static CustomOption vultureCooldown;
    public static CustomOption vultureNumberToWin;
    public static CustomOption vultureCanUseVents;
    public static CustomOption vultureShowArrows;

    public static CustomOption mediumSpawnRate;
    public static CustomOption mediumCooldown;
    public static CustomOption mediumDuration;
    public static CustomOption mediumOneTimeUse;
    public static CustomOption mediumChanceAdditionalInfo;

    public static CustomOption lawyerSpawnRate;
    public static CustomOption lawyerTargetKnows;
    public static CustomOption lawyerIsProsecutorChance;
    public static CustomOption lawyerTargetCanBeJester;
    public static CustomOption lawyerVision;
    public static CustomOption lawyerKnowsRole;
    public static CustomOption lawyerCanCallEmergency;
    public static CustomOption pursuerCooldown;
    public static CustomOption pursuerBlanksNumber;

    public static CustomOption jumperSpawnRate;
    public static CustomOption jumperJumpTime;
    public static CustomOption jumperChargesOnPlace;

    public static CustomOption jumperResetPlaceAfterMeeting;
    //public static CustomOption jumperChargesGainOnMeeting;
    //public static CustomOption jumperMaxCharges;

    public static CustomOption escapistSpawnRate;
    public static CustomOption escapistEscapeTime;
    public static CustomOption escapistChargesOnPlace;

    public static CustomOption escapistResetPlaceAfterMeeting;
    //public static CustomOption escapistChargesGainOnMeeting;
    //public static CustomOption escapistMaxCharges;

    public static CustomOption werewolfSpawnRate;
    public static CustomOption werewolfRampageCooldown;
    public static CustomOption werewolfRampageDuration;
    public static CustomOption werewolfKillCooldown;

    public static CustomOption thiefSpawnRate;
    public static CustomOption thiefCooldown;
    public static CustomOption thiefHasImpVision;
    public static CustomOption thiefCanUseVents;
    public static CustomOption thiefCanKillSheriff;
    public static CustomOption thiefCanStealWithGuess;


    public static CustomOption trapperSpawnRate;
    public static CustomOption trapperCooldown;
    public static CustomOption trapperMaxCharges;
    public static CustomOption trapperRechargeTasksNumber;
    public static CustomOption trapperTrapNeededTriggerToReveal;
    public static CustomOption trapperAnonymousMap;
    public static CustomOption trapperInfoType;
    public static CustomOption trapperTrapDuration;

    public static CustomOption bomberSpawnRate;
    public static CustomOption bomberBombDestructionTime;
    public static CustomOption bomberBombDestructionRange;
    public static CustomOption bomberBombHearRange;
    public static CustomOption bomberDefuseDuration;
    public static CustomOption bomberBombCooldown;
    public static CustomOption bomberBombActiveAfter;

    public static CustomOption modifiersAreHidden;

    public static CustomOption modifierAssassin;
    public static CustomOption modifierAssassinQuantity;
    public static CustomOption modifierAssassinNumberOfShots;
    public static CustomOption modifierAssassinMultipleShotsPerMeeting;
    public static CustomOption modifierAssassinKillsThroughShield;
    public static CustomOption modifierAssassinCultist;

    public static CustomOption modifierBait;
    public static CustomOption modifierBaitQuantity;
    public static CustomOption modifierBaitReportDelayMin;
    public static CustomOption modifierBaitReportDelayMax;
    public static CustomOption modifierBaitShowKillFlash;

    public static CustomOption modifierLover;
    public static CustomOption modifierLoverImpLoverRate;
    public static CustomOption modifierLoverBothDie;
    public static CustomOption modifierLoverEnableChat;

    public static CustomOption modifierBloody;
    public static CustomOption modifierBloodyQuantity;
    public static CustomOption modifierBloodyDuration;

    public static CustomOption modifierAntiTeleport;
    public static CustomOption modifierAntiTeleportQuantity;

    public static CustomOption modifierTieBreaker;

    public static CustomOption modifierSunglasses;
    public static CustomOption modifierSunglassesQuantity;
    public static CustomOption modifierSunglassesVision;

    public static CustomOption modifierTorch;
    public static CustomOption modifierTorchQuantity;
    public static CustomOption modifierTorchVision;

    public static CustomOption modifierFlash;
    public static CustomOption modifierFlashQuantity;
    public static CustomOption modifierFlashSpeed;

    public static CustomOption modifierMultitasker;
    public static CustomOption modifierMultitaskerQuantity;

    public static CustomOption modifierDisperser;
    public static CustomOption modifierDisperserCooldown;
    public static CustomOption modifierDisperserNumberOfUses;
    public static CustomOption modifierDisperserDispersesToVent;

    public static CustomOption modifierMini;
    public static CustomOption modifierMiniGrowingUpDuration;
    public static CustomOption modifierMiniGrowingUpInMeeting;

    public static CustomOption modifierIndomitable;

    public static CustomOption modifierBlind;

    public static CustomOption modifierTunneler;

    public static CustomOption modifierWatcher;

    public static CustomOption modifierRadar;

    public static CustomOption modifierSlueth;
    //public static CustomOption modifierSwooper;

    public static CustomOption modifierCursed;

    public static CustomOption modifierVip;
    public static CustomOption modifierVipQuantity;
    public static CustomOption modifierVipShowColor;

    public static CustomOption modifierInvert;
    public static CustomOption modifierInvertQuantity;
    public static CustomOption modifierInvertDuration;

    public static CustomOption modifierChameleon;
    public static CustomOption modifierChameleonQuantity;
    public static CustomOption modifierChameleonHoldDuration;
    public static CustomOption modifierChameleonFadeDuration;
    public static CustomOption modifierChameleonMinVisibility;


    public static CustomOption modifierShifter;

    public static CustomOption ResetButtonCooldown;

    public static CustomOption maxNumberOfMeetings;
    public static CustomOption blockSkippingInEmergencyMeetings;
    public static CustomOption noVoteIsSelfVote;
    public static CustomOption hidePlayerNames;
    public static CustomOption showButtonTarget;
    public static CustomOption blockGameEnd;
    public static CustomOption allowParallelMedBayScans;
    public static CustomOption shieldFirstKill;
    public static CustomOption hideVentAnimOnShadows;
    public static CustomOption disableCamsRound1;
    public static CustomOption hideOutOfSightNametags;
    public static CustomOption impostorSeeRoles;
    public static CustomOption transparentTasks;
    public static CustomOption randomGameStartPosition;
    public static CustomOption randomGameStartToVents;
    public static CustomOption allowModGuess;
    public static CustomOption finishTasksBeforeHauntingOrZoomingOut;
    public static CustomOption camsNightVision;
    public static CustomOption camsNoNightVisionIfImpVision;

    public static CustomOption dynamicMap;
    public static CustomOption dynamicMapEnableSkeld;
    public static CustomOption dynamicMapEnableMira;
    public static CustomOption dynamicMapEnablePolus;
    public static CustomOption dynamicMapEnableAirShip;
    public static CustomOption dynamicMapEnableFungle;
    public static CustomOption dynamicMapEnableSubmerged;
    public static CustomOption dynamicMapSeparateSettings;

    public static CustomOption movePolusVents;
    public static CustomOption swapNavWifi;
    public static CustomOption movePolusVitals;
    public static CustomOption enableBetterPolus;
    public static CustomOption moveColdTemp;

    public static CustomOption disableMedbayWalk;

    public static CustomOption enableCamoComms;

    public static CustomOption restrictDevices;

    //public static CustomOption restrictAdmin;
    public static CustomOption restrictCameras;
    public static CustomOption restrictVents;

    //Guesser Gamemode
    public static CustomOption guesserGamemodeCrewNumber;
    public static CustomOption guesserGamemodeNeutralNumber;
    public static CustomOption guesserGamemodeImpNumber;
    public static CustomOption guesserForceJackalGuesser;
    public static CustomOption guesserGamemodeSidekickIsAlwaysGuesser;
    public static CustomOption guesserForceThiefGuesser;
    public static CustomOption guesserGamemodeHaveModifier;
    public static CustomOption guesserGamemodeNumberOfShots;
    public static CustomOption guesserGamemodeHasMultipleShotsPerMeeting;
    public static CustomOption guesserGamemodeKillsThroughShield;
    public static CustomOption guesserGamemodeEvilCanKillSpy;
    public static CustomOption guesserGamemodeCantGuessSnitchIfTaksDone;

    // Hide N Seek Gamemode
    public static CustomOption hideNSeekHunterCount;
    public static CustomOption hideNSeekKillCooldown;
    public static CustomOption hideNSeekHunterVision;
    public static CustomOption hideNSeekHuntedVision;
    public static CustomOption hideNSeekTimer;
    public static CustomOption hideNSeekCommonTasks;
    public static CustomOption hideNSeekShortTasks;
    public static CustomOption hideNSeekLongTasks;
    public static CustomOption hideNSeekTaskWin;
    public static CustomOption hideNSeekTaskPunish;
    public static CustomOption hideNSeekCanSabotage;
    public static CustomOption hideNSeekMap;
    public static CustomOption hideNSeekHunterWaiting;

    public static CustomOption hunterLightCooldown;
    public static CustomOption hunterLightDuration;
    public static CustomOption hunterLightVision;
    public static CustomOption hunterLightPunish;
    public static CustomOption hunterAdminCooldown;
    public static CustomOption hunterAdminDuration;
    public static CustomOption hunterAdminPunish;
    public static CustomOption hunterArrowCooldown;
    public static CustomOption hunterArrowDuration;
    public static CustomOption hunterArrowPunish;

    public static CustomOption huntedShieldCooldown;
    public static CustomOption huntedShieldDuration;
    public static CustomOption huntedShieldRewindTime;
    public static CustomOption huntedShieldNumber;

    // Prop Hunt Settings
    public static CustomOption propHuntMap;
    public static CustomOption propHuntTimer;
    public static CustomOption propHuntNumberOfHunters;
    public static CustomOption hunterInitialBlackoutTime;
    public static CustomOption hunterMissCooldown;
    public static CustomOption hunterHitCooldown;
    public static CustomOption hunterMaxMissesBeforeDeath;
    public static CustomOption propBecomesHunterWhenFound;
    public static CustomOption propHunterVision;
    public static CustomOption propVision;
    public static CustomOption propHuntRevealCooldown;
    public static CustomOption propHuntRevealDuration;
    public static CustomOption propHuntRevealPunish;
    public static CustomOption propHuntUnstuckCooldown;
    public static CustomOption propHuntUnstuckDuration;
    public static CustomOption propHuntInvisCooldown;
    public static CustomOption propHuntInvisDuration;
    public static CustomOption propHuntSpeedboostCooldown;
    public static CustomOption propHuntSpeedboostDuration;
    public static CustomOption propHuntSpeedboostSpeed;
    public static CustomOption propHuntSpeedboostEnabled;
    public static CustomOption propHuntInvisEnabled;
    public static CustomOption propHuntAdminCooldown;
    public static CustomOption propHuntFindCooldown;
    public static CustomOption propHuntFindDuration;

    internal static Dictionary<byte, byte[]> blockedRolePairings = new();

    public static string cs(Color c, string s)
    {
        return $"<color=#{ToByte(c.r):X2}{ToByte(c.g):X2}{ToByte(c.b):X2}{ToByte(c.a):X2}>{s}</color>";
    }

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }

    public static bool isMapSelectionOption(CustomOption option)
    {
        return option == propHuntMap && option == hideNSeekMap;
    }

    public static void Load()
    {
        CustomOption.vanillaSettings = TheOtherRolesPlugin.Instance.Config.Bind("Preset0", "VanillaOptions", "");

        // Role Options
        presetSelection = new CustomOption(0, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Preset"), presets, null,
            true);
        activateRoles = new CustomOption(1,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Enable Mod Roles And Block Vanilla Roles"), true, null,
            true);

        anyPlayerCanStopStart = new CustomOption(2,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Any Player Can Stop The Start"), false, null, false);

        // Using new id's for the options to not break compatibilty with older versions
        crewmateRolesCountMin = new CustomOption(5,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Crewmate Roles"), 15f, 0f, 15f, 1f, null, true);
        crewmateRolesCountMax = new CustomOption(6,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Crewmate Roles"), 15f, 0f, 15f, 1f);
        crewmateRolesFill = new CustomOption(7,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Fill Crewmate Roles\n(Ignores Min/Max)"), false);
        neutralRolesCountMin = new CustomOption(8,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Neutral Roles"), 15f, 0f, 15f, 1f);
        neutralRolesCountMax = new CustomOption(9,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Neutral Roles"), 15f, 0f, 15f, 1f);
        impostorRolesCountMin = new CustomOption(10,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Impostor Roles"), 15f, 0f, 15f, 1f);
        impostorRolesCountMax = new CustomOption(11,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Impostor Roles"), 15f, 0f, 15f, 1f);
        modifiersCountMin = new CustomOption(12,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Minimum Modifiers"), 15f, 0f, 15f, 1f);
        modifiersCountMax = new CustomOption(13,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Maximum Modifiers"), 15f, 0f, 15f, 1f);

        //-------------------------- Other options 1 - 599 -------------------------- //

        resteButtonCooldown = new CustomOption(20, "Game Start CoolDown", 10f, 2.5f, 30f, 2.5f, null, true);

        maxNumberOfMeetings =
            new CustomOption(21, "Number Of Meetings (excluding Mayor meeting)", 10, 0, 15, 1, null, true);
        blockSkippingInEmergencyMeetings = new CustomOption(22, "Block Skipping In Emergency Meetings", false);
        noVoteIsSelfVote = new CustomOption(23, "No Vote Is Self Vote", false, blockSkippingInEmergencyMeetings);
        shieldFirstKill = new CustomOption(24, "Shield Last Game First Kill", false);
        hidePlayerNames = new CustomOption(25, "Hide Player Names", false);
        hideOutOfSightNametags = new CustomOption(26, "Hide Obstructed Player Names", false);
        hideVentAnimOnShadows = new CustomOption(27, "Hide Vent Animation Out Of Vision", false);
        showButtonTarget = new CustomOption(28, "Show Button Target", true);
        impostorSeeRoles = new CustomOption(29, "Impostors Can See The Roles Of Their Team", false);
        blockGameEnd = new CustomOption(30, "Block Game End If Power Crew Is Alive", false);
        allowModGuess = new CustomOption(31, "Allow Guessing Some Modifiers", false);

        transparentTasks = new CustomOption(32, "Tasks Are Transparent", false, null, true);
        disableMedbayWalk = new CustomOption(33, "Disable MedBay Animations", false);
        allowParallelMedBayScans = new CustomOption(34, "Allow Parallel MedBay Scans", false);
        finishTasksBeforeHauntingOrZoomingOut =
            new CustomOption(35, "Finish Tasks Before Haunting Or Zooming Out", true);

        //Map options
        randomGameStartPosition = new CustomOption(50, "Random Spawn Location", false, null, true);
        randomGameStartToVents = new CustomOption(51, "Random Spawn To Vents", false, randomGameStartPosition);
        enableBetterPolus = new CustomOption(60, "Enable Better Polus", false);
        movePolusVents = new CustomOption(61, "Adjust Vents", false, enableBetterPolus, false);
        movePolusVitals = new CustomOption(62, "Move Vitals To Labs", false, enableBetterPolus, false);
        swapNavWifi = new CustomOption(63, "Swap Reboot And Chart Course", false, enableBetterPolus, false);
        moveColdTemp = new CustomOption(64, "Move Cold Temp To Death Valley", false, enableBetterPolus, false);

        enableCamoComms = new CustomOption(101, "Enable Camouflage Comms", false, null, false);
        restrictDevices = new CustomOption(102, "Restrict Map Information", new[] { "Off", "Per Round", "Per Game" },
            null, false);
        //restrictAdmin = new CustomOption(103, "Restrict Admin Table", 30f, 0f, 600f, 5f, restrictDevices);
        restrictCameras = new CustomOption(104, "Restrict Cameras", 30f, 0f, 600f, 5f, restrictDevices);
        restrictVents = new CustomOption(105, "Restrict Vitals", 30f, 0f, 600f, 5f, restrictDevices);
        disableCamsRound1 = new CustomOption(106, "No Cameras First Round", false, null, false);
        camsNightVision = new CustomOption(107, "Cams Switch To Night Vision If Lights Are Off", false, null, true);
        camsNoNightVisionIfImpVision = new CustomOption(108, "Impostor Vision Ignores Night Vision Cams", false,
            camsNightVision, false);

        dynamicMap = new CustomOption(120, "Play On A Random Map", false, null, true);
        dynamicMapEnableSkeld = new CustomOption(121, "Skeld", rates, dynamicMap, false);
        dynamicMapEnableMira = new CustomOption(122, "Mira", rates, dynamicMap, false);
        dynamicMapEnablePolus = new CustomOption(123, "Polus", rates, dynamicMap, false);
        dynamicMapEnableAirShip = new CustomOption(124, "Airship", rates, dynamicMap, false);
        dynamicMapEnableFungle = new CustomOption(125, "Fungle", rates, dynamicMap, false);
        dynamicMapEnableSubmerged = new CustomOption(126, "Submerged", rates, dynamicMap, false);
        dynamicMapSeparateSettings = new CustomOption(127, "Use Random Map Setting Presets", false, dynamicMap, false);

        //-------------------------- Impostor Options 10000-19999 -------------------------- //

        modifierAssassin = new CustomOption(10000, cs(Color.red, "Assassin"), rates, null, true);
        modifierAssassinQuantity =
            new CustomOption(10001, cs(Color.red, "Assassin Quantity"), ratesModifier, modifierAssassin);
        modifierAssassinNumberOfShots = new CustomOption(10002, "Number Of Shots", 5f, 1f, 15f, 1f, modifierAssassin);
        modifierAssassinMultipleShotsPerMeeting =
            new CustomOption(10003, "Can Shoot Multiple Times Per Meeting", true, modifierAssassin);
        guesserEvilCanKillSpy = new CustomOption(10004, "Can Guess The Spy", true, modifierAssassin);
        guesserEvilCanKillCrewmate = new CustomOption(10005, "Can Guess Crewmate", true, modifierAssassin);
        guesserCantGuessSnitchIfTaksDone =
            new CustomOption(10006, "Can't Guess Snitch When Revealed", true, modifierAssassin);
        modifierAssassinKillsThroughShield =
            new CustomOption(10007, "Guesses Ignore The Medic Shield", false, modifierAssassin);
        modifierAssassinCultist = new CustomOption(10008, "Cultist Follower Gets Ability", false, modifierAssassin);

        mafiaSpawnRate = new CustomOption(10010, cs(Janitor.color, "Mafia"), rates, null, true);
        janitorCooldown = new CustomOption(10011, "Janitor Cooldown", 30f, 10f, 60f, 2.5f, mafiaSpawnRate);

        morphlingSpawnRate = new CustomOption(10020, cs(Morphling.color, "Morphling"), rates, null, true);
        morphlingCooldown = new CustomOption(10021, "Morphling Cooldown", 30f, 10f, 60f, 2.5f, morphlingSpawnRate);
        morphlingDuration = new CustomOption(10022, "Morph Duration", 10f, 1f, 20f, 0.5f, morphlingSpawnRate);

        bomber2SpawnRate = new CustomOption(10030, cs(Bomber2.color, "Bomber [BETA]"), rates, null, true);
        bomber2BombCooldown = new CustomOption(10031, "Bomber2 Cooldown", 30f, 25f, 60f, 2.5f, bomber2SpawnRate);
        bomber2Delay = new CustomOption(10032, "Bomb Delay", 10f, 1f, 20f, 0.5f, bomber2SpawnRate);
        bomber2Timer = new CustomOption(10033, "Bomb Timer", 10f, 5f, 30f, 5f, bomber2SpawnRate);
        //bomber2HotPotatoMode = new CustomOption(10034, "Hot Potato Mode", false, bomber2SpawnRate);

        undertakerSpawnRate = new CustomOption(10040, cs(Undertaker.color, "Undertaker"), rates, null, true);
        undertakerDragingDelaiAfterKill =
            new CustomOption(10041, "Draging Delay After Kill", 0f, 0f, 15, 1f, undertakerSpawnRate);
        undertakerDragingAfterVelocity =
            new CustomOption(10042, "undertaker Drag Speed", 0.75f, 0.5f, 2f, 0.125f, undertakerSpawnRate);
        undertakerCanDragAndVent = new CustomOption(10043, "Can Vent While Dragging", true, undertakerSpawnRate);

        camouflagerSpawnRate = new CustomOption(10050, cs(Camouflager.color, "Camouflager"), rates, null, true);
        camouflagerCooldown =
            new CustomOption(10051, "Camouflager Cooldown", 30f, 10f, 60f, 2.5f, camouflagerSpawnRate);
        camouflagerDuration = new CustomOption(10052, "Camo Duration", 10f, 1f, 20f, 0.5f, camouflagerSpawnRate);

        vampireSpawnRate = new CustomOption(10060, cs(Vampire.color, "Vampire"), rates, null, true);
        vampireKillDelay = new CustomOption(10061, "Vampire Kill Delay", 10f, 1f, 20f, 1f, vampireSpawnRate);
        vampireCooldown = new CustomOption(10062, "Vampire Cooldown", 30f, 10f, 60f, 2.5f, vampireSpawnRate);
        vampireGarlicButton = new CustomOption(10063, "Enable Garlic", true, vampireSpawnRate);
        vampireCanKillNearGarlics = new CustomOption(10064, "Vampire Can Kill Near Garlics", true, vampireGarlicButton);

        eraserSpawnRate = new CustomOption(10070, cs(Eraser.color, "Eraser"), rates, null, true);
        eraserCooldown = new CustomOption(10071, "Eraser Cooldown", 30f, 10f, 120f, 5f, eraserSpawnRate);
        eraserCanEraseAnyone = new CustomOption(10072, "Eraser Can Erase Anyone", false, eraserSpawnRate);

        poucherSpawnRate = new CustomOption(10080, cs(Poucher.color, "Poucher"), rates, null, true);
        mimicSpawnRate = new CustomOption(10081, cs(Mimic.color, "Mimic"), rates, null, true);

        escapistSpawnRate = new CustomOption(10090, cs(Escapist.color, "Escapist"), rates, null, true);
        escapistEscapeTime = new CustomOption(10091, "Mark and Escape Cooldown", 30, 0, 60, 5, escapistSpawnRate);
        escapistChargesOnPlace = new CustomOption(10092, "Charges On Place", 1, 1, 10, 1, escapistSpawnRate);
        //escapistResetPlaceAfterMeeting = new CustomOption(10093, Types.Crewmate, "Reset Places After Meeting", true, jumperSpawnRate);
        //escapistChargesGainOnMeeting = new CustomOption(10094, Types.Crewmate, "Charges Gained After Meeting", 2, 0, 10, 1, jumperSpawnRate);
        //escapistMaxCharges = new CustomOption(10095, "Maximum Charges", 3, 0, 10, 1, escapistSpawnRate);

        cultistSpawnRate = new CustomOption(10100, cs(Cultist.color, "Cultist"), rates, null, true);

        tricksterSpawnRate = new CustomOption(10110, cs(Trickster.color, "Trickster"), rates, null, true);
        tricksterPlaceBoxCooldown =
            new CustomOption(10111, "Trickster Box Cooldown", 10f, 2.5f, 30f, 2.5f, tricksterSpawnRate);
        tricksterLightsOutCooldown =
            new CustomOption(10112, "Trickster Lights Out Cooldown", 30f, 10f, 60f, 5f, tricksterSpawnRate);
        tricksterLightsOutDuration =
            new CustomOption(10113, "Trickster Lights Out Duration", 15f, 5f, 60f, 2.5f, tricksterSpawnRate);

        cleanerSpawnRate = new CustomOption(10120, cs(Cleaner.color, "Cleaner"), rates, null, true);
        cleanerCooldown = new CustomOption(10121, "Cleaner Cooldown", 30f, 10f, 60f, 2.5f, cleanerSpawnRate);

        warlockSpawnRate = new CustomOption(10130, cs(Cleaner.color, "Warlock"), rates, null, true);
        warlockCooldown = new CustomOption(10131, "Warlock Cooldown", 30f, 10f, 60f, 2.5f, warlockSpawnRate);
        warlockRootTime = new CustomOption(10132, "Warlock Root Time", 5f, 0f, 15f, 1f, warlockSpawnRate);

        bountyHunterSpawnRate = new CustomOption(10140, cs(BountyHunter.color, "Bounty Hunter"), rates, null, true);
        bountyHunterBountyDuration = new CustomOption(10141, "Duration After Which Bounty Changes", 60f, 10f, 180f, 10f,
            bountyHunterSpawnRate);
        bountyHunterReducedCooldown = new CustomOption(10142, "Cooldown After Killing Bounty", 2.5f, 0f, 30f, 2.5f,
            bountyHunterSpawnRate);
        bountyHunterPunishmentTime = new CustomOption(10143, "Additional Cooldown After Killing Others", 20f, 0f, 60f,
            2.5f, bountyHunterSpawnRate);
        bountyHunterShowArrow =
            new CustomOption(10144, "Show Arrow Pointing Towards The Bounty", true, bountyHunterSpawnRate);
        bountyHunterArrowUpdateIntervall = new CustomOption(10145, "Arrow Update Intervall", 15f, 2.5f, 60f, 2.5f,
            bountyHunterShowArrow);

        witchSpawnRate = new CustomOption(10150, cs(Witch.color, "Witch"), rates, null, true);
        witchCooldown = new CustomOption(10151, "Witch Spell Casting Cooldown", 30f, 10f, 120f, 5f, witchSpawnRate);
        witchAdditionalCooldown =
            new CustomOption(10152, "Witch Additional Cooldown", 10f, 0f, 60f, 5f, witchSpawnRate);
        witchCanSpellAnyone = new CustomOption(10153, "Witch Can Spell Anyone", false, witchSpawnRate);
        witchSpellCastingDuration = new CustomOption(10154, "Spell Casting Duration", 1f, 0f, 10f, 1f, witchSpawnRate);
        witchTriggerBothCooldowns = new CustomOption(10155, "Trigger Both Cooldowns", true, witchSpawnRate);
        witchVoteSavesTargets = new CustomOption(10156, "Voting The Witch Saves All The Targets", true, witchSpawnRate);

        ninjaSpawnRate = new CustomOption(10160, cs(Ninja.color, "Ninja"), rates, null, true);
        ninjaCooldown = new CustomOption(10161, "Ninja Mark Cooldown", 30f, 10f, 120f, 5f, ninjaSpawnRate);
        ninjaKnowsTargetLocation = new CustomOption(10162, "Ninja Knows Location Of Target", true, ninjaSpawnRate);
        ninjaTraceTime = new CustomOption(10163, "Trace Duration", 5f, 1f, 20f, 0.5f, ninjaSpawnRate);
        ninjaTraceColorTime =
            new CustomOption(10164, "Time Till Trace Color Has Faded", 2f, 0f, 20f, 0.5f, ninjaSpawnRate);
        ninjaInvisibleDuration =
            new CustomOption(10165, "Time The Ninja Is Invisible", 3f, 0f, 20f, 1f, ninjaSpawnRate);

        blackmailerSpawnRate = new CustomOption(10170, cs(Blackmailer.color, "Blackmailer"), rates, null, true);
        blackmailerCooldown = new CustomOption(10171, "Blackmail Cooldown", 30f, 5f, 120f, 5f, blackmailerSpawnRate);

        bomberSpawnRate = new CustomOption(10180, cs(Bomber.color, "Terrorist"), rates, null, true);
        bomberBombDestructionTime =
            new CustomOption(10181, "Bomb Destruction Time", 20f, 2.5f, 120f, 2.5f, bomberSpawnRate);
        bomberBombDestructionRange =
            new CustomOption(10182, "Bomb Destruction Range", 50f, 5f, 150f, 5f, bomberSpawnRate);
        bomberBombHearRange = new CustomOption(10183, "Bomb Hear Range", 60f, 5f, 150f, 5f, bomberSpawnRate);
        bomberDefuseDuration = new CustomOption(10184, "Bomb Defuse Duration", 3f, 0.5f, 30f, 0.5f, bomberSpawnRate);
        bomberBombCooldown = new CustomOption(10185, "Bomb Cooldown", 15f, 2.5f, 30f, 2.5f, bomberSpawnRate);
        bomberBombActiveAfter = new CustomOption(10186, "Bomb Is Active After", 3f, 0.5f, 15f, 0.5f, bomberSpawnRate);

        minerSpawnRate = new CustomOption(10190, cs(Miner.color, "Miner"), rates, null, true);
        minerCooldown = new CustomOption(10191, "Mine Cooldown", 25f, 10f, 60f, 2.5f, minerSpawnRate);


        //-------------------------- Neutral Options 20000-29999 -------------------------- //

        jesterSpawnRate = new CustomOption(20000, cs(Jester.color, "Jester"), rates, null, true);
        jesterCanCallEmergency = new CustomOption(20001, "Jester Can Call Emergency Meeting", true, jesterSpawnRate);
        jesterCanVent = new CustomOption(20002, "Jester Can Hide In Vent", true, jesterSpawnRate);
        jesterHasImpostorVision = new CustomOption(20003, "Jester Has Impostor Vision", false, jesterSpawnRate);

        amnisiacSpawnRate = new CustomOption(20010, cs(Amnisiac.color, "Amnesiac"), rates, null, true);
        amnisiacShowArrows = new CustomOption(20011, "Show Arrows To Dead Bodies", true, amnisiacSpawnRate);
        amnisiacResetRole = new CustomOption(20012, "Reset Role When Taken", true, amnisiacSpawnRate);

        arsonistSpawnRate = new CustomOption(20030, cs(Arsonist.color, "Arsonist"), rates, null, true);
        arsonistCooldown = new CustomOption(20031, "Arsonist Cooldown", 12.5f, 2.5f, 60f, 2.5f, arsonistSpawnRate);
        arsonistDuration = new CustomOption(20032, "Arsonist Douse Duration", 3f, 1f, 10f, 1f, arsonistSpawnRate);

        jackalSpawnRate = new CustomOption(20040, cs(Jackal.color, "Jackal"), rates, null, true);
        jackalKillCooldown =
            new CustomOption(20041, "Jackal/Sidekick Kill Cooldown", 30f, 10f, 60f, 2.5f, jackalSpawnRate);
        jackalChanceSwoop = new CustomOption(20042, "Chance That Jackal Can Swoop", rates, jackalSpawnRate);
        swooperCooldown = new CustomOption(20043, "Swoop Cooldown", 30f, 10f, 60f, 2.5f, jackalChanceSwoop);
        swooperDuration = new CustomOption(20044, "Swoop Duration", 10f, 1f, 20f, 0.5f, jackalChanceSwoop);
        jackalCanUseVents = new CustomOption(20045, "Jackal Can Use Vents", true, jackalSpawnRate);
        jackalCanUseSabo = new CustomOption(20046, "Jackal Team Can Sabotage", false, jackalSpawnRate);
        jackalAndSidekickHaveImpostorVision =
            new CustomOption(20047, "Jackal And Sidekick Have Impostor Vision", false, jackalSpawnRate);
        jackalCanCreateSidekick = new CustomOption(20048, "Jackal Can Create A Sidekick", false, jackalSpawnRate);
        jackalCreateSidekickCooldown = new CustomOption(20049, "Jackal Create Sidekick Cooldown", 30f, 10f, 60f, 2.5f,
            jackalCanCreateSidekick);
        jackalImpostorCanFindSidekick = new CustomOption(20050,
            cs(Palette.ImpostorRed, "Impostors can see teammates turned Sidekick"), true, jackalCanCreateSidekick);
        sidekickCanKill = new CustomOption(20051, "Sidekick Can Kill", false, jackalCanCreateSidekick);
        sidekickCanUseVents = new CustomOption(20052, "Sidekick Can Use Vents", true, jackalCanCreateSidekick);
        sidekickPromotesToJackal = new CustomOption(20053, "Sidekick Gets Promoted To Jackal On Jackal Death", false,
            jackalCanCreateSidekick);
        jackalPromotedFromSidekickCanCreateSidekick = new CustomOption(20054,
            "Jackals Promoted From Sidekick Can Create A Sidekick", true, sidekickPromotesToJackal);
        jackalCanCreateSidekickFromImpostor = new CustomOption(20055, "Jackals Can Make An Impostor To His Sidekick",
            true, jackalCanCreateSidekick);
        jackalKillFakeImpostor =
            new CustomOption(20056, "Jackal Kills A Failed Sidekick Attempt", true, jackalCanCreateSidekick);

        vultureSpawnRate = new CustomOption(20060, cs(Vulture.color, "Vulture"), rates, null, true);
        vultureCooldown = new CustomOption(20061, "Vulture Cooldown", 15f, 10f, 60f, 2.5f, vultureSpawnRate);
        vultureNumberToWin = new CustomOption(20062, "Number Of Corpses Needed To Be Eaten", 4f, 1f, 10f, 1f,
            vultureSpawnRate);
        vultureCanUseVents = new CustomOption(20063, "Vulture Can Use Vents", true, vultureSpawnRate);
        vultureShowArrows = new CustomOption(20064, "Show Arrows Pointing Towards The Corpses", true, vultureSpawnRate);

        lawyerSpawnRate = new CustomOption(20070, cs(Lawyer.color, "Lawyer"), rates, null, true);
        lawyerIsProsecutorChance =
            new CustomOption(20071, "Chance That The Lawyer Is Prosecutor", rates, lawyerSpawnRate);
        lawyerTargetKnows = new CustomOption(20072, "Lawyer Target Knows", true, lawyerSpawnRate);
        lawyerVision = new CustomOption(20073, "Vision", 1f, 0.25f, 3f, 0.25f, lawyerSpawnRate);
        lawyerKnowsRole = new CustomOption(20074, "Lawyer/Prosecutor Knows Target Role", false, lawyerSpawnRate);
        lawyerCanCallEmergency =
            new CustomOption(20075, "Lawyer/Prosecutor Can Call Emergency Meeting", true, lawyerSpawnRate);
        lawyerTargetCanBeJester = new CustomOption(20076, "Lawyer Target Can Be The Jester", false, lawyerSpawnRate);
        pursuerCooldown = new CustomOption(20077, "Pursuer Blank Cooldown", 30f, 5f, 60f, 2.5f, lawyerSpawnRate);
        pursuerBlanksNumber = new CustomOption(20078, "Pursuer Number Of Blanks", 5f, 1f, 20f, 1f, lawyerSpawnRate);

        werewolfSpawnRate = new CustomOption(20080, cs(Werewolf.color, "Werewolf"), rates, null, true);
        werewolfRampageCooldown = new CustomOption(20081, "Rampage Cooldown", 30f, 10f, 60f, 2.5f, werewolfSpawnRate);
        werewolfRampageDuration = new CustomOption(20082, "Rampage Duration", 15f, 1f, 20f, 0.5f, werewolfSpawnRate);
        werewolfKillCooldown = new CustomOption(20083, "Kill Cooldown", 3f, 1f, 60f, 1f, werewolfSpawnRate);

        //-------------------------- Crewmate Options 30000-39999 -------------------------- //

        guesserSpawnRate = new CustomOption(30000, Types.Crewmate, cs(Guesser.color, "Vigilante"), rates, null, true);
        guesserNumberOfShots = new CustomOption(30001, Types.Crewmate, "Vigilante Number Of Shots", 5f, 1f, 15f, 1f,
            guesserSpawnRate);
        guesserHasMultipleShotsPerMeeting = new CustomOption(30002, Types.Crewmate,
            "Vigilante Can Shoot Multiple Times Per Meeting", true, guesserSpawnRate);
        guesserShowInfoInGhostChat =
            new CustomOption(30003, Types.Crewmate, "Guesses Visible In Ghost Chat", true, guesserSpawnRate);
        guesserKillsThroughShield = new CustomOption(30004, Types.Crewmate, "Guesses Ignore The Medic Shield", false,
            guesserSpawnRate);

        mayorSpawnRate = new CustomOption(30010, Types.Crewmate, cs(Mayor.color, "Mayor"), rates, null, true);
        mayorCanSeeVoteColors =
            new CustomOption(30011, Types.Crewmate, "Mayor Can See Vote Colors", false, mayorSpawnRate);
        mayorTasksNeededToSeeVoteColors = new CustomOption(30012, Types.Crewmate,
            "Completed Tasks Needed To See Vote Colors", 5f, 0f, 20f, 1f, mayorCanSeeVoteColors);
        mayorMeetingButton = new CustomOption(30013, Types.Crewmate, "Mobile Emergency Button", true, mayorSpawnRate);
        mayorMaxRemoteMeetings = new CustomOption(30014, Types.Crewmate, "Number Of Remote Meetings", 1f, 1f, 5f, 1f,
            mayorMeetingButton);
        mayorChooseSingleVote = new CustomOption(30015, Types.Crewmate, "Mayor Can Choose Single Vote",
            new[] { "Off", "On (Before Voting)", "On (Until Meeting Ends)" }, mayorSpawnRate);

        engineerSpawnRate = new CustomOption(30020, Types.Crewmate, cs(Engineer.color, "Engineer"), rates, null, true);
        engineerRemoteFix = new CustomOption(30021, Types.Crewmate, "Enable Remote Fix", true, engineerSpawnRate);
        engineerResetFixAfterMeeting =
            new CustomOption(30022, Types.Crewmate, "Reset Fixes After Meeting", false, engineerRemoteFix);
        engineerNumberOfFixes = new CustomOption(30023, Types.Crewmate, "Number Of Sabotage Fixes", 1f, 1f, 3f, 1f,
            engineerRemoteFix);
        //engineerExpertRepairs = new CustomOption(30024, Types.Crewmate, "Advanced Sabotage Repair", false, engineerSpawnRate);
        engineerHighlightForImpostors = new CustomOption(30025, Types.Crewmate, "Impostors See Vents Highlighted", true,
            engineerSpawnRate);
        engineerHighlightForTeamJackal = new CustomOption(30026, Types.Crewmate,
            "Jackal and Sidekick See Vents Highlighted ", true, engineerSpawnRate);

        privateInvestigatorSpawnRate = new CustomOption(30030, Types.Crewmate,
            cs(PrivateInvestigator.color, "Detective"), rates, null, true);
        privateInvestigatorSeeColor = new CustomOption(30031, Types.Crewmate, "Can See Target Player Color", true,
            privateInvestigatorSpawnRate);

        sheriffSpawnRate = new CustomOption(30040, Types.Crewmate, cs(Sheriff.color, "Sheriff"), rates, null, true);
        sheriffCooldown = new CustomOption(30041, Types.Crewmate, "Sheriff Cooldown", 30f, 10f, 60f, 2.5f,
            sheriffSpawnRate);
        sheriffMisfireKills = new CustomOption(30042, Types.Crewmate, "Misfire Kills",
            new[] { "Self", "Target", "Both" }, sheriffSpawnRate);
        sheriffCanKillNeutrals =
            new CustomOption(30043, Types.Crewmate, "Sheriff Can Kill Neutrals", false, sheriffSpawnRate);
        sheriffCanKillJester = new CustomOption(30044, Types.Crewmate, "Sheriff Can Kill " + cs(Jester.color, "Jester"),
            false, sheriffCanKillNeutrals);
        sheriffCanKillProsecutor = new CustomOption(30045, Types.Crewmate,
            "Sheriff Can Kill " + cs(Lawyer.color, "Prosecutor"), false, sheriffCanKillNeutrals);
        sheriffCanKillAmnesiac = new CustomOption(30046, Types.Crewmate,
            "Sheriff Can Kill " + cs(Amnisiac.color, "Amnesiac"), false, sheriffCanKillNeutrals);
        sheriffCanKillArsonist = new CustomOption(30047, Types.Crewmate,
            "Sheriff Can Kill " + cs(Arsonist.color, "Arsonist"), false, sheriffCanKillNeutrals);
        sheriffCanKillVulture = new CustomOption(30048, Types.Crewmate,
            "Sheriff Can Kill " + cs(Vulture.color, "Vulture"), false, sheriffCanKillNeutrals);
        sheriffCanKillLawyer = new CustomOption(30049, Types.Crewmate, "Sheriff Can Kill " + cs(Lawyer.color, "Lawyer"),
            false, sheriffCanKillNeutrals);
        sheriffCanKillThief = new CustomOption(30050, Types.Crewmate, "Sheriff Can Kill " + cs(Thief.color, "Thief"),
            false, sheriffCanKillNeutrals);
        sheriffCanKillPursuer = new CustomOption(30051, Types.Crewmate,
            "Sheriff Can Kill " + cs(Pursuer.color, "Pursuer"), false, sheriffCanKillNeutrals);

        deputySpawnRate = new CustomOption(30060, Types.Crewmate, "Sheriff Has A Deputy", rates, sheriffSpawnRate);
        deputyNumberOfHandcuffs = new CustomOption(30061, Types.Crewmate, "Deputy Number Of Handcuffs", 3f, 1f, 10f, 1f,
            deputySpawnRate);
        deputyHandcuffCooldown = new CustomOption(30062, Types.Crewmate, "Handcuff Cooldown", 30f, 10f, 60f, 2.5f,
            deputySpawnRate);
        deputyHandcuffDuration =
            new CustomOption(30063, Types.Crewmate, "Handcuff Duration", 15f, 5f, 60f, 2.5f, deputySpawnRate);
        deputyKnowsSheriff = new CustomOption(30064, Types.Crewmate, "Sheriff And Deputy Know Each Other ", true,
            deputySpawnRate);
        deputyGetsPromoted = new CustomOption(30065, Types.Crewmate, "Deputy Gets Promoted To Sheriff",
            new[] { "Off", "On (Immediately)", "On (After Meeting)" }, deputySpawnRate);
        deputyKeepsHandcuffs = new CustomOption(30066, Types.Crewmate, "Deputy Keeps Handcuffs When Promoted", true,
            deputyGetsPromoted);

        lighterSpawnRate = new CustomOption(30070, Types.Crewmate, cs(Lighter.color, "Lighter"), rates, null, true);
        lighterModeLightsOnVision = new CustomOption(30071, Types.Crewmate, "Vision On Lights On", 1.5f, 0.25f, 5f,
            0.25f, lighterSpawnRate);
        lighterModeLightsOffVision = new CustomOption(30072, Types.Crewmate, "Vision On Lights Off", 0.5f, 0.25f, 5f,
            0.25f, lighterSpawnRate);
        lighterFlashlightWidth = new CustomOption(30073, Types.Crewmate, "Flashlight Width", 0.3f, 0.1f, 1f, 0.1f,
            lighterSpawnRate);

        detectiveSpawnRate =
            new CustomOption(30080, Types.Crewmate, cs(Detective.color, "Investigator"), rates, null, true);
        detectiveAnonymousFootprints =
            new CustomOption(30081, Types.Crewmate, "Anonymous Footprints", false, detectiveSpawnRate);
        detectiveFootprintIntervall = new CustomOption(30082, Types.Crewmate, "Footprint Intervall", 0.5f, 0.25f, 10f,
            0.25f, detectiveSpawnRate);
        detectiveFootprintDuration = new CustomOption(30083, Types.Crewmate, "Footprint Duration", 5f, 0.25f, 10f,
            0.25f, detectiveSpawnRate);
        detectiveReportNameDuration = new CustomOption(30084, Types.Crewmate,
            "Time Where Investigator Reports Will Have Name", 0, 0, 60, 2.5f, detectiveSpawnRate);
        detectiveReportColorDuration = new CustomOption(30085, Types.Crewmate,
            "Time Where Investigator Reports Will Have Color Type", 20, 0, 120, 2.5f, detectiveSpawnRate);

        timeMasterSpawnRate =
            new CustomOption(30090, Types.Crewmate, cs(TimeMaster.color, "Time Master"), rates, null, true);
        timeMasterCooldown = new CustomOption(30091, Types.Crewmate, "Time Master Cooldown", 30f, 10f, 120f, 2.5f,
            timeMasterSpawnRate);
        timeMasterRewindTime =
            new CustomOption(30092, Types.Crewmate, "Rewind Time", 3f, 1f, 10f, 1f, timeMasterSpawnRate);
        timeMasterShieldDuration = new CustomOption(30093, Types.Crewmate, "Time Master Shield Duration", 3f, 1f, 20f,
            1f, timeMasterSpawnRate);

        veterenSpawnRate = new CustomOption(30100, Types.Crewmate, cs(Veteren.color, "Veteran"), rates, null, true);
        veterenCooldown =
            new CustomOption(30101, Types.Crewmate, "Alert Cooldown", 30f, 10f, 120f, 2.5f, veterenSpawnRate);
        veterenAlertDuration =
            new CustomOption(30102, Types.Crewmate, "Alert Duration", 3f, 1f, 20f, 1f, veterenSpawnRate);

        medicSpawnRate = new CustomOption(30110, Types.Crewmate, cs(Medic.color, "Medic"), rates, null, true);
        medicShowShielded = new CustomOption(30111, Types.Crewmate, "Show Shielded Player",
            new[] { "Everyone", "Shielded + Medic", "Medic" }, medicSpawnRate);
        medicBreakShield = new CustomOption(30112, Types.Crewmate, "Shield Is Unbreakable", true, medicSpawnRate);
        medicShowAttemptToShielded = new CustomOption(30113, Types.Crewmate, "Shielded Player Sees Murder Attempt",
            false, medicBreakShield);
        medicResetTargetAfterMeeting =
            new CustomOption(30114, Types.Crewmate, "Reset Target After Meeting", false, medicSpawnRate);
        medicSetOrShowShieldAfterMeeting = new CustomOption(30115, Types.Crewmate, "Shield Will Be Activated",
            new[] { "Instantly", "Instantly, Visible\nAfter Meeting", "After Meeting" }, medicSpawnRate);
        medicShowAttemptToMedic = new CustomOption(30116, Types.Crewmate,
            "Medic Sees Murder Attempt On Shielded Player", false, medicBreakShield);

        swapperSpawnRate = new CustomOption(30120, Types.Crewmate, cs(Swapper.color, "Swapper"), rates, null, true);
        swapperCanCallEmergency = new CustomOption(30121, Types.Crewmate, "Swapper Can Call Emergency Meeting", false,
            swapperSpawnRate);
        swapperCanFixSabotages =
            new CustomOption(30122, Types.Crewmate, "Swapper Can Fix Sabotages", false, swapperSpawnRate);
        swapperCanOnlySwapOthers =
            new CustomOption(30123, Types.Crewmate, "Swapper Can Only Swap Others", false, swapperSpawnRate);
        swapperSwapsNumber =
            new CustomOption(30124, Types.Crewmate, "Initial Swap Charges", 1f, 0f, 5f, 1f, swapperSpawnRate);
        swapperRechargeTasksNumber = new CustomOption(30125, Types.Crewmate, "Number Of Tasks Needed For Recharging",
            2f, 1f, 10f, 1f, swapperSpawnRate);

        seerSpawnRate = new CustomOption(30140, Types.Crewmate, cs(Seer.color, "Seer"), rates, null, true);
        seerMode = new CustomOption(30141, Types.Crewmate, "Seer Mode",
            new[] { "Show Death Flash + Souls", "Show Death Flash", "Show Souls" }, seerSpawnRate);
        seerLimitSoulDuration =
            new CustomOption(30142, Types.Crewmate, "Seer Limit Soul Duration", false, seerSpawnRate);
        seerSoulDuration = new CustomOption(30143, Types.Crewmate, "Seer Soul Duration", 15f, 0f, 120f, 5f,
            seerLimitSoulDuration);

        hackerSpawnRate = new CustomOption(30150, Types.Crewmate, cs(Hacker.color, "Hacker"), rates, null, true);
        hackerCooldown = new CustomOption(30151, Types.Crewmate, "Hacker Cooldown", 30f, 5f, 60f, 5f, hackerSpawnRate);
        hackerHackeringDuration =
            new CustomOption(30152, Types.Crewmate, "Hacker Duration", 10f, 2.5f, 60f, 2.5f, hackerSpawnRate);
        hackerOnlyColorType =
            new CustomOption(30153, Types.Crewmate, "Hacker Only Sees Color Type", false, hackerSpawnRate);
        hackerToolsNumber = new CustomOption(30154, Types.Crewmate, "Max Mobile Gadget Charges", 5f, 1f, 30f, 1f,
            hackerSpawnRate);
        hackerRechargeTasksNumber = new CustomOption(30155, Types.Crewmate, "Number Of Tasks Needed For Recharging", 2f,
            1f, 5f, 1f, hackerSpawnRate);
        hackerNoMove = new CustomOption(30156, Types.Crewmate, "Cant Move During Mobile Gadget Duration", true,
            hackerSpawnRate);

        trackerSpawnRate = new CustomOption(30160, Types.Crewmate, cs(Tracker.color, "Tracker"), rates, null, true);
        trackerUpdateIntervall = new CustomOption(30161, Types.Crewmate, "Tracker Update Intervall", 5f, 1f, 30f, 1f,
            trackerSpawnRate);
        trackerResetTargetAfterMeeting = new CustomOption(30162, Types.Crewmate, "Tracker Reset Target After Meeting",
            false, trackerSpawnRate);
        trackerCanTrackCorpses =
            new CustomOption(30163, Types.Crewmate, "Tracker Can Track Corpses", true, trackerSpawnRate);
        trackerCorpsesTrackingCooldown = new CustomOption(30164, Types.Crewmate, "Corpses Tracking Cooldown", 30f, 5f,
            120f, 5f, trackerCanTrackCorpses);
        trackerCorpsesTrackingDuration = new CustomOption(30165, Types.Crewmate, "Corpses Tracking Duration", 5f, 2.5f,
            30f, 2.5f, trackerCanTrackCorpses);
        /*
        snitchSpawnRate = new CustomOption(30170, Types.Crewmate, cs(Snitch.color, "Snitch"), rates, null, true);
        snitchLeftTasksForReveal = new CustomOption(30171, Types.Crewmate, "Task Count Where The Snitch Will Be Revealed", 5f, 0f, 25f, 1f, snitchSpawnRate);
        snitchMode = new CustomOption(30172, Types.Crewmate, "Information Mode", new string[] { "Chat", "Map", "Chat & Map" }, snitchSpawnRate);
        snitchTargets = new CustomOption(30173, Types.Crewmate, "Targets", new string[] { "All Evil Players", "Killing Players" }, snitchSpawnRate);
        */

        snitchSpawnRate = new CustomOption(30170, Types.Crewmate, cs(Snitch.color, "Snitch"), rates, null, true);
        snitchLeftTasksForReveal = new CustomOption(30171, Types.Crewmate,
            "Task Count Where The Snitch Will Be Revealed", 1f, 0f, 10f, 1f, snitchSpawnRate);
        snitchSeeMeeting = new CustomOption(30172, Types.Crewmate, "Show Roles In Meeting", false, snitchSpawnRate);
        snitchCanSeeRoles = new CustomOption(30173, Types.Crewmate, "Can See Roles", false, snitchSpawnRate);
        snitchIncludeNeutralTeam = new CustomOption(30174, Types.Crewmate, "Include Team Neutral",
            ["Off", "Killer", "Evil", "All"], snitchSpawnRate);
        snitchTeamNeutraUseDifferentArrowColor = new CustomOption(30175, Types.Crewmate,
            "Use Different Color For Neutra Team", true, snitchIncludeNeutralTeam);

        spySpawnRate = new CustomOption(30180, Types.Crewmate, cs(Spy.color, "Spy"), rates, null, true);
        spyCanDieToSheriff = new CustomOption(30181, Types.Crewmate, "Spy Can Die To Sheriff", false, spySpawnRate);
        spyImpostorsCanKillAnyone = new CustomOption(30182, Types.Crewmate,
            "Impostors Can Kill Anyone If There Is A Spy", true, spySpawnRate);
        spyCanEnterVents = new CustomOption(30183, Types.Crewmate, "Spy Can Enter Vents", false, spySpawnRate);
        spyHasImpostorVision = new CustomOption(30184, Types.Crewmate, "Spy Has Impostor Vision", false, spySpawnRate);

        portalmakerSpawnRate =
            new CustomOption(30190, Types.Crewmate, cs(Portalmaker.color, "Portalmaker"), rates, null, true);
        portalmakerCooldown = new CustomOption(30191, Types.Crewmate, "Portalmaker Cooldown", 30f, 10f, 60f, 2.5f,
            portalmakerSpawnRate);
        portalmakerUsePortalCooldown = new CustomOption(30192, Types.Crewmate, "Use Portal Cooldown", 30f, 10f, 60f,
            2.5f, portalmakerSpawnRate);
        portalmakerLogOnlyColorType = new CustomOption(30193, Types.Crewmate, "Portalmaker Log Only Shows Color Type",
            true, portalmakerSpawnRate);
        portalmakerLogHasTime = new CustomOption(30194, Types.Crewmate, "Log Shows Time", true, portalmakerSpawnRate);
        portalmakerCanPortalFromAnywhere = new CustomOption(30195, Types.Crewmate, "Can Port To Portal From Everywhere",
            true, portalmakerSpawnRate);

        securityGuardSpawnRate = new CustomOption(30200, Types.Crewmate, cs(SecurityGuard.color, "Security Guard"),
            rates, null, true);
        securityGuardCooldown = new CustomOption(30201, Types.Crewmate, "Security Guard Cooldown", 30f, 10f, 60f, 2.5f,
            securityGuardSpawnRate);
        securityGuardTotalScrews = new CustomOption(30202, Types.Crewmate, "Security Guard Number Of Screws", 7f, 1f,
            15f, 1f, securityGuardSpawnRate);
        securityGuardCamPrice = new CustomOption(30203, Types.Crewmate, "Number Of Screws Per Cam", 2f, 1f, 15f, 1f,
            securityGuardSpawnRate);
        securityGuardVentPrice = new CustomOption(30204, Types.Crewmate, "Number Of Screws Per Vent", 1f, 1f, 15f, 1f,
            securityGuardSpawnRate);
        securityGuardCamDuration = new CustomOption(30205, Types.Crewmate, "Security Guard Duration", 10f, 2.5f, 60f,
            2.5f, securityGuardSpawnRate);
        securityGuardCamMaxCharges = new CustomOption(30206, Types.Crewmate, "Gadget Max Charges", 5f, 1f, 30f, 1f,
            securityGuardSpawnRate);
        securityGuardCamRechargeTasksNumber = new CustomOption(30207, Types.Crewmate,
            "Number Of Tasks Needed For Recharging", 3f, 1f, 10f, 1f, securityGuardSpawnRate);
        securityGuardNoMove = new CustomOption(30208, Types.Crewmate, "Cant Move During Cam Duration", true,
            securityGuardSpawnRate);

        mediumSpawnRate = new CustomOption(30210, Types.Crewmate, cs(Medium.color, "Medium"), rates, null, true);
        mediumCooldown = new CustomOption(30211, Types.Crewmate, "Medium Questioning Cooldown", 30f, 5f, 120f, 5f,
            mediumSpawnRate);
        mediumDuration = new CustomOption(30212, Types.Crewmate, "Medium Questioning Duration", 3f, 0f, 15f, 1f,
            mediumSpawnRate);
        mediumOneTimeUse = new CustomOption(30213, Types.Crewmate, "Each Soul Can Only Be Questioned Once", false,
            mediumSpawnRate);
        mediumChanceAdditionalInfo = new CustomOption(30214, Types.Crewmate,
            "Chance That The Answer Contains \n    Additional Information", rates, mediumSpawnRate);

        jumperSpawnRate = new CustomOption(30220, Types.Crewmate, cs(Jumper.color, "Jumper"), rates, null, true);
        jumperJumpTime = new CustomOption(30221, Types.Crewmate, "Jump Cooldown", 30, 0, 60, 5, jumperSpawnRate);
        jumperChargesOnPlace =
            new CustomOption(30222, Types.Crewmate, "Charges On Place", 1, 1, 10, 1, jumperSpawnRate);

        bodyGuardSpawnRate =
            new CustomOption(30230, Types.Crewmate, cs(BodyGuard.color, "Bodyguard"), rates, null, true);
        bodyGuardResetTargetAfterMeeting = new CustomOption(30231, Types.Crewmate, "Reset Target After Meeting", true,
            bodyGuardSpawnRate);
        bodyGuardFlash = new CustomOption(30232, Types.Crewmate, "Show Flash On Death", true, bodyGuardSpawnRate);

        thiefSpawnRate = new CustomOption(30240, cs(Thief.color, "Thief"), rates, null, true);
        thiefCooldown = new CustomOption(30241, "Thief Cooldown", 30f, 5f, 120f, 5f, thiefSpawnRate);
        thiefCanKillSheriff = new CustomOption(30242, "Thief Can Kill Sheriff", true, thiefSpawnRate);
        thiefHasImpVision = new CustomOption(30243, "Thief Has Impostor Vision", true, thiefSpawnRate);
        thiefCanUseVents = new CustomOption(30244, "Thief Can Use Vents", true, thiefSpawnRate);
        thiefCanStealWithGuess =
            new CustomOption(30245, "Thief Can Guess To Steal A Role (If Guesser)", false, thiefSpawnRate);

        trapperSpawnRate = new CustomOption(30250, Types.Crewmate, cs(Trapper.color, "Trapper"), rates, null, true);
        trapperCooldown =
            new CustomOption(30251, Types.Crewmate, "Trapper Cooldown", 30f, 5f, 120f, 5f, trapperSpawnRate);
        trapperMaxCharges =
            new CustomOption(30252, Types.Crewmate, "Max Traps Charges", 5f, 1f, 15f, 1f, trapperSpawnRate);
        trapperRechargeTasksNumber = new CustomOption(30253, Types.Crewmate, "Number Of Tasks Needed For Recharging",
            2f, 1f, 15f, 1f, trapperSpawnRate);
        trapperTrapNeededTriggerToReveal = new CustomOption(30254, Types.Crewmate, "Trap Needed Trigger To Reveal", 3f,
            2f, 10f, 1f, trapperSpawnRate);
        trapperAnonymousMap = new CustomOption(30255, Types.Crewmate, "Show Anonymous Map", false, trapperSpawnRate);
        trapperInfoType = new CustomOption(30256, Types.Crewmate, "Trap Information Type",
            new[] { "Role", "Good/Evil Role", "Name" }, trapperSpawnRate);
        trapperTrapDuration =
            new CustomOption(30257, Types.Crewmate, "Trap Duration", 5f, 1f, 15f, 1f, trapperSpawnRate);

        //-------------------------- Modifier (1000 - 1999) -------------------------- //

        modifiersAreHidden = new CustomOption(1000, cs(Color.yellow, "Hide After Death Modifiers"), true, null, true);

        modifierDisperser = new CustomOption(1010, cs(Color.red, "Disperser"), rates, null, true);
        modifierDisperserCooldown =
            new CustomOption(1011, "Disperser Cooldown", 30f, 10f, 60f, 2.5f, modifierDisperser);
        modifierDisperserNumberOfUses = new CustomOption(1012, "Number Of Uses", 1, 1, 5, 1, modifierDisperser);
        modifierDisperserDispersesToVent = new CustomOption(1013, "Disperser To Vent", true, modifierDisperser);

        modifierBloody = new CustomOption(1020, cs(Color.yellow, "Bloody"), rates, null, true);
        modifierBloodyQuantity =
            new CustomOption(1021, cs(Color.yellow, "Bloody Quantity"), ratesModifier, modifierBloody);
        modifierBloodyDuration = new CustomOption(1022, "Trail Duration", 10f, 3f, 60f, 1f, modifierBloody);

        modifierAntiTeleport = new CustomOption(1030, cs(Color.yellow, "Anti Teleport"), rates, null, true);
        modifierAntiTeleportQuantity = new CustomOption(1031, cs(Color.yellow, "Anti Teleport Quantity"), ratesModifier,
            modifierAntiTeleport);

        modifierTieBreaker = new CustomOption(1040, cs(Color.yellow, "Tie Breaker"), rates, null, true);

        modifierBait = new CustomOption(1050, cs(Color.yellow, "Bait"), rates, null, true);
        modifierBaitQuantity = new CustomOption(1051, cs(Color.yellow, "Bait Quantity"), ratesModifier, modifierBait);
        modifierBaitReportDelayMin = new CustomOption(1052, "Bait Report Delay Min", 0f, 0f, 10f, 1f, modifierBait);
        modifierBaitReportDelayMax = new CustomOption(1053, "Bait Report Delay Max", 0f, 0f, 10f, 1f, modifierBait);
        modifierBaitShowKillFlash = new CustomOption(1054, "Warn The Killer With A Flash", true, modifierBait);

        modifierLover = new CustomOption(1060, cs(Color.yellow, "Lovers"), rates, null, true);
        modifierLoverImpLoverRate = new CustomOption(1061, "Chance That One Lover Is Impostor", rates, modifierLover);
        modifierLoverBothDie = new CustomOption(1062, "Both Lovers Die", true, modifierLover);
        modifierLoverEnableChat = new CustomOption(1063, "Enable Lover Chat", true, modifierLover);

        modifierSunglasses = new CustomOption(1070, cs(Color.yellow, "Sunglasses"), rates, null, true);
        modifierSunglassesQuantity = new CustomOption(1071, cs(Color.yellow, "Sunglasses Quantity"), ratesModifier,
            modifierSunglasses);
        modifierSunglassesVision = new CustomOption(1072, "Vision With Sunglasses",
            new[] { "-10%", "-20%", "-30%", "-40%", "-50%" }, modifierSunglasses);

        modifierTorch = new CustomOption(1080, cs(Color.yellow, "Torch"), rates, null, true);
        modifierTorchQuantity =
            new CustomOption(1081, cs(Color.yellow, "Torch Quantity"), ratesModifier, modifierTorch);
        modifierTorchVision = new CustomOption(1082, cs(Color.yellow, "Vision With Torch"), 1.5f, 1f, 3f, 0.125f,
            modifierTorch);

        modifierFlash = new CustomOption(1090, cs(Color.yellow, "Flash"), rates, null, true);
        modifierFlashQuantity = new CustomOption(110, cs(Color.yellow, "Flash Quantity"), ratesModifier, modifierFlash);
        modifierFlashSpeed = new CustomOption(1212, "Flash Speed", 1.25f, 1f, 3f, 0.125f, modifierFlash);

        modifierMultitasker = new CustomOption(1100, cs(Color.yellow, "Multitasker"), rates, null, true);
        modifierMultitaskerQuantity = new CustomOption(1101, cs(Color.yellow, "Multitasker Quantity"), ratesModifier,
            modifierMultitasker);

        modifierMini = new CustomOption(1110, cs(Color.yellow, "Mini"), rates, null, true);
        modifierMiniGrowingUpDuration =
            new CustomOption(1111, "Mini Growing Up Duration", 400f, 100f, 1500f, 100f, modifierMini);
        modifierMiniGrowingUpInMeeting = new CustomOption(1112, "Mini Grows Up In Meeting", true, modifierMini);

        modifierIndomitable = new CustomOption(1120, cs(Color.yellow, "Indomitable"), rates, null, true);

        modifierBlind = new CustomOption(1130, cs(Color.yellow, "Blind"), rates, null, true);

        modifierWatcher = new CustomOption(1140, cs(Color.yellow, "Watcher"), rates, null, true);

        modifierRadar = new CustomOption(1150, cs(Color.yellow, "Radar"), rates, null, true);

        modifierTunneler = new CustomOption(1160, cs(Color.yellow, "Tunneler"), rates, null, true);

        modifierSlueth = new CustomOption(1170, cs(Color.yellow, "Sleuth"), rates, null, true);

        modifierCursed = new CustomOption(1180, cs(Color.yellow, "Fanatic"), rates, null, true);

        modifierVip = new CustomOption(1190, cs(Color.yellow, "VIP"), rates, null, true);
        modifierVipQuantity = new CustomOption(1191, cs(Color.yellow, "VIP Quantity"), ratesModifier, modifierVip);
        modifierVipShowColor = new CustomOption(1192, "Show Team Color", true, modifierVip);

        modifierInvert = new CustomOption(1200, cs(Color.yellow, "Invert"), rates, null, true);
        modifierInvertQuantity =
            new CustomOption(1201, cs(Color.yellow, "Modifier Quantity"), ratesModifier, modifierInvert);
        modifierInvertDuration = new CustomOption(1202, "Number Of Meetings Inverted", 3f, 1f, 15f, 1f, modifierInvert);

        modifierChameleon = new CustomOption(1210, cs(Color.yellow, "Chameleon"), rates, null, true);
        modifierChameleonQuantity =
            new CustomOption(1211, cs(Color.yellow, "Chameleon Quantity"), ratesModifier, modifierChameleon);
        modifierChameleonHoldDuration =
            new CustomOption(1212, "Time Until Fading Starts", 3f, 1f, 10f, 0.5f, modifierChameleon);
        modifierChameleonFadeDuration =
            new CustomOption(1213, "Fade Duration", 1f, 0.25f, 10f, 0.25f, modifierChameleon);
        modifierChameleonMinVisibility = new CustomOption(1214, "Minimum Visibility",
            new[] { "0%", "10%", "20%", "30%", "40%", "50%" }, modifierChameleon);

        modifierShifter = new CustomOption(1220, cs(Color.yellow, "Shifter"), rates, null, true);

        // Guesser Gamemode (2000 - 2999)
        guesserGamemodeCrewNumber = new CustomOption(2001, Types.Guesser, cs(Guesser.color, "Number of Crew Guessers"),
            15f, 1f, 15f, 1f, null, true);
        guesserGamemodeNeutralNumber = new CustomOption(2002, Types.Guesser,
            cs(Guesser.color, "Number of Neutral Guessers"), 15f, 1f, 15f, 1f, null, true);
        guesserGamemodeImpNumber = new CustomOption(2003, Types.Guesser,
            cs(Guesser.color, "Number of Impostor Guessers"), 15f, 1f, 15f, 1f, null, true);
        guesserForceJackalGuesser = new CustomOption(2007, Types.Guesser, "Force Jackal Guesser", false, null, true);
        guesserGamemodeSidekickIsAlwaysGuesser =
            new CustomOption(2012, Types.Guesser, "Sidekick Is Always Guesser", false, null);
        guesserForceThiefGuesser = new CustomOption(2011, Types.Guesser, "Force Thief Guesser", false, null, true);
        guesserGamemodeHaveModifier = new CustomOption(2004, Types.Guesser, "Guessers Can Have A Modifier", true, null);
        guesserGamemodeNumberOfShots =
            new CustomOption(2005, Types.Guesser, "Guesser Number Of Shots", 3f, 1f, 15f, 1f, null);
        guesserGamemodeHasMultipleShotsPerMeeting = new CustomOption(2006, Types.Guesser,
            "Guesser Can Shoot Multiple Times Per Meeting", false, null);
        guesserGamemodeKillsThroughShield =
            new CustomOption(2008, Types.Guesser, "Guesses Ignore The Medic Shield", true, null);
        guesserGamemodeEvilCanKillSpy =
            new CustomOption(2009, Types.Guesser, "Evil Guesser Can Guess The Spy", true, null);
        guesserGamemodeCantGuessSnitchIfTaksDone = new CustomOption(2010, Types.Guesser,
            "Guesser Can't Guess Snitch When Tasks Completed", true, null);

        //-------------------------- Hide N Seek 3000 - 3999 -------------------------- //

        hideNSeekMap = new CustomOption(3020, Types.HideNSeekMain, cs(Color.yellow, "Map"),
            new[] { "The Skeld", "Mira", "Polus", "Airship", "Fungle", "Submerged", "LI Map" }, null, true,
            onChange: () =>
            {
                int map = hideNSeekMap.OptionSelection;
                if (map >= 3) map++;
                GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)map;
            });
        hideNSeekHunterCount = new CustomOption(3000, Types.HideNSeekMain, cs(Color.yellow, "Number Of Hunters"), 1f,
            1f, 3f, 1f);
        hideNSeekKillCooldown = new CustomOption(3021, Types.HideNSeekMain, cs(Color.yellow, "Kill Cooldown"), 10f,
            2.5f, 60f, 2.5f);
        hideNSeekHunterVision = new CustomOption(3001, Types.HideNSeekMain, cs(Color.yellow, "Hunter Vision"), 0.5f,
            0.25f, 2f, 0.25f);
        hideNSeekHuntedVision = new CustomOption(3002, Types.HideNSeekMain, cs(Color.yellow, "Hunted Vision"), 2f,
            0.25f, 5f, 0.25f);
        hideNSeekCommonTasks =
            new CustomOption(3023, Types.HideNSeekMain, cs(Color.yellow, "Common Tasks"), 1f, 0f, 4f, 1f);
        hideNSeekShortTasks =
            new CustomOption(3024, Types.HideNSeekMain, cs(Color.yellow, "Short Tasks"), 3f, 1f, 23f, 1f);
        hideNSeekLongTasks =
            new CustomOption(3025, Types.HideNSeekMain, cs(Color.yellow, "Long Tasks"), 3f, 0f, 15f, 1f);
        hideNSeekTimer =
            new CustomOption(3003, Types.HideNSeekMain, cs(Color.yellow, "Timer In Min"), 5f, 1f, 30f, 0.5f);
        hideNSeekTaskWin = new CustomOption(3004, Types.HideNSeekMain, cs(Color.yellow, "Task Win Is Possible"), false);
        hideNSeekTaskPunish = new CustomOption(3017, Types.HideNSeekMain,
            cs(Color.yellow, "Finish Tasks Punish In Sec"), 10f, 0f, 30f, 1f);
        hideNSeekCanSabotage = new CustomOption(3019, Types.HideNSeekMain, cs(Color.yellow, "Enable Sabotages"), false);
        hideNSeekHunterWaiting = new CustomOption(3022, Types.HideNSeekMain,
            cs(Color.yellow, "Time The Hunter Needs To Wait"), 15f, 2.5f, 60f, 2.5f);

        hunterLightCooldown = new CustomOption(3005, Types.HideNSeekRoles, cs(Color.red, "Hunter Light Cooldown"), 30f,
            5f, 60f, 1f, null, true);
        hunterLightDuration = new CustomOption(3006, Types.HideNSeekRoles, cs(Color.red, "Hunter Light Duration"), 5f,
            1f, 60f, 1f);
        hunterLightVision = new CustomOption(3007, Types.HideNSeekRoles, cs(Color.red, "Hunter Light Vision"), 3f, 1f,
            5f, 0.25f);
        hunterLightPunish = new CustomOption(3008, Types.HideNSeekRoles, cs(Color.red, "Hunter Light Punish In Sec"),
            5f, 0f, 30f, 1f);
        hunterAdminCooldown = new CustomOption(3009, Types.HideNSeekRoles, cs(Color.red, "Hunter Admin Cooldown"), 30f,
            5f, 60f, 1f);
        hunterAdminDuration = new CustomOption(3010, Types.HideNSeekRoles, cs(Color.red, "Hunter Admin Duration"), 5f,
            1f, 60f, 1f);
        hunterAdminPunish = new CustomOption(3011, Types.HideNSeekRoles, cs(Color.red, "Hunter Admin Punish In Sec"),
            5f, 0f, 30f, 1f);
        hunterArrowCooldown = new CustomOption(3012, Types.HideNSeekRoles, cs(Color.red, "Hunter Arrow Cooldown"), 30f,
            5f, 60f, 1f);
        hunterArrowDuration = new CustomOption(3013, Types.HideNSeekRoles, cs(Color.red, "Hunter Arrow Duration"), 5f,
            0f, 60f, 1f);
        hunterArrowPunish = new CustomOption(3014, Types.HideNSeekRoles, cs(Color.red, "Hunter Arrow Punish In Sec"),
            5f, 0f, 30f, 1f);

        huntedShieldCooldown = new CustomOption(3015, Types.HideNSeekRoles, cs(Color.gray, "Hunted Shield Cooldown"),
            30f, 5f, 60f, 1f, null, true);
        huntedShieldDuration = new CustomOption(3016, Types.HideNSeekRoles, cs(Color.gray, "Hunted Shield Duration"),
            5f, 1f, 60f, 1f);
        huntedShieldRewindTime = new CustomOption(3018, Types.HideNSeekRoles, cs(Color.gray, "Hunted Rewind Time"), 3f,
            1f, 10f, 1f);
        huntedShieldNumber = new CustomOption(3026, Types.HideNSeekRoles, cs(Color.gray, "Hunted Shield Number"), 3f,
            1f, 15f, 1f);

        //-------------------------- Prop Hunt General Options 4000 - 4999 -------------------------- //

        propHuntMap = new CustomOption(4020, Types.PropHunt, cs(Color.yellow, "Map"),
            ["The Skeld", "Mira", "Polus", "Airship", "Fungle", "Submerged", "LI Map"], null, true, onChange: () =>
            {
                int map = propHuntMap.OptionSelection;
                if (map >= 3) map++;
                GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)map;
            });
        propHuntTimer = new CustomOption(4021, Types.PropHunt, cs(Color.yellow, "Timer In Min"), 5f, 1f, 30f, 0.5f);
        propHuntUnstuckCooldown = new CustomOption(4011, Types.PropHunt, cs(Color.yellow, "Unstuck Cooldown"), 30f,
            2.5f, 60f, 2.5f);
        propHuntUnstuckDuration =
            new CustomOption(4012, Types.PropHunt, cs(Color.yellow, "Unstuck Duration"), 2f, 1f, 60f, 1f);
        propHunterVision =
            new CustomOption(4006, Types.PropHunt, cs(Color.yellow, "Hunter Vision"), 0.5f, 0.25f, 2f, 0.25f);
        propVision = new CustomOption(4007, Types.PropHunt, cs(Color.yellow, "Prop Vision"), 2f, 0.25f, 5f, 0.25f);
        // Hunter Options
        propHuntNumberOfHunters = new CustomOption(4000, Types.PropHunt, cs(Color.red, "Number Of Hunters"), 1f, 1f, 5f,
            1f, null, true);
        hunterInitialBlackoutTime = new CustomOption(4001, Types.PropHunt,
            cs(Color.red, "Hunter Initial Blackout Duration"), 10f, 5f, 20f, 1f);
        hunterMissCooldown = new CustomOption(4004, Types.PropHunt, cs(Color.red, "Kill Cooldown After Miss"), 10f,
            2.5f, 60f, 2.5f);
        hunterHitCooldown = new CustomOption(4005, Types.PropHunt, cs(Color.red, "Kill Cooldown After Hit"), 10f, 2.5f,
            60f, 2.5f);
        propHuntRevealCooldown = new CustomOption(4008, Types.PropHunt, cs(Color.red, "Reveal Prop Cooldown"), 30f, 10f,
            90f, 2.5f);
        propHuntRevealDuration =
            new CustomOption(4009, Types.PropHunt, cs(Color.red, "Reveal Prop Duration"), 5f, 1f, 60f, 1f);
        propHuntRevealPunish =
            new CustomOption(4010, Types.PropHunt, cs(Color.red, "Reveal Time Punish"), 10f, 0f, 1800f, 5f);
        propHuntAdminCooldown = new CustomOption(4022, Types.PropHunt, cs(Color.red, "Hunter Admin Cooldown"), 30f,
            2.5f, 1800f, 2.5f);
        propHuntFindCooldown =
            new CustomOption(4023, Types.PropHunt, cs(Color.red, "Find Cooldown"), 60f, 2.5f, 1800f, 2.5f);
        propHuntFindDuration = new CustomOption(4024, Types.PropHunt, cs(Color.red, "Find Duration"), 5f, 1f, 15f, 1f);
        // Prop Options
        propBecomesHunterWhenFound = new CustomOption(4003, Types.PropHunt,
            cs(Palette.CrewmateBlue, "Props Become Hunters When Found"), false, null, true);
        propHuntInvisEnabled = new CustomOption(4013, Types.PropHunt, cs(Palette.CrewmateBlue, "Invisibility Enabled"),
            true, null, true);
        propHuntInvisCooldown = new CustomOption(4014, Types.PropHunt,
            cs(Palette.CrewmateBlue, "Invisibility Cooldown"), 120f, 10f, 1800f, 2.5f, propHuntInvisEnabled);
        propHuntInvisDuration = new CustomOption(4015, Types.PropHunt,
            cs(Palette.CrewmateBlue, "Invisibility Duration"), 5f, 1f, 30f, 1f, propHuntInvisEnabled);
        propHuntSpeedboostEnabled = new CustomOption(4016, Types.PropHunt,
            cs(Palette.CrewmateBlue, "Speedboost Enabled"), true, null, true);
        propHuntSpeedboostCooldown = new CustomOption(4017, Types.PropHunt,
            cs(Palette.CrewmateBlue, "Speedboost Cooldown"), 60f, 2.5f, 1800f, 2.5f, propHuntSpeedboostEnabled);
        propHuntSpeedboostDuration = new CustomOption(4018, Types.PropHunt,
            cs(Palette.CrewmateBlue, "Speedboost Duration"), 5f, 1f, 15f, 1f, propHuntSpeedboostEnabled);
        propHuntSpeedboostSpeed = new CustomOption(4019, Types.PropHunt, cs(Palette.CrewmateBlue, "Speedboost Ratio"),
            2f, 1.25f, 5f, 0.25f, propHuntSpeedboostEnabled);


        blockedRolePairings.Add((byte)RoleId.Vampire, [(byte)RoleId.Warlock]);
        blockedRolePairings.Add((byte)RoleId.Warlock, [(byte)RoleId.Vampire]);
        blockedRolePairings.Add((byte)RoleId.Spy, [(byte)RoleId.Mini]);
        blockedRolePairings.Add((byte)RoleId.Mini, [(byte)RoleId.Spy]);
        blockedRolePairings.Add((byte)RoleId.Vulture, [(byte)RoleId.Cleaner]);
        blockedRolePairings.Add((byte)RoleId.Cleaner, [(byte)RoleId.Vulture]);

        blockedRolePairings.Add((byte)RoleId.Mayor, [(byte)RoleId.Watcher]);
        blockedRolePairings.Add((byte)RoleId.Watcher, [(byte)RoleId.Mayor]);
        blockedRolePairings.Add((byte)RoleId.Engineer, [(byte)RoleId.Tunneler]);
        blockedRolePairings.Add((byte)RoleId.Tunneler, [(byte)RoleId.Engineer]);
        blockedRolePairings.Add((byte)RoleId.Bomber2, [(byte)RoleId.Bait]);
        blockedRolePairings.Add((byte)RoleId.Bait, [(byte)RoleId.Bomber2]);
    }
}