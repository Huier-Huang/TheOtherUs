using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Arsonist : RoleBase
{
    public PlayerControl arsonist;

    public CustomButton arsonistButton;
    public CustomOption arsonistCooldown;
    public CustomOption arsonistDuration;

    public float cooldown = 30f;

    public PlayerControl currentTarget;
    public List<PlayerControl> dousedPlayers = [];

    private readonly ResourceSprite douseSprite = new("DouseButton.png");
    public PlayerControl douseTarget;
    public float duration = 3f;
    private readonly ResourceSprite igniteSprite = new("IgniteButton.png");
    public bool triggerArsonistWin;
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Arsonist),
        RoleClassType = typeof(Arsonist),
        Color = new Color32(238, 112, 46, byte.MaxValue),
        RoleId = RoleId.Arsonist,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Neutral,
        GetRole = Get<Arsonist>,
        IntroInfo = "Let them burn",
        DescriptionText = "Let them burn",
        CreateRoleController = player => new ArsonistController(player)
    };
    public class ArsonistController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Arsonist>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        arsonistCooldown = roleOption.AddChild("Arsonist Cooldown", new FloatOptionSelection(12.5f, 2.5f, 60f, 2.5f));
        arsonistDuration = roleOption.AddChild("Arsonist Douse Duration", new FloatOptionSelection(3f, 1f, 10f, 1f));
    }

    public bool dousedEveryoneAlive()
    {
        return AllPlayers.All(x =>
        {
            return x.Is<Arsonist>() || x.IsDead || x.Disconnected ||
                   dousedPlayers.Any(y => y.PlayerId == x.PlayerId);
        });
    }

    public override void ClearAndReload()
    {
        arsonist = null;
        currentTarget = null;
        douseTarget = null;
        triggerArsonistWin = false;
        dousedPlayers = [];
        /*foreach (var p in MapOptions.playerIcons.Values.Where(p => p != null && p.gameObject != null))
            p.gameObject.SetActive(false);*/
        cooldown = arsonistCooldown;
        duration = arsonistDuration;
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Arsonist button
        arsonistButton = new CustomButton(
            () =>
            {
                //var dousedEveryoneAlive = dousedEveryoneAlive();
                if (dousedEveryoneAlive())
                {
                    var winWriter = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.ArsonistWin, SendOption.Reliable);
                    AmongUsClient.Instance.FinishRpcImmediately(winWriter);
                    /*RPCProcedure.arsonistWin();*/
                    arsonistButton.HasEffect = false;
                }
                else if (currentTarget != null)
                {
                    /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                    Helpers.checkWatchFlash(currentTarget);*/
                    douseTarget = currentTarget;
                    arsonistButton.HasEffect = true;
                    SoundEffectsManager.play("arsonistDouse");
                }
            },
            () => arsonist != null && arsonist == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                //var dousedEveryoneAlive = dousedEveryoneAlive();
                if (!dousedEveryoneAlive())
                    ButtonHelper.showTargetNameOnButton(currentTarget, arsonistButton, "");
                if (dousedEveryoneAlive()) arsonistButton.actionButton.graphic.sprite = igniteSprite;

                if (!arsonistButton.isEffectActive || douseTarget == currentTarget)
                    return LocalPlayer.Control.CanMove &&
                           (dousedEveryoneAlive() || currentTarget != null);
                douseTarget = null;
                arsonistButton.Timer = 0f;
                arsonistButton.isEffectActive = false;

                return LocalPlayer.Control.CanMove &&
                       (dousedEveryoneAlive() || currentTarget != null);
            },
            () =>
            {
                arsonistButton.Timer = arsonistButton.MaxTimer;
                arsonistButton.isEffectActive = false;
                douseTarget = null;
            },
            douseSprite,
            DefButtonPositions.lowerRowRight,
            _hudManager,
            KeyCode.F,
            true,
            duration,
            () =>
            {
                if (douseTarget != null) dousedPlayers.Add(douseTarget);

                arsonistButton.Timer = dousedEveryoneAlive() ? 0 : arsonistButton.MaxTimer;

                /*foreach (var p in dousedPlayers)
                    if (MapOptions.playerIcons.ContainsKey(p.PlayerId))
                        MapOptions.playerIcons[p.PlayerId].setSemiTransparent(false);*/

                // Ghost Info
                var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(LocalPlayer.PlayerId);
                /*writer.Write((byte)RPCProcedure.GhostInfoTypes.ArsonistDouse);*/
                writer.Write(douseTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                douseTarget = null;
            }
        );
    }

    public override void ResetCustomButton()
    {
        arsonistButton.MaxTimer = cooldown;
        arsonistButton.EffectDuration = duration;
    }
}