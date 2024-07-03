using System;
using System.Collections.Generic;
using Hazel;
using TheOtherUs.Objects;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Deputy : RoleBase
{
    public class DeputyController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Deputy>();
    }
    
    
    private readonly ResourceSprite buttonSprite = new("DeputyHandcuffButton.png");
    
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

    public float handcuffCooldown;
    public float handcuffDuration;
    public Dictionary<byte, float> handcuffedKnows = new();
    public List<byte> handcuffedPlayers = [];
    private readonly ResourceSprite handcuffedSprite = new("DeputyHandcuffed.png");
    public bool keepsHandcuffsOnPromotion;
    public bool knowsSheriff;
    public int promotesToSheriff; // No: 0, Immediately: 1, After Meeting: 2
    public float remainingHandcuffs;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(248, 205, 70, byte.MaxValue),
        GetRole = Get<Deputy>,
        CreateRoleController = n => new DeputyController(n),
        DescriptionText = "Handcuff the Impostors",
        IntroInfo = "Handcuff the <color=#FF1919FF>Impostors</color>",
        Name = nameof(Deputy),
        RoleClassType = typeof(Deputy),
        RoleId = RoleId.Deputy,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };

    public override CustomRoleOption roleOption { get; set; }

    // Can be used to enable / disable the handcuff effect on the target's buttons
    public void setHandcuffedKnows(bool active = true, byte playerId = byte.MaxValue)
    {
        if (playerId == byte.MaxValue)
            playerId = LocalPlayer.PlayerId;

        if (active && playerId == LocalPlayer.PlayerId)
        {
            /*var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
            writer.Write(LocalPlayer.PlayerId);
            writer.Write((byte)RPCProcedure.GhostInfoTypes.HandcuffNoticed);
            AmongUsClient.Instance.FinishRpcImmediately(writer);*/
        }

        if (active)
        {
            handcuffedKnows.Add(playerId, handcuffDuration);
            handcuffedPlayers.RemoveAll(x => x == playerId);
        }

        if (playerId != LocalPlayer.PlayerId) return;
        setAllButtonsHandcuffedStatus(active);
        SoundEffectsManager.play("deputyHandcuff");
    }
    
    [RPCListener(CustomRPC.DeputyPromotes)]
    public static void deputyPromotes(MessageReader reader)
    {
        if (!RoleIsAlive<Deputy>()) return;
    }

    public override void ClearAndReload()
    {
        deputy = null;
        currentTarget = null;
        handcuffedPlayers = [];
        handcuffedKnows = new Dictionary<byte, float>();
        setAllButtonsHandcuffedStatus(false, true);
        promotesToSheriff = deputyGetsPromoted.Selection;
        remainingHandcuffs = deputyNumberOfHandcuffs;
        handcuffCooldown = deputyHandcuffCooldown;
        keepsHandcuffsOnPromotion = deputyKeepsHandcuffs;
        handcuffDuration = deputyHandcuffDuration;
        knowsSheriff = deputyKnowsSheriff;
    }

    public override bool EnableAssign { get; set; } = false;

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this, enableRate:false);
        
        deputyNumberOfHandcuffs = new CustomOption("Deputy Number Of Handcuffs", roleOption, new IntOptionSelection(3, 1,10, 1));
        deputyHandcuffCooldown =
            new CustomOption("Handcuff Cooldown", roleOption, new FloatOptionSelection(30, 10, 60, 2.5f));
        deputyHandcuffDuration = new CustomOption("Handcuff Duration", roleOption, new FloatOptionSelection(15, 5, 60, 2.5f));
        deputyKnowsSheriff = new CustomOption("Sheriff And Deputy Know Each Other ", roleOption, new BoolOptionSelection());;
        deputyGetsPromoted = new CustomOption( "Deputy Gets Promoted To Sheriff", roleOption, new StringOptionSelection(
            ["Off", "On (Immediately)", "On (After Meeting)"]));
        deputyKeepsHandcuffs = new CustomOption("Deputy Keeps Handcuffs When Promoted", roleOption, new BoolOptionSelection());
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Deputy Handcuff
        deputyHandcuffButton = new CustomButton(
            () =>
            {
                var target = LocalPlayer.Control.Is<Sheriff>()
                    ? Get<Sheriff>().currentTarget
                    : currentTarget; // If the deputy is now the sheriff, sheriffs target, else deputies target
                /*Helpers.checkWatchFlash(target);*/
                var targetId = target.PlayerId;
                /*if (Helpers.checkAndDoVetKill(target)) return;*/
                var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.DeputyUsedHandcuffs, SendOption.Reliable);
                writer.Write(targetId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                /*RPCProcedure.deputyUsedHandcuffs(targetId);*/
                currentTarget = null;
                deputyHandcuffButton.Timer = deputyHandcuffButton.MaxTimer;

                SoundEffectsManager.play("deputyHandcuff");
            },
            () => ((deputy != null && LocalPlayer.Control.Is<Deputy>()) ||
                   (LocalPlayer.Control.Is<Sheriff>() && Get<Sheriff>().formerDeputy.Is<Sheriff>() &&
                    keepsHandcuffsOnPromotion)) &&
                  !LocalPlayer.NetPlayerInfo.IsDead,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, deputyHandcuffButton, "CUFF");
                if (deputyButtonHandcuffsText != null) deputyButtonHandcuffsText.text = $"{remainingHandcuffs}";
                return ((deputy != null && deputy == LocalPlayer.Control &&
                         currentTarget) ||
                        (LocalPlayer.Control.Is<Sheriff>() &&
                         Get<Sheriff>().formerDeputy.Is<Sheriff>() && Get<Sheriff>().currentTarget)) &&
                       remainingHandcuffs > 0 &&
                       LocalPlayer.Control.CanMove;
            },
            () => { deputyHandcuffButton.Timer = deputyHandcuffButton.MaxTimer; },
            buttonSprite,
            DefButtonPositions.lowerRowRight,
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
        if (deputyHandcuffedButtons.ContainsKey(LocalPlayer.PlayerId))
            deputyHandcuffedButtons[LocalPlayer.PlayerId].Add(replacementHandcuffedButton);
        else
            deputyHandcuffedButtons.Add(LocalPlayer.PlayerId,
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
            case true when !deputyHandcuffedButtons.ContainsKey(LocalPlayer.PlayerId):
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
                        DefButtonPositions.upperRowRight,
                        () => FastDestroyableSingleton<HudManager>.Instance.KillButton.currentTarget != null);
                // Vent Button if enabled
                if (LocalPlayer.CanUseVent())
                    addReplacementHandcuffedButton(Get<Arsonist>().arsonistButton,
                        DefButtonPositions.upperRowCenter,
                        () => FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.currentTarget != null);
                // Report Button
                addReplacementHandcuffedButton(Get<Arsonist>().arsonistButton,
                    !LocalPlayer.NetPlayerInfo.Role.IsImpostor
                        ? new Vector3(-1f, -0.06f, 0)
                        : DefButtonPositions.lowerRowRight,
                    () => FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.color ==
                          Palette.EnabledColor);
                break;
            }
            // Reset to original. Disables the replacements, enables the original buttons.
            case false when
                deputyHandcuffedButtons.ContainsKey(LocalPlayer
                    .PlayerId):
            {
                foreach (var replacementButton in deputyHandcuffedButtons[LocalPlayer.PlayerId])
                {
                    replacementButton.HasButton = () => { return false; };
                    replacementButton.Update(); // To make it disappear properly.
                    CustomButton.buttons.Remove(replacementButton);
                }

                deputyHandcuffedButtons.Remove(LocalPlayer.PlayerId);

                foreach (var button in CustomButton.buttons) button.isHandcuffed = false;
                break;
            }
        }
    }
}