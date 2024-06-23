using TheOtherUs.Objects;
using TheOtherUs.Patches;
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
            Get<Camouflager>().camouflageTimer >= 0.1f || Helpers.isCamoComms()
                                                       ||
                                                       Helpers.isLightsActive()
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
        button.actionButton.cooldownTimerText.transform.localPosition =
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
        targetDisplay.setSemiTransparent(false);
        targetDisplay.gameObject.SetActive(true);
    }
}