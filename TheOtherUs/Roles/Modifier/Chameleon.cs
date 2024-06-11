using System;
using System.Collections.Generic;
using AmongUs.Data;
using UnityEngine;
using Impostor_Ninja = TheOtherUs.Roles.Impostors.Ninja;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Chameleon : RoleBase
{
    public List<PlayerControl> chameleon = [];
    public float fadeDuration = 0.5f;
    public float holdDuration = 1f;
    public Dictionary<byte, float> lastMoved;
    public float minVisibility = 0.2f;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        chameleon = [];
        lastMoved = new Dictionary<byte, float>();
        holdDuration = CustomOptionHolder.modifierChameleonHoldDuration.getFloat();
        fadeDuration = CustomOptionHolder.modifierChameleonFadeDuration.getFloat();
        minVisibility = CustomOptionHolder.modifierChameleonMinVisibility.getSelection() / 10f;
    }

    public float visibility(byte playerId)
    {
        var visibility = 1f;
        if (lastMoved != null && lastMoved.TryGetValue(playerId, out var value))
        {
            var tStill = Time.time - value;
            if (tStill > holdDuration)
            {
                if (tStill - holdDuration > fadeDuration) visibility = minVisibility;
                else
                    visibility = ((1 - ((tStill - holdDuration) / fadeDuration)) * (1 - minVisibility)) + minVisibility;
            }
        }

        if (PlayerControl.LocalPlayer.Data.IsDead && visibility < 0.1f) // Ghosts can always see!
            visibility = 0.1f;
        return visibility;
    }

    public void update()
    {
        foreach (var chameleonPlayer in chameleon)
        {
            if
            (
                (
                    chameleonPlayer.Is<Impostor_Ninja>()
                    &&
                    Get<Impostor_Ninja>().isInvisble
                )
                ||
                (
                    chameleonPlayer.Is<Jackal>()
                    &&
                    Get<Jackal>().isInvisable
                )
            )
                continue; // Dont make Ninja visible...
            // check movement by animation
            var playerPhysics = chameleonPlayer.MyPhysics;
            var currentPhysicsAnim = playerPhysics.Animations.Animator.GetCurrentAnimation();
            if (currentPhysicsAnim != playerPhysics.Animations.group.IdleAnim)
                lastMoved[chameleonPlayer.PlayerId] = Time.time;
            // calculate and set visibility
            var visibility = this.visibility(chameleonPlayer.PlayerId);
            var petVisibility = visibility;
            if (chameleonPlayer.Data.IsDead)
            {
                visibility = 0.5f;
                petVisibility = 1f;
            }

            try
            {
                // Sometimes renderers are missing for weird reasons. Try catch to avoid exceptions
                chameleonPlayer.cosmetics.currentBodySprite.BodySprite.color =
                    chameleonPlayer.cosmetics.currentBodySprite.BodySprite.color.SetAlpha(visibility);
                if (DataManager.Settings.Accessibility.ColorBlindMode)
                    chameleonPlayer.cosmetics.colorBlindText.color =
                        chameleonPlayer.cosmetics.colorBlindText.color.SetAlpha(visibility);
                chameleonPlayer.SetHatAndVisorAlpha(visibility);
                chameleonPlayer.cosmetics.skin.layer.color =
                    chameleonPlayer.cosmetics.skin.layer.color.SetAlpha(visibility);
                chameleonPlayer.cosmetics.nameText.color =
                    chameleonPlayer.cosmetics.nameText.color.SetAlpha(visibility);
                foreach (var rend in chameleonPlayer.cosmetics.currentPet.renderers)
                    rend.color = rend.color.SetAlpha(petVisibility);
                foreach (var shadowRend in chameleonPlayer.cosmetics.currentPet.shadows)
                    shadowRend.color = shadowRend.color.SetAlpha(petVisibility);
            }
            catch
            {
                // ignored
            }
        }
    }
}