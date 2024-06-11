using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherUs.Modules.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Options;

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
    public abstract TabTypes TabType { get; set; }

    public virtual void CreateTabMenu(GameOptionsMenu __instance)
    {
        IsDefault = true;
        RoleTab = GameObject.Find("RoleTab");
        RoleTab.DestroyAllChildren<OptionBehaviour>();
        defPos = RoleTab.transform.position;
        RoleTab.SetActive(false);
        GameTab = GameObject.Find("GameTab");
        GameSettings = GameObject.Find("Game Settings");
        StringOptionTemplate ??= Object.FindObjectsOfType<StringOption>().FirstOrDefault();
        GameSettingMenu ??= Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
        GameSettingMenu!.RolesSettings.gameObject.SetActive(false);

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
        AllOption = CustomOptionManager.Instance.options.Where(n => n.TabType == TabType).ToList();
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

public class OptionTab
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
        var parent = optionTabMenuBase.RoleTab.transform.parent;
        TabHighlightGameObject =
            Object.Instantiate(optionTabMenuBase.RoleTab, parent);
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
        
        TabGameObject = Object.Instantiate(optionTabMenuBase.RoleTab, parent);
        TabGameObject.AddComponent<CustomOptionUpdateComponent>();
    }

    public virtual void CreateOption()
    {
        
    }

    public void SetActive(bool active)
    {
        TabGameObject.SetActive(active);
        TabHighlightSpriteRenderer.enabled = false;
    }
}

public class PresetTab : OptionTab
{
    public static readonly PresetTab Tab = new();
    public HashSet<OptionPreset> presets => CustomOptionManager.Instance.presets;
    public override void CreateOption()
    {
    }
}

public enum TabTypes
{
    Classic,
    Guesser,
    HideNSeek,
    PropHunt
}

public class ClassicTabMenu : OptionTabMenuBase
{
    public readonly OptionTab TORSettings = new()
    {
        TabName = "TORSettings",
        TabSprite = new ResourceSprite("TheOtherUs.Resources.TabIcon.png"),
        TabPos = Vector3.left * 2f
    };
    
    public readonly OptionTab ImpostorSettings = new()
    {
        TabName = "TORSettings",
        TabSprite = new ResourceSprite("TheOtherUs.Resources.TabIcon.png"),
        TabPos = Vector3.left * 2f
    };
    
    public readonly OptionTab CrewmateSettings = new()
    {
        TabName = "TORSettings",
        TabSprite = new ResourceSprite("TheOtherUs.Resources.TabIcon.png"),
        TabPos = Vector3.left * 2f
    };
    
    public readonly OptionTab NeutralSettings = new()
    {
        TabName = "TORSettings",
        TabSprite = new ResourceSprite("TheOtherUs.Resources.TabIcon.png"),
        TabPos = Vector3.left * 2f
    };

    public override TabTypes TabType { get; set; } = TabTypes.Classic;

    public override void SetOptions(GameOptionsMenu __instance, OptionTab menu)
    {
        base.SetOptions(__instance, menu);
        if (menu == TORSettings)
        {
            menu.Options = AllOption.Where(n => n is CustomGeneralOption).ToList();
        }

        var roles = AllOption.OfTypeList<CustomRoleOption>();
        if (menu == ImpostorSettings)
        {
            menu.Options = roles.Where(n => n.roleBase.RoleInfo.RoleTeam == RoleTeam.Impostor).CastList<CustomOption>();
        }
        
        if (menu == NeutralSettings)
        {
            menu.Options = roles.Where(n => n.roleBase.RoleInfo.RoleTeam == RoleTeam.Neutral).CastList<CustomOption>();
        }
        
    }

    public ClassicTabMenu()
    {
        OptionTabs.Add(TORSettings);
        OptionTabs.Add(ImpostorSettings);
        OptionTabs.Add(NeutralSettings);
        OptionTabs.Add(PresetTab.Tab);
    }
}

public class GuesserTabMenu : OptionTabMenuBase
{
    public override TabTypes TabType { get; set; } = TabTypes.Guesser;
}

public class HideNSeekTabMenu : OptionTabMenuBase
{
    public override TabTypes TabType { get; set; } = TabTypes.HideNSeek;
}

public class PropHuntTabMenu : OptionTabMenuBase
{
    public override TabTypes TabType { get; set; } = TabTypes.PropHunt;
}