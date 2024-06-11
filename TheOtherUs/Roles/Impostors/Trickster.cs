using System;
using Hazel;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using TheOtherUs.Roles.Crewmates;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Trickster : RoleBase
{
    public Color color = Palette.ImpostorRed;
    private readonly ResourceSprite lightOutButtonSprite = new("LightsOutButton.png");
    private CustomButton lightsOutButton;
    public float lightsOutCooldown = 30f;
    public float lightsOutDuration = 10f;
    public float lightsOutTimer;

    private readonly ResourceSprite placeBoxButtonSprite = new("PlaceJackInTheBoxButton.png");
    public float placeBoxCooldown = 30f;

    private CustomButton placeJackInTheBoxButton;
    public PlayerControl trickster;
    public CustomOption tricksterLightsOutCooldown;
    public CustomOption tricksterLightsOutDuration;
    public CustomOption tricksterPlaceBoxCooldown;

    public CustomOption tricksterSpawnRate;
    private ResourceSprite tricksterVentButtonSprite = new("TricksterVentButton.png");

    public override RoleInfo RoleInfo { get; protected set; }

    public override Type RoleType { get; protected set; }
        = typeof(Tracker);


    public override void ClearAndReload()
    {
        trickster = null;
        lightsOutTimer = 0f;
        placeBoxCooldown = tricksterPlaceBoxCooldown.getFloat();
        lightsOutCooldown = tricksterLightsOutCooldown.getFloat();
        lightsOutDuration = tricksterLightsOutDuration.getFloat();
        JackInTheBox.UpdateStates(); // if the role is erased, we might have to update the state of the created objects
    }

    public override void OptionCreate()
    {
        tricksterSpawnRate =
            new CustomOption(250, "Trickster".ColorString(color), CustomOptionHolder.rates, null, true);
        tricksterPlaceBoxCooldown = new CustomOption(251, "Trickster Box Cooldown", 10f, 2.5f, 30f,
            2.5f, tricksterSpawnRate);
        tricksterLightsOutCooldown = new CustomOption(252, "Trickster Lights Out Cooldown", 30f, 10f,
            60f, 5f, tricksterSpawnRate);
        tricksterLightsOutDuration = new CustomOption(253, "Trickster Lights Out Duration", 15f, 5f,
            60f, 2.5f, tricksterSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        placeJackInTheBoxButton = new CustomButton(
            () =>
            {
                placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer;

                var pos = CachedPlayer.LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.PlaceJackInTheBox);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.placeJackInTheBox(buff);
                SoundEffectsManager.play("tricksterPlaceBox");
            },
            () => trickster != null && trickster == CachedPlayer.LocalPlayer.Control &&
                  !CachedPlayer.LocalPlayer.Data.IsDead && !JackInTheBox.hasJackInTheBoxLimitReached(),
            () => CachedPlayer.LocalPlayer.Control.CanMove && !JackInTheBox.hasJackInTheBoxLimitReached(),
            () => { placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer; },
            placeBoxButtonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );

        lightsOutButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.LightsOut, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.lightsOut();
                SoundEffectsManager.play("lighterLight");
            },
            () => trickster != null && trickster == CachedPlayer.LocalPlayer.Control &&
                  !CachedPlayer.LocalPlayer.Data.IsDead
                  && JackInTheBox.hasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents,
            () => CachedPlayer.LocalPlayer.Control.CanMove && JackInTheBox.hasJackInTheBoxLimitReached() &&
                  JackInTheBox.boxesConvertedToVents,
            () =>
            {
                lightsOutButton.Timer = lightsOutButton.MaxTimer;
                lightsOutButton.isEffectActive = false;
                lightsOutButton.actionButton.graphic.color = Palette.EnabledColor;
            },
            lightOutButtonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F,
            true,
            lightsOutDuration,
            () =>
            {
                lightsOutButton.Timer = lightsOutButton.MaxTimer;
                SoundEffectsManager.play("lighterLight");
            }
        );
    }

    public override void ResetCustomButton()
    {
        lightsOutButton.EffectDuration = lightsOutDuration;
        placeJackInTheBoxButton.MaxTimer = placeBoxCooldown;
        lightsOutButton.MaxTimer = lightsOutCooldown;
    }
}