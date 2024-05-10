using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Options;

public abstract class OptionTabMenuBase
{
    public List<CustomOption> AllOption = [];
    public OptionTab CurrentTab;
    public Vector3 defPos;
    public GameObject GameSettings;
    public GameObject GameTab;

    public bool IsDefault;
    public GameObject RoleTab;
    public List<OptionTab> OptionTabs { get; set; } = [];

    public virtual void CreateTabMenu(GameOptionsMenu __instance)
    {
        IsDefault = true;
        RoleTab = GameObject.Find("RoleTab");
        defPos = RoleTab.transform.position;
        RoleTab.SetActive(false);
        GameTab = GameObject.Find("GameTab");
        GameSettings = GameObject.Find("Game Settings");
        StringOptionTemplate ??= Object.FindObjectsOfType<StringOption>().FirstOrDefault();
        GameSettingMenu ??= Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
        GameSettingMenu!.RolesSettings.gameObject.Destroy();

        foreach (var optionTab in OptionTabs)
        {
            SetOptions(__instance, optionTab);
            optionTab.CreateTab(this);
            optionTab.SetActive(false);
        }

        UpdateOptionTab();
        SetTabPos();
    }

    public virtual void SetOptions(GameOptionsMenu __instance, OptionTab menu)
    {
    }

    public void UpdateOptionTab()
    {
        GameSettings.SetActive(IsDefault);
        GameSettingMenu!.GameSettingsHightlight.enabled = IsDefault;
        foreach (var optionTab in OptionTabs)
        {
            if (IsDefault)
            {
                optionTab.SetActive(false);
                continue;
            }

            optionTab.SetActive(optionTab == CurrentTab);
        }
    }

    public void SetTabPos()
    {
        CurrentPos = defPos + (Vector3.left * 3f);
        ;
        GameTab.transform.position += Vector3.left * 3f;
        foreach (var tab in OptionTabs)
        {
            tab.TabHighlightGameObject.transform.position = (Vector3)CurrentPos;

            CurrentPos += Vector3.right * 1f;
        }
    }

#nullable enable
    public Vector3? CurrentPos;
    public StringOption? StringOptionTemplate;
    public GameSettingMenu? GameSettingMenu;
#nullable disable
}

public sealed class OptionTab
{
    public string TabName { get; set; }

    public ResourceSprite TabSprite { get; set; }

    public Vector3 TabPos { get; set; }

    public OptionTypes OptionType { get; set; }

    public GameObject TabGameObject { get; set; }

    public GameObject TabHighlightGameObject { get; set; }

    public SpriteRenderer TabHighlightSpriteRenderer { get; set; }

#nullable enable
    public List<CustomOption>? Options { get; set; }
#nullable disable

    public void CreateTab(OptionTabMenuBase optionTabMenuBase)
    {
        TabHighlightGameObject =
            Object.Instantiate(optionTabMenuBase.RoleTab, optionTabMenuBase.RoleTab.transform.parent);
        TabHighlightSpriteRenderer = TabHighlightGameObject.transform.FindChild("Hat Button")
            .FindChild("Tab Background")
            .GetComponent<SpriteRenderer>();
        TabHighlightGameObject.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>()
            .sprite = TabSprite;
        TabHighlightGameObject.name = TabName + "Highlight";

        var button = TabHighlightGameObject.GetComponent<PassiveButton>();
        button.OnClick.RemoveAllListeners();
        button.OnClick.AddListener((Action)(() =>
        {
            optionTabMenuBase.CurrentTab = this;
            optionTabMenuBase.UpdateOptionTab();
        }));
    }

    public void SetActive(bool active)
    {
        TabGameObject.SetActive(active);
        TabHighlightSpriteRenderer.enabled = false;
    }
}

public class ClassicTabMenu : OptionTabMenuBase
{
    public readonly OptionTab TORSettings = new()
    {
        TabName = "TORSettings",
        TabSprite = new ResourceSprite("TheOtherRoles.Resources.TabIcon.png"),
        TabPos = Vector3.left * 2f
    };

    public ClassicTabMenu()
    {
        OptionTabs.Add(TORSettings);
    }
}