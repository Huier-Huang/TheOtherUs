using System;
using System.Collections.Generic;
using Hazel;
using TheOtherUs.Helper;
using TheOtherUs.Modules;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using TheOtherUs.Roles.Neutral;
using TheOtherUs.Utilities;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Crewmate;

[RegisterRole]
public class Deputy : RoleBase
{
    private readonly ResourceSprite buttonSprite = new("DeputyHandcuffButton.png");

    public Color color = new Color32(248, 205, 70, byte.MaxValue);
    public PlayerControl currentTarget;
    public PlayerControl deputy;
    public TMP_Text deputyButtonHandcuffsText;
    public CustomOption deputyGetsPromoted;

    private CustomButton deputyHandcuffButton;
    public CustomOption deputyHandcuffCooldown;
    public CustomOption deputyHandcuffDuration;
    public Dictionary<byte, List<CustomButton>> deputyHandcuffedButtons;
    public CustomOption deputyKeepsHandcuffs;
    public CustomOption deputyKnowsSheriff;
    public CustomOption deputyNumberOfHandcuffs;

    public CustomOption deputySpawnRate;
    public float handcuffCooldown;
    public float handcuffDuration;
    public Dictionary<byte, float> handcuffedKnows = new();
    public List<byte> handcuffedPlayers = [];
    private readonly ResourceSprite handcuffedSprite = new("DeputyHandcuffed.png");
    public bool keepsHandcuffsOnPromotion;
    public bool knowsSheriff;
    public int promotesToSheriff; // No: 0, Immediately: 1, After Meeting: 2
    public float remainingHandcuffs;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    // Can be used to enable / disable the handcuff effect on the target's buttons
    public void setHandcuffedKnows(bool active = true, byte playerId = byte.MaxValue)
    {
        if (playerId == byte.MaxValue)
            playerId = CachedPlayer.LocalPlayer.PlayerId;

        if (active && playerId == CachedPlayer.LocalPlayer.PlayerId)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
            writer.Write(CachedPlayer.LocalPlayer.PlayerId);
            writer.Write((byte)RPCProcedure.GhostInfoTypes.HandcuffNoticed);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        if (active)
        {
            handcuffedKnows.Add(playerId, handcuffDuration);
            handcuffedPlayers.RemoveAll(x => x == playerId);
        }

        if (playerId == CachedPlayer.LocalPlayer.PlayerId)
        {
            setAllButtonsHandcuffedStatus(active);
            SoundEffectsManager.play("deputyHandcuff");
        }
    }

    public override void ClearAndReload()
    {
        deputy = null;
        currentTarget = null;
        handcuffedPlayers = [];
        handcuffedKnows = new Dictionary<byte, float>();
        setAllButtonsHandcuffedStatus(false, true);
        promotesToSheriff = deputyGetsPromoted.getSelection();
        remainingHandcuffs = deputyNumberOfHandcuffs;
        handcuffCooldown = deputyHandcuffCooldown;
        keepsHandcuffsOnPromotion = deputyKeepsHandcuffs;
        handcuffDuration = deputyHandcuffDuration;
        knowsSheriff = deputyKnowsSheriff;
    }

    public override void OptionCreate()
    {
        deputySpawnRate = new CustomOption(103, "Sheriff Has A Deputy", CustomOptionHolder.rates,
            Get<Sheriff>().sheriffSpawnRate);
        deputyNumberOfHandcuffs = new CustomOption(104, "Deputy Number Of Handcuffs", 3f, 1f, 10f,
            1f, deputySpawnRate);
        deputyHandcuffCooldown =
            new CustomOption(105, "Handcuff Cooldown", 30f, 10f, 60f, 2.5f, deputySpawnRate);
        deputyHandcuffDuration =
            new CustomOption(106, "Handcuff Duration", 15f, 5f, 60f, 2.5f, deputySpawnRate);
        deputyKnowsSheriff = new CustomOption(107, "Sheriff And Deputy Know Each Other ", true,
            deputySpawnRate);
        deputyGetsPromoted = new CustomOption(108, "Deputy Gets Promoted To Sheriff",
            ["Off", "On (Immediately)", "On (After Meeting)"], deputySpawnRate);
        deputyKeepsHandcuffs = new CustomOption(109, "Deputy Keeps Handcuffs When Promoted", true,
            deputyGetsPromoted);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Deputy Handcuff
        deputyHandcuffButton = new CustomButton(
            () =>
            {
                var target = CachedPlayer.LocalPlayer.Control.Is<Sheriff>()
                    ? Get<Sheriff>().currentTarget
                    : currentTarget; // If the deputy is now the sheriff, sheriffs target, else deputies target
                Helpers.checkWatchFlash(target);
                var targetId = target.PlayerId;
                if (Helpers.checkAndDoVetKill(target)) return;
                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.DeputyUsedHandcuffs, SendOption.Reliable);
                writer.Write(targetId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.deputyUsedHandcuffs(targetId);
                currentTarget = null;
                deputyHandcuffButton.Timer = deputyHandcuffButton.MaxTimer;

                SoundEffectsManager.play("deputyHandcuff");
            },
            () => ((deputy != null && CachedPlayer.LocalPlayer.Control.Is<Deputy>()) ||
                   (CachedPlayer.LocalPlayer.Control.Is<Sheriff>() && Get<Sheriff>().formerDeputy.Is<Sheriff>() &&
                    keepsHandcuffsOnPromotion)) &&
                  !CachedPlayer.LocalPlayer.Data.IsDead,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, deputyHandcuffButton, "CUFF");
                if (deputyButtonHandcuffsText != null) deputyButtonHandcuffsText.text = $"{remainingHandcuffs}";
                return ((deputy != null && deputy == CachedPlayer.LocalPlayer.Control &&
                         currentTarget) ||
                        (CachedPlayer.LocalPlayer.Control.Is<Sheriff>() &&
                         Get<Sheriff>().formerDeputy.Is<Sheriff>() && Get<Sheriff>().currentTarget)) &&
                       remainingHandcuffs > 0 &&
                       CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () => { deputyHandcuffButton.Timer = deputyHandcuffButton.MaxTimer; },
            buttonSprite,
            CustomButton.ButtonPositions.lowerRowRight,
            _hudManager,
            KeyCode.F
        );
        // Deputy Handcuff button handcuff counter
        deputyButtonHandcuffsText = Object.Instantiate(deputyHandcuffButton.actionButton.cooldownTimerText,
            deputyHandcuffButton.actionButton.cooldownTimerText.transform.parent);
        deputyButtonHandcuffsText.text = "";
        deputyButtonHandcuffsText.enableWordWrapping = false;
        deputyButtonHandcuffsText.transform.localScale = Vector3.one * 0.5f;
        deputyButtonHandcuffsText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);
    }

    public override void ResetCustomButton()
    {
        deputyHandcuffButton.MaxTimer = handcuffCooldown;
        deputyHandcuffedButtons = new Dictionary<byte, List<CustomButton>>();
    }

    private void addReplacementHandcuffedButton(CustomButton button, Vector3? positionOffset = null,
        Func<bool> couldUse = null)
    {
        var positionOffsetValue =
            positionOffset ?? button.PositionOffset; // For non custom buttons, we can set these manually.
        positionOffsetValue.z = -0.1f;
        couldUse ??= button.CouldUse;
        var replacementHandcuffedButton = new CustomButton(() => { }, () => { return true; }, couldUse, () => { },
            handcuffedSprite, positionOffsetValue, button.hudManager, button.hotkey,
            true, handcuffDuration, () => { }, button.mirror);
        replacementHandcuffedButton.Timer = replacementHandcuffedButton.EffectDuration;
        replacementHandcuffedButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
        replacementHandcuffedButton.isEffectActive = true;
        if (deputyHandcuffedButtons.ContainsKey(CachedPlayer.LocalPlayer.PlayerId))
            deputyHandcuffedButtons[CachedPlayer.LocalPlayer.PlayerId].Add(replacementHandcuffedButton);
        else
            deputyHandcuffedButtons.Add(CachedPlayer.LocalPlayer.PlayerId,
                [replacementHandcuffedButton]);
    }

    // Disables / Enables all Buttons (except the ones disabled in the Deputy class), and replaces them with new buttons.
    public void setAllButtonsHandcuffedStatus(bool handcuffed, bool reset = false)
    {
        if (reset)
        {
            deputyHandcuffedButtons = new Dictionary<byte, List<CustomButton>>();
            return;
        }

        switch (handcuffed)
        {
            case true when !deputyHandcuffedButtons.ContainsKey(CachedPlayer.LocalPlayer.PlayerId):
            {
                var maxI = CustomButton.buttons.Count;
                for (var i = 0; i < maxI; i++)
                    try
                    {
                        if (CustomButton.buttons[i].HasButton()) // For each custombutton the player has
                            addReplacementHandcuffedButton(CustomButton
                                .buttons[i]); // The new buttons are the only non-handcuffed buttons now!
                        CustomButton.buttons[i].isHandcuffed = true;
                    }
                    catch (NullReferenceException)
                    {
                        System.Console.WriteLine(
                            "[WARNING] NullReferenceException from MeetingEndedUpdate().HasButton(), if theres only one warning its fine"); // Note: idk what this is good for, but i copied it from above /gendelo
                    }

                // Non Custom (Vanilla) Buttons. The Originals are disabled / hidden in UpdatePatch.cs already, just need to replace them. Can use any button, as we replace onclick etc anyways.
                // Kill Button if enabled for the Role
                if (FastDestroyableSingleton<HudManager>.Instance.KillButton.isActiveAndEnabled)
                    addReplacementHandcuffedButton(Get<Arsonist>().arsonistButton,
                        CustomButton.ButtonPositions.upperRowRight,
                        () => FastDestroyableSingleton<HudManager>.Instance.KillButton.currentTarget != null);
                // Vent Button if enabled
                if (CachedPlayer.LocalPlayer.Control.roleCanUseVents())
                    addReplacementHandcuffedButton(Get<Arsonist>().arsonistButton,
                        CustomButton.ButtonPositions.upperRowCenter,
                        () => FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.currentTarget != null);
                // Report Button
                addReplacementHandcuffedButton(Get<Arsonist>().arsonistButton,
                    !CachedPlayer.LocalPlayer.Data.Role.IsImpostor
                        ? new Vector3(-1f, -0.06f, 0)
                        : CustomButton.ButtonPositions.lowerRowRight,
                    () => FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.color ==
                          Palette.EnabledColor);
                break;
            }
            // Reset to original. Disables the replacements, enables the original buttons.
            case false when
                deputyHandcuffedButtons.ContainsKey(CachedPlayer.LocalPlayer
                    .PlayerId):
            {
                foreach (var replacementButton in deputyHandcuffedButtons[CachedPlayer.LocalPlayer.PlayerId])
                {
                    replacementButton.HasButton = () => { return false; };
                    replacementButton.Update(); // To make it disappear properly.
                    CustomButton.buttons.Remove(replacementButton);
                }

                deputyHandcuffedButtons.Remove(CachedPlayer.LocalPlayer.PlayerId);

                foreach (var button in CustomButton.buttons) button.isHandcuffed = false;
                break;
            }
        }
    }
}