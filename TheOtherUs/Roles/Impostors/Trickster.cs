using System;
using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Trickster : RoleBase
{

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

    
    private ResourceSprite tricksterVentButtonSprite = new("TricksterVentButton.png");

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Trickster),
        RoleClassType = typeof(Trickster),
        Color = Palette.ImpostorRed,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        RoleId = RoleId.Trickster,
        GetRole = Get<Trickster>,
        IntroInfo = "Use your jack-in-the-boxes to surprise others",
        DescriptionText = "Surprise your enemies",
        CreateRoleController = player => new TricksterController(player)
    };
    
    public class TricksterController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Trickster>();
    }

    public override CustomRoleOption roleOption { get; set; }



    public override void ClearAndReload()
    {
        trickster = null;
        lightsOutTimer = 0f;
        placeBoxCooldown = tricksterPlaceBoxCooldown;
        lightsOutCooldown = tricksterLightsOutCooldown;
        lightsOutDuration = tricksterLightsOutDuration;
        JackInTheBox.UpdateStates(); // if the role is erased, we might have to update the state of the created objects
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        tricksterPlaceBoxCooldown = roleOption.AddChild( "Trickster Box Cooldown", new FloatOptionSelection(10f, 2.5f, 30f,
            2.5f));
        tricksterLightsOutCooldown = roleOption.AddChild("Trickster Lights Out Cooldown", new FloatOptionSelection(30f, 10f,
            60f, 5f));
        tricksterLightsOutDuration = roleOption.AddChild("Trickster Lights Out Duration", new FloatOptionSelection(15f, 5f,
            60f, 2.5f));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        placeJackInTheBoxButton = new CustomButton(
            () =>
            {
                placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer;

                var pos = LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var writer = AmongUsClient.Instance.StartRpc(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.PlaceJackInTheBox);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                /*RPCProcedure.placeJackInTheBox(buff);*/
                SoundEffectsManager.play("tricksterPlaceBox");
            },
            () => trickster != null && trickster == LocalPlayer.Control &&
                  !LocalPlayer.IsDead && !JackInTheBox.hasJackInTheBoxLimitReached(),
            () => LocalPlayer.Control.CanMove && !JackInTheBox.hasJackInTheBoxLimitReached(),
            () => { placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer; },
            placeBoxButtonSprite,
            DefButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );

        lightsOutButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.LightsOut, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                /*RPCProcedure.lightsOut();*/
                SoundEffectsManager.play("lighterLight");
            },
            () => trickster != null && trickster == LocalPlayer.Control &&
                  !LocalPlayer.IsDead
                  && JackInTheBox.hasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents,
            () => LocalPlayer.Control.CanMove && JackInTheBox.hasJackInTheBoxLimitReached() &&
                  JackInTheBox.boxesConvertedToVents,
            () =>
            {
                lightsOutButton.Timer = lightsOutButton.MaxTimer;
                lightsOutButton.isEffectActive = false;
                lightsOutButton.actionButton.graphic.color = Palette.EnabledColor;
            },
            lightOutButtonSprite,
            DefButtonPositions.upperRowLeft,
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