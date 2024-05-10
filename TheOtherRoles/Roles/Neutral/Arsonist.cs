using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherRoles.Objects;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Arsonist : RoleBase
{
    public PlayerControl arsonist;

    public CustomButton arsonistButton;
    public CustomOption arsonistCooldown;
    public CustomOption arsonistDuration;

    public CustomOption arsonistSpawnRate;
    public Color color = new Color32(238, 112, 46, byte.MaxValue);

    public float cooldown = 30f;

    public PlayerControl currentTarget;
    public List<PlayerControl> dousedPlayers = [];

    private readonly ResourceSprite douseSprite = new("DouseButton.png");
    public PlayerControl douseTarget;
    public float duration = 3f;
    private readonly ResourceSprite igniteSprite = new("IgniteButton.png");
    public bool triggerArsonistWin;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public bool dousedEveryoneAlive()
    {
        return CachedPlayer.AllPlayers.All(x =>
        {
            return x.PlayerControl == arsonist || x.Data.IsDead || x.Data.Disconnected ||
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
        foreach (var p in TORMapOptions.playerIcons.Values.Where(p => p != null && p.gameObject != null))
            p.gameObject.SetActive(false);
        cooldown = arsonistCooldown.getFloat();
        duration = arsonistDuration.getFloat();
    }

    public override void OptionCreate()
    {
        arsonistSpawnRate = new CustomOption(290, "Arsonist".ColorString(color), CustomOptionHolder.rates, null, true);
        arsonistCooldown = new CustomOption(291, "Arsonist Cooldown", 12.5f, 2.5f, 60f, 2.5f, arsonistSpawnRate);
        arsonistDuration = new CustomOption(292, "Arsonist Douse Duration", 3f, 1f, 10f, 1f, arsonistSpawnRate);
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
                        CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.ArsonistWin, SendOption.Reliable);
                    AmongUsClient.Instance.FinishRpcImmediately(winWriter);
                    RPCProcedure.arsonistWin();
                    arsonistButton.HasEffect = false;
                }
                else if (currentTarget != null)
                {
                    if (Helpers.checkAndDoVetKill(currentTarget)) return;
                    Helpers.checkWatchFlash(currentTarget);
                    douseTarget = currentTarget;
                    arsonistButton.HasEffect = true;
                    SoundEffectsManager.play("arsonistDouse");
                }
            },
            () =>
            {
                return arsonist != null && arsonist == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                //var dousedEveryoneAlive = dousedEveryoneAlive();
                if (!dousedEveryoneAlive())
                    ButtonHelper.showTargetNameOnButton(currentTarget, arsonistButton, "");
                if (dousedEveryoneAlive()) arsonistButton.actionButton.graphic.sprite = igniteSprite;

                if (!arsonistButton.isEffectActive || douseTarget == currentTarget)
                    return CachedPlayer.LocalPlayer.Control.CanMove &&
                           (dousedEveryoneAlive() || currentTarget != null);
                douseTarget = null;
                arsonistButton.Timer = 0f;
                arsonistButton.isEffectActive = false;

                return CachedPlayer.LocalPlayer.Control.CanMove &&
                       (dousedEveryoneAlive() || currentTarget != null);
            },
            () =>
            {
                arsonistButton.Timer = arsonistButton.MaxTimer;
                arsonistButton.isEffectActive = false;
                douseTarget = null;
            },
            douseSprite,
            CustomButton.ButtonPositions.lowerRowRight,
            _hudManager,
            KeyCode.F,
            true,
            duration,
            () =>
            {
                if (douseTarget != null) dousedPlayers.Add(douseTarget);

                arsonistButton.Timer = dousedEveryoneAlive() ? 0 : arsonistButton.MaxTimer;

                foreach (var p in dousedPlayers)
                    if (TORMapOptions.playerIcons.ContainsKey(p.PlayerId))
                        TORMapOptions.playerIcons[p.PlayerId].setSemiTransparent(false);

                // Ghost Info
                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.ArsonistDouse);
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