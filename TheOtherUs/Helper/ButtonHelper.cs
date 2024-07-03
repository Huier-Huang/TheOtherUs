using System.Linq;
using TheOtherUs.Modules.Compatibility;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Helper;

public static class ButtonHelper
{
    public static PoolablePlayer targetDisplay;

    public static void showTargetNameOnButton(PlayerControl target, CustomButton button, string defaultText)
    {
        if (!CustomOptionHolder.showButtonTarget) return;
        // Should the button show the target name option
        var text = target.Data.PlayerName;
        var _morphling = Get<Morphling>();
        if (_morphling.morphling != null && _morphling.morphTarget != null && target == _morphling.morphling &&
            _morphling.morphTimer > 0)
            goto End;

        if (
            Get<Camouflager>().camouflageTimer >= 0.1f || 
                                                       isCamoComms()
                                                       ||
                                                       isLightsActive()
                                                       ||
                                                       (Get<Trickster>().trickster != null &&
                                                        Get<Trickster>().lightsOutTimer > 0f)
                                                       ||
                                                       (Get<Trickster>().trickster != null &&
                                                        Get<Trickster>().lightsOutTimer > 0f)
                                                       ||
                                                       (target.Is<Jackal>() && Get<Jackal>().isInvisable)
                                                       ||
                                                       (target.Is<Jackal>() && Get<Jackal>().isInvisable)
                                                       ||
                                                       target == null
        )
            text = defaultText;

        End: // Set text to defaultText if no target
        showTargetNameOnButtonExplicit(null, button, text);
    }


    public static void showTargetNameOnButtonExplicit(PlayerControl target, CustomButton button, string defaultText)
    {
        var text = target == null
            ? defaultText
            : // Set text to defaultText if no target
            target.Data.PlayerName; // Set text to playername
        button.actionButton.OverrideText(text);
        button.showButtonText = true;
    }

    public static void setButtonTargetDisplay(PlayerControl target, CustomButton button = null, Vector3? offset = null)
    {
        if (target == null || button == null)
        {
            if (targetDisplay == null) return;
            // Reset the poolable player
            GameObject gameObject;
            (gameObject = targetDisplay.gameObject).SetActive(false);
            Object.Destroy(gameObject);
            targetDisplay = null;

            return;
        }

        // Add poolable player to the button so that the target outfit is shown
        /*button.actionButton.cooldownTimerText.transform.localPosition =
            new Vector3(0, 0, -1f); // Before the poolable player
        targetDisplay = Object.Instantiate(IntroCutsceneOnDestroyPatch.playerPrefab, button.actionButton.transform);
        var data = target.Data;
        target.SetPlayerMaterialColors(targetDisplay.cosmetics.currentBodySprite.BodySprite);
        targetDisplay.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
        targetDisplay.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
        targetDisplay.cosmetics.nameText.text = ""; // Hide the name!
        targetDisplay.transform.localPosition = new Vector3(0f, 0.22f, -0.01f);
        if (offset != null) targetDisplay.transform.localPosition += (Vector3)offset;
        targetDisplay.transform.localScale = Vector3.one * 0.33f;
        targetDisplay.gameObject.SetActive(true);*/
    }
    
    public static SabatageTypes getActiveSabo()
    {
        foreach (var task in LocalPlayer.Control.myTasks.GetFastEnumerator())
            switch (task.TaskType)
            {
                case TaskTypes.FixLights:
                    return SabatageTypes.Lights;
                case TaskTypes.RestoreOxy:
                    return SabatageTypes.O2;
                default:
                {
                    if (task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.StopCharles ||
                        task.TaskType == TaskTypes.StopCharles)
                        return SabatageTypes.Reactor;
                    if (task.TaskType == TaskTypes.FixComms)
                        return SabatageTypes.Comms;

                    var compatibility = CompatibilityManager.Instance.GetCompatibility<SubmergedCompatibility>();
                    if (compatibility.IsSubmerged && task.TaskType == compatibility.RetrieveOxygenMask)
                        return SabatageTypes.OxyMask;
                    break;
                }
            }
        return SabatageTypes.None;
    }
    
    public static bool isLightsActive()
    {
        return getActiveSabo() == SabatageTypes.Lights;
    }

    public static bool isCommsActive()
    {
        return getActiveSabo() == SabatageTypes.Comms;
    }


    public static bool isCamoComms()
    {
        return isCommsActive();
    }
    
    public static bool checkAndDoVetKill(PlayerControl target)
    {
        var shouldVetKill =  target.Is<Veteren>() && Get<Veteren>().alertActive;
        if (shouldVetKill)
        {
            var writer = FastRpcWriter.StartNewRpcWriter(CustomRPC.VeterenKill);
            writer.Write(LocalPlayer.Control.PlayerId).RPCSend();
            /*RPCProcedure.veterenKill(CachedPlayer.LocalPlayer.Control.PlayerId);*/
        }

        return shouldVetKill;
    }
    
    
    public static bool hidePlayerName(PlayerControl source, PlayerControl target)
    {
        var localPlayer = PlayerControl.LocalPlayer;
        /*if (Camouflager.camouflageTimer > 0f || MushroomSabotageActive() || isCamoComms())
            return true; // No names are visible
        if (SurveillanceMinigamePatch.nightVisionIsActive) return true;
        if (Ninja.isInvisble && Ninja.ninja == target) return true;
        if (Jackal.isInvisable && Jackal.jackal == target) return true;*/
        if (/*MapOptions.hideOutOfSightNametags && gameStarted && !source.Data.IsDead &&*/
            PhysicsHelpers.AnythingBetween(localPlayer.GetTruePosition(), target.GetTruePosition(),
                Constants.ShadowMask, false)) return true;
 
        {
            float num = (isLightsActive() ? 2f : 1.25f);
            float num2 = Vector3.Distance(source.transform.position, target.transform.position);
            if (PhysicsHelpers.AnythingBetween(source.GetTruePosition(), target.GetTruePosition(), Constants.ShadowMask, useTriggers: false))
            {
                return true;
            }
        }
        /*if (!MapOptions.hidePlayerNames) return false; // All names are visible*/
        /*if (source == null || target == null) return true;
        if (source == target) return false; // Player sees his own name
        if (source.Data.Role.IsImpostor && (target.Data.Role.IsImpostor || target == Spy.spy ||
                                            (target == Sidekick.sidekick && Sidekick.wasTeamRed) ||
                                            (target == Jackal.jackal && Jackal.wasTeamRed)))
            return false; // Members of team Impostors see the names of Impostors/Spies
        if ((source == Lovers.lover1 || source == Lovers.lover2) &&
            (target == Lovers.lover1 || target == Lovers.lover2))
            return false; // Members of team Lovers see the names of each other
        if ((source == Jackal.jackal || source == Sidekick.sidekick) && (target == Jackal.jackal ||
                                                                         target == Sidekick.sidekick ||
                                                                         target == Jackal.fakeSidekick))
            return false; // Members of team Jackal see the names of each other
        if (Deputy.knowsSheriff && (source == Sheriff.sheriff || source == Deputy.deputy) &&
            (target == Sheriff.sheriff || target == Deputy.deputy))
            return false;*/ // Sheriff & Deputy see the names of each other
        return true;
    }
    
    public static bool MushroomSabotageActive()
    {
        return LocalPlayer.Control.myTasks.ToArray()
            .Any(x => x.TaskType == TaskTypes.MushroomMixupSabotage);
    }
}