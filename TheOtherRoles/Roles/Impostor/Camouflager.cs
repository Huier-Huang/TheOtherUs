using System;
using System.Linq;
using Hazel;
using TheOtherRoles.Objects;
using TheOtherRoles.Roles.Neutral;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Camouflager : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("CamoButton.png");
    public bool camoComms;
    public PlayerControl camouflager;
    private CustomButton camouflagerButton;
    public CustomOption camouflagerCooldown;
    public CustomOption camouflagerDuration;

    public CustomOption camouflagerSpawnRate;
    public float camouflageTimer;
    public Color color = Palette.ImpostorRed;

    public float cooldown = 30f;
    public float duration = 10f;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public void resetCamouflage()
    {
        if (Helpers.isCamoComms()) return;
        camouflageTimer = 0f;
        foreach (var p in CachedPlayer.AllPlayers.Select(n => (PlayerControl)n).Where(p =>
                     (!p.Is<Ninja>() || !Get<Ninja>().isInvisble) && (!p.Is<Jackal>() || !Get<Jackal>().isInvisable)))
        {
            p.setDefaultLook();
            camoComms = false;
        }
    }

    public override void ClearAndReload()
    {
        resetCamouflage();
        camoComms = false;
        camouflager = null;
        camouflageTimer = 0f;
        cooldown = camouflagerCooldown.getFloat();
        duration = camouflagerDuration.getFloat();
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Camouflager camouflage
        camouflagerButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.CamouflagerCamouflage, SendOption.Reliable);
                writer.Write(1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.camouflagerCamouflage(1);
                SoundEffectsManager.play("morphlingMorph");
            },
            () =>
            {
                return camouflager != null &&
                       camouflager == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () => { return !Helpers.isActiveCamoComms() && CachedPlayer.LocalPlayer.Control.CanMove; },
            () =>
            {
                camouflagerButton.Timer = camouflagerButton.MaxTimer;
                camouflagerButton.isEffectActive = false;
                camouflagerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F,
            true,
            duration,
            () =>
            {
                camouflagerButton.Timer = camouflagerButton.MaxTimer;
                SoundEffectsManager.play("morphlingMorph");
            }
        );
    }

    public override void ResetCustomButton()
    {
        camouflagerButton.MaxTimer = cooldown;
        camouflagerButton.EffectDuration = duration;
    }
}