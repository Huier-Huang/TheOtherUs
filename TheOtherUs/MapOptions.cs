using System.Collections.Generic;
using TheOtherUs.Helper;
using TheOtherUs.Roles.Crewmate;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs;

internal static class MapOptions
{
    // Set values
    public static int maxNumberOfMeetings = 10;
    public static bool blockSkippingInEmergencyMeetings;
    public static bool noVoteIsSelfVote;
    public static bool hidePlayerNames;
    public static bool ghostsSeeRoles = true;
    public static bool ghostsSeeModifier = true;
    public static bool ghostsSeeInformation = true;
    public static bool ghostsSeeVotes = true;
    public static bool showRoleSummary = true;
    public static bool allowParallelMedBayScans;
    public static bool showLighterDarker;
    public static bool enableSoundEffects = true;
    public static bool enableHorseMode;
    public static bool shieldFirstKill;
    public static bool hideVentAnim;
    public static bool impostorSeeRoles;
    public static bool transparentTasks;
    public static bool hideOutOfSightNametags;
    public static bool disableMedscanWalking;
    public static int restrictDevices;
    public static float restrictCamerasTime = 600f;
    public static float restrictCamerasTimeMax = 600f;
    public static float restrictVitalsTime = 600f;
    public static float restrictVitalsTimeMax = 600f;
    public static bool disableCamsRoundOne;
    public static bool isRoundOne = true;
    public static bool camoComms;
    public static CustomGameModes gameMode = CustomGameModes.Classic;

    // Updating values
    public static int meetingsCount;
    public static List<SurvCamera> camerasToAdd = [];
    public static List<Vent> ventsToSeal = [];
    public static Dictionary<byte, PoolablePlayer> playerIcons = new();
    public static string firstKillName;
    public static PlayerControl firstKillPlayer;

    public static bool CanShow => CachedPlayer.LocalPlayer.Control.Is<Hacker>() ||
                                  CachedPlayer.LocalPlayer.Data.IsDead ||
                                  restrictDevices == 0;

    public static bool canUseCameras => CanShow || restrictCamerasTime > 0f;

    public static bool couldUseCameras => CanShow || restrictCamerasTimeMax > 0f;

    public static bool canUseVitals => CanShow || restrictVitalsTime > 0f;

    public static bool couldUseVitals => CanShow || restrictVitalsTimeMax > 0f;

    public static void clearAndReloadMapOptions()
    {
        meetingsCount = 0;
        camerasToAdd = [];
        ventsToSeal = [];
        playerIcons = new Dictionary<byte, PoolablePlayer>();

        maxNumberOfMeetings = Mathf.RoundToInt(CustomOptionHolder.maxNumberOfMeetings.Selection);
        blockSkippingInEmergencyMeetings = CustomOptionHolder.blockSkippingInEmergencyMeetings;
        noVoteIsSelfVote = CustomOptionHolder.noVoteIsSelfVote;
        hidePlayerNames = CustomOptionHolder.hidePlayerNames;
        hideOutOfSightNametags = CustomOptionHolder.hideOutOfSightNametags;
        hideVentAnim = CustomOptionHolder.hideVentAnimOnShadows;
        allowParallelMedBayScans = CustomOptionHolder.allowParallelMedBayScans;
        disableMedscanWalking = CustomOptionHolder.disableMedbayWalk;
        camoComms = CustomOptionHolder.enableCamoComms;
        shieldFirstKill = CustomOptionHolder.shieldFirstKill;
        impostorSeeRoles = CustomOptionHolder.impostorSeeRoles;
        transparentTasks = CustomOptionHolder.transparentTasks;
        restrictDevices = CustomOptionHolder.restrictDevices;
        restrictCamerasTime = restrictCamerasTimeMax = CustomOptionHolder.restrictCameras;
        restrictVitalsTime = restrictVitalsTimeMax = CustomOptionHolder.restrictVents;
        disableCamsRoundOne = CustomOptionHolder.disableCamsRound1;
        firstKillPlayer = null;
        isRoundOne = true;
    }

    public static void reloadPluginOptions()
    {
        ghostsSeeRoles = Main.GhostsSeeRoles.Value;
        ghostsSeeModifier = Main.GhostsSeeModifier.Value;
        ghostsSeeInformation = Main.GhostsSeeInformation.Value;
        ghostsSeeVotes = Main.GhostsSeeVotes.Value;
        showRoleSummary = Main.ShowRoleSummary.Value;
        showLighterDarker = Main.ShowLighterDarker.Value;
        enableSoundEffects = Main.EnableSoundEffects.Value;
        enableHorseMode = Main.EnableHorseMode.Value;
    }

    public static void resetDeviceTimes()
    {
        restrictCamerasTime = restrictCamerasTimeMax;
        restrictVitalsTime = restrictVitalsTimeMax;
    }
}