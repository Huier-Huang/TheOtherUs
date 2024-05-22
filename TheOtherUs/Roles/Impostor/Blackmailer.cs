using System;
using Hazel;
using TheOtherUs.Helper;
using TheOtherUs.Modules;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs.Roles.Impostor;

[RegisterRole]
public class Blackmailer : RoleBase
{
    //private ResourceSprite overlaySprite = new("BlackmailerOverlay.png");
    private static Sprite overlaySprite;

    public static CustomOption blackmailerSpawnRate;
    public static CustomOption blackmailerCooldown;

    public static CustomButton blackmailerButton;
    public bool alreadyShook = false;

    private readonly ResourceSprite blackmailButtonSprite = new("BlackmailerBlackmailButton.png");
    public PlayerControl blackmailed;
    public Color blackmailedColor = Palette.White;
    public PlayerControl blackmailer;
    public Color color = Palette.ImpostorRed;
    public float cooldown = 30f;
    public PlayerControl currentTarget;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public static Sprite getBlackmailOverlaySprite()
    {
        if (overlaySprite) return overlaySprite;
        overlaySprite = UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.BlackmailerOverlay.png", 100f);
        return overlaySprite;
    }

    public static Sprite getBlackmailLetterSprite()
    {
        if (overlaySprite) return overlaySprite;
        overlaySprite = UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.BlackmailerLetter.png", 115f);
        return overlaySprite;
    }

    public override void OptionCreate()
    {
        blackmailerSpawnRate =
            new CustomOption(710, "Blackmailer".ColorString(color), CustomOptionHolder.rates, null, true);
        blackmailerCooldown = new CustomOption(711, "Blackmail Cooldown", 30f, 5f, 120f, 5f,
            blackmailerSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        blackmailerButton = new CustomButton(
            () =>
            {
                // Action when Pressed
                if (currentTarget == null) return;
                if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);
                var writer = AmongUsClient.Instance.StartRpcImmediately(
                    CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.BlackmailPlayer,
                    SendOption.Reliable);
                writer.Write(currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.blackmailPlayer(currentTarget.PlayerId);
                blackmailerButton.Timer = blackmailerButton.MaxTimer;
            },
            () =>
            {
                return blackmailer != null &&
                       blackmailer == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                // Could Use
                var text = "BLACKMAIL";
                if (blackmailed != null) text = blackmailed.Data.PlayerName;
                ButtonHelper.showTargetNameOnButtonExplicit(currentTarget, blackmailerButton,
                    text); //Show target name under button if setting is true
                return currentTarget != null && CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () => { blackmailerButton.Timer = blackmailerButton.MaxTimer; },
            blackmailButtonSprite,
            CustomButton.ButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.F,
            true,
            0f,
            () => { },
            false,
            "Blackmail"
        );
    }

    public override void ResetCustomButton()
    {
        blackmailerButton.MaxTimer = cooldown;
    }


    public override void ClearAndReload()
    {
        blackmailer = null;
        currentTarget = null;
        blackmailed = null;
        cooldown = blackmailerCooldown.getFloat();
    }
}