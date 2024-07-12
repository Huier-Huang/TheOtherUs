using System.Linq;
using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

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

    public float cooldown = 30f;
    public float duration = 10f;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Camouflager),
        RoleClassType = typeof(Camouflager),
        RoleTeam = RoleTeam.Impostor,
        RoleId = RoleId.Camouflager,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Camouflager>,
        Color = Palette.ImpostorRed,
        DescriptionText = "Hide among others",
        IntroInfo = "Camouflage and kill the Crewmates",
        CreateRoleController = n => new CamouflagerController(n)
    };
    
    public class CamouflagerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Camouflager>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public void resetCamouflage()
    {
        if (ButtonHelper.isCamoComms()) return;
        camouflageTimer = 0f;
        foreach (var p in AllPlayers.Where(p =>
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
        cooldown = camouflagerCooldown;
        duration = camouflagerDuration;
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Camouflager camouflage
        camouflagerButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.CamouflagerCamouflage, SendOption.Reliable);
                writer.Write(1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                /*RPCProcedure.camouflagerCamouflage(1);*/
                SoundEffectsManager.play("morphlingMorph");
            },
            () => camouflager != null &&
                  camouflager == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () => !ButtonHelper.isCommsActive() && LocalPlayer.Control.CanMove,
            () =>
            {
                camouflagerButton.Timer = camouflagerButton.MaxTimer;
                camouflagerButton.isEffectActive = false;
                camouflagerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            buttonSprite,
            DefButtonPositions.upperRowLeft,
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