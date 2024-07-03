using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TheOtherUs.Objects;

public static class DefButtonPositions
{
    public static readonly Vector3
        lowerRowRight = new(-2f, -0.06f, 0); // Not usable for imps beacuse of new button positions!

    public static readonly Vector3 lowerRowCenter = new(-3f, -0.06f, 0);
    public static readonly Vector3 lowerRowLeft = new(-4f, -0.06f, 0);
    public static readonly Vector3 lowerRowFarLeft = new(-3f, -0.06f, 0f);

    public static readonly Vector3
        upperRowRight = new(0f, 1f, 0f); // Not usable for imps beacuse of new button positions!

    public static readonly Vector3
        upperRowCenter = new(-1f, 1f, 0f); // Not usable for imps beacuse of new button positions!

    public static readonly Vector3 upperRowLeft = new(-2f, 1f, 0f);
    public static readonly Vector3 upperRowFarLeft = new(-3f, 1f, 0f);
}

public class CustomButton
{
    public static List<CustomButton> buttons = [];
    private static readonly int Desat = Shader.PropertyToID("_Desat");
    public ActionButton actionButton;
    public GameObject actionButtonGameObject;
    public TextMeshPro actionButtonLabelText;
    public Material actionButtonMat;
    public SpriteRenderer actionButtonRenderer;
    private readonly string buttonText;
    public Func<bool> CouldUse;
    public float EffectDuration;
    public Func<bool> HasButton;
    public bool HasEffect;
    public KeyCode? hotkey;
    public HudManager hudManager;
    public bool isEffectActive;
    public bool isHandcuffed = false;
    public float MaxTimer = float.MaxValue;
    public bool mirror;
    private Action OnClick;
    private Action OnEffectEnds;
    private Action OnMeetingEnds;
    public Vector3 PositionOffset;
    public bool showButtonText;
    public Sprite Sprite;
    public float Timer;

    public CustomButton(Action OnClick, Func<bool> HasButton, Func<bool> CouldUse, Action OnMeetingEnds, Sprite Sprite,
        Vector3 PositionOffset, HudManager hudManager, KeyCode? hotkey, bool HasEffect, float EffectDuration,
        Action OnEffectEnds, bool mirror = false, string buttonText = "")
    {
        this.hudManager = hudManager;
        this.HasButton = HasButton;
        this.CouldUse = CouldUse;
        this.PositionOffset = PositionOffset;
        this.HasEffect = HasEffect;
        this.EffectDuration = EffectDuration;
        this.Sprite = Sprite;
        this.mirror = mirror;
        this.hotkey = hotkey;
        this.buttonText = buttonText;
        buttons.Add(this);
        
        this.OnClick = OnClick;
        this.OnMeetingEnds = OnMeetingEnds;
        this.OnEffectEnds = OnEffectEnds;
        
        actionButton = Object.Instantiate(hudManager.KillButton, hudManager.KillButton.transform.parent);
        actionButtonGameObject = actionButton.gameObject;
        actionButtonRenderer = actionButton.graphic;
        actionButtonMat = actionButtonRenderer.material;
        actionButtonLabelText = actionButton.buttonLabelText;
        var button = actionButton.GetComponent<PassiveButton>();
        showButtonText = actionButtonRenderer.sprite == Sprite || buttonText != "";
        button.OnClick = new Button.ButtonClickedEvent();
        button.OnClick.AddListener(OnClickMethod);

        setActive(false);
    }

    public CustomButton Create()
    {
        return this;
    }

    public Func<CustomButton, bool> CheckCanClick
    {
        get;
        set;
    } = button => true;

    public void OnClickMethod()
    {
        if (!CheckCanClick(this)) return;
        
        OnClick();
    }

    public CustomButton(Action OnClick, Func<bool> HasButton, Func<bool> CouldUse, Action OnMeetingEnds, Sprite Sprite,
        Vector3 PositionOffset, HudManager hudManager, KeyCode? hotkey, bool mirror = false, string buttonText = "")
        : this(OnClick, HasButton, CouldUse, OnMeetingEnds, Sprite, PositionOffset, hudManager, hotkey, false, 0f,
            () => { }, mirror, buttonText)
    {
    }

    public void onClickEvent()
    {
        if (Timer < 0f && HasButton() && CouldUse())
        {
            actionButtonRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            OnClick();

            /*// Deputy skip onClickEvent if handcuffed
            if (Deputy.handcuffedKnows.ContainsKey(LocalPlayer.PlayerId) &&
                Deputy.handcuffedKnows[LocalPlayer.PlayerId] > 0f) return;

            if (!HasEffect || isEffectActive) return;
            DeputyTimer = EffectDuration;*/
            Timer = EffectDuration;
            actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
            isEffectActive = true;
        }
    }

    public static void HudUpdate()
    {
        buttons.RemoveAll(item => item.actionButton == null);

        foreach (var t in buttons)
            try
            {
                t.Update();
            }
            catch (NullReferenceException)
            {
                Warn(
                    "NullReferenceException from HudUpdate().HasButton(), if theres only one warning its fine");
            }
    }

    public static void MeetingEndedUpdate()
    {
        buttons.RemoveAll(item => item.actionButton == null);
        foreach (var t in buttons)
            try
            {
                t.OnMeetingEnds();
                t.Update();
            }
            catch (NullReferenceException)
            {
                Warn(
                    "NullReferenceException from MeetingEndedUpdate().HasButton(), if theres only one warning its fine");
            }
    }

    public static void ResetAllCooldowns()
    {
        foreach (var t in buttons)
            try
            {
                t.Timer = t.MaxTimer;
                t.Update();
            }
            catch (NullReferenceException)
            {
                System.Console.WriteLine(
                    "[WARNING] NullReferenceException from MeetingEndedUpdate().HasButton(), if theres only one warning its fine");
            }
    }

    public void setActive(bool isActive)
    {
        switch (isActive)
        {
            case true:
                actionButton.SetEnabled();
                actionButton.Show();
                break;
            default:
                actionButton.SetDisabled();
                actionButton.Hide();
                break;
        }
    }

    public void Update()
    {
        var localPlayer = LocalPlayer;
        var moveable = localPlayer.Control.moveable;

        if (MeetingHud.Instance || ExileController.Instance || !HasButton())
        {
            setActive(false);
            return;
        }

        setActive(hudManager.UseButton.isActiveAndEnabled || hudManager.PetButton.isActiveAndEnabled);

        /*if (DeputyTimer >= 0)
        {
            // This had to be reordered, so that the handcuffs do not stop the underlying timers from running
            if (HasEffect && isEffectActive)
                DeputyTimer -= Time.deltaTime;
            else if (!localPlayer.Control.inVent && moveable)
                DeputyTimer -= Time.deltaTime;
        }

        if (DeputyTimer <= 0 && HasEffect && isEffectActive)
        {
            isEffectActive = false;
            actionButton.cooldownTimerText.color = Palette.EnabledColor;
            OnEffectEnds();
        }*/

        if (isHandcuffed)
        {
            setActive(false);
            return;
        }

        actionButtonRenderer.sprite = Sprite;
        if (showButtonText && buttonText != "") actionButton.OverrideText(buttonText);
        actionButtonLabelText.enabled = showButtonText; // Only show the text if it's a kill button
        if (hudManager.UseButton != null)
        {
            var pos = hudManager.UseButton.transform.localPosition;
            if (mirror)
            {
                var aspect = Camera.main.aspect;
                var safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
                var xpos = 0.05f - (safeOrthographicSize * aspect * 1.70f);
                pos = new Vector3(xpos, pos.y, pos.z);
            }

            actionButton.transform.localPosition = pos + PositionOffset;
        }

        if (CouldUse())
        {
            actionButton.SetEnabled();
        }
        else
        {
            actionButton.SetDisabled();
        }

        if (Timer >= 0)
        {
            if (HasEffect && isEffectActive)
                Timer -= Time.deltaTime;
            else if (!localPlayer.Control.inVent && moveable)
                Timer -= Time.deltaTime;
        }

        if (Timer <= 0 && HasEffect && isEffectActive)
        {
            isEffectActive = false;
            actionButton.cooldownTimerText.color = Palette.EnabledColor;
            OnEffectEnds();
        }

        actionButton.SetCoolDown(Timer, HasEffect && isEffectActive ? EffectDuration : MaxTimer);

        // Trigger OnClickEvent if the hotkey is being pressed down
        if (hotkey.HasValue && Input.GetKeyDown(hotkey.Value)) onClickEvent();

        /*
        // Deputy disable the button and display Handcuffs instead...
        if (Deputy.handcuffedPlayers.Contains(localPlayer.PlayerId))
            OnClick = () => { Deputy.setHandcuffedKnows(); };
        else // Reset.
            OnClick = InitialOnClick;*/
    }
}