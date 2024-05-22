using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AmongUs.GameOptions;
using Hazel;
using Reactor.Utilities.Extensions;
using TheOtherUs.Helper;
using TheOtherUs.Modules.Components;
using TheOtherUs.Roles;
using TheOtherUs.Roles.Crewmate;
using TheOtherUs.Roles.Modifier;
using TheOtherUs.Roles.Neutral;
using TheOtherUs.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Color = UnityEngine.Color;

namespace TheOtherUs.Options;

public enum OptionTypes
{
    General,
    Role,
    Mode
}

public class CustomRoleOption(RoleBase @base, string Title)
    : CustomParentOption(Title, OptionTypes.Role, new StringOptionSelection(0, roleRateStrings), color:@base.RoleInfo.Color)
{
    public static readonly string[] roleRateStrings = ["0%","10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"];
    public int Rate => 10 * OptionSelection.Selection;
    public RoleBase roleBase = @base;
}

public class CustomModeOption(CustomGameModes mode, string Title, OptionSelectionBase selection, Color color = default)
    : CustomParentOption(Title, OptionTypes.Mode, selection, color)
{
    public CustomGameModes mode = mode;
}

public class CustomParentOption(string Title, OptionTypes type, OptionSelectionBase selection, Color color)
    : CustomOption(Title, type, selection, default, color)
{
    public override void Create(OptionTabMenuBase optionTabMenuBase)
    {
        base.Create(optionTabMenuBase);
    }
}

public class CustomOption
{
    [JsonIgnore] 
    #nullable enable
    public OptionBehaviour? optionBehaviour;
    #nullable disable
    
    [JsonIgnore] 
    public Color Color { get; set; } = Color.white;

    [JsonIgnore] public int Selection => OptionSelection.Selection;

    public virtual void Create(OptionTabMenuBase optionTabMenuBase, Transform Parent)
    {
        var stringOption = Object.Instantiate(optionTabMenuBase.StringOptionTemplate, Parent);
        stringOption.OnValueChanged = new Action<OptionBehaviour>(o => { });
        if (EnabledTranslate)
        {
            stringOption.TitleText.Destroy();
            stringOption.TitleText = stringOption.gameObject.AddComponent<TranslateText>();
            var TitleText = (TranslateText)stringOption.TitleText;
            TitleText.Id = TranslateId;
            
            
            stringOption.ValueText.Destroy();
            stringOption.ValueText = stringOption.gameObject.AddComponent<TranslateText>();
            var ValueText = (TranslateText)stringOption.ValueText;
            ValueText.Id = OptionSelection.translateId;
        }
        stringOption.TitleText.text = Title;
        stringOption.Value = stringOption.oldValue = OptionSelection.Selection;
        stringOption.ValueText.text = OptionSelection;

        optionBehaviour = stringOption;
    }


    public CustomOption(string Title, OptionTypes type, OptionSelectionBase selection, CustomOption Parent, Color color = default)
    {
        optionType = type;
        selection.option = this;
        optionInfo = new OptionInfo
        {
            Title = Title,
            option = selection.option = this
        };

        if (Parent != null)
        {
            optionInfo.Parent = Parent.optionInfo;
            Color = Parent.Color;
            Parent.optionInfo.Children.Add(optionInfo);
        }

        if (color != default)
            Color = color;

        CustomOptionManager.Instance.Register(this);
    }

    public OptionTypes optionType { get; set; }

    [JsonIgnore] 
    public OptionEvent optionEvent { get; } = new();

    public OptionSelection OptionSelection { get; set; }
    public OptionInfo optionInfo { get; set; }

    /// <summary>
    ///     启用
    /// </summary>
    [JsonIgnore]
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     启用翻译
    /// </summary>
    [JsonIgnore]
    public bool EnabledTranslate { get; set; } = false;


    /// <summary>
    ///     翻译Id
    /// </summary>
    [JsonIgnore]
    public string TranslateId { get; set; } = string.Empty;

    public void ShareOptionChange()
    {
        FastRpcWriter.StartNewRpcWriter(CustomRPC.Option, GameData.Instance)
            .WritePacked((int)Option_Flag.Share)
            .WritePacked(optionInfo.Id)
            .Write(OptionSelection.Selection)
            .RPCSend();
    }

    public string Title => EnabledTranslate ? optionInfo.Title.Translate() : optionInfo.Title;

    // Getter
    public static implicit operator bool(CustomOption option)
    {
        return option.OptionSelection;
    }

    public static implicit operator float(CustomOption option)
    {
        return option.OptionSelection;
    }

    public static implicit operator int(CustomOption option)
    {
        return option.OptionSelection;
    }

    public static implicit operator string(CustomOption option)
    {
        return option.OptionSelection;
    }

    public void Serialize(FastRpcWriter writer)
    {
        writer.Write(JsonSerializer.Serialize(OptionSelection));
        writer.Write(JsonSerializer.Serialize(optionInfo));
    }

    public void Deserialize(MessageReader reader)
    {
        var selectionString = reader.ReadString();
        OptionSelection = JsonSerializer.Deserialize<OptionSelection>(selectionString);
        OptionSelection.InitFormJson();

        var infoString = reader.ReadString();
        optionInfo = JsonSerializer.Deserialize<OptionInfo>(infoString);
        optionInfo.InitFormId();
    }

    internal enum Option_Flag
    {
        Share,
        ShareAll,
        ShareAllSelection
    }
}

[HarmonyPatch(typeof(GameOptionsMenu))]
internal static class GameOptionsMenuStartPatch
{
    public static OptionTabMenuBase optionTabMenu;
    
    [HarmonyPatch(nameof(GameOptionsMenu.Start)), HarmonyPostfix]
    private static void GameOptionMenuStartPatch(GameOptionsMenu __instance)
    {
        optionTabMenu = MapOptions.gameMode switch
        {
            CustomGameModes.Classic => new ClassicTabMenu(),
            CustomGameModes.Guesser => new GuesserTabMenu(),
            CustomGameModes.HideNSeek => new HideNSeekTabMenu(),
            CustomGameModes.PropHunt => new PropHuntTabMenu(),
            _ => new ClassicTabMenu()
        };
        
        optionTabMenu.CreateTabMenu(__instance);
    }
    public static void Postfix(GameOptionsMenu __instance)
    {
        
        // create copy to clipboard and paste from clipboard buttons.
        var template = GameObject.Find("CloseButton");
        var copyButton = Object.Instantiate(template, template.transform.parent);
        copyButton.transform.localPosition += Vector3.down * 0.8f;
        var copyButtonPassive = copyButton.GetComponent<PassiveButton>();
        var copyButtonRenderer = copyButton.GetComponent<SpriteRenderer>();
        copyButtonRenderer.sprite = UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.CopyButton.png", 175f);
        copyButtonPassive.OnClick.RemoveAllListeners();
        copyButtonPassive.OnClick = new Button.ButtonClickedEvent();
        copyButtonPassive.OnClick.AddListener((Action)(() =>
        {
            CustomOptionManager.Instance.copyToClipboard();
            copyButtonRenderer.color = Color.green;
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>(p =>
            {
                if (p > 0.95)
                    copyButtonRenderer.color = Color.white;
            })));
        }));
        var pasteButton = Object.Instantiate(template, template.transform.parent);
        pasteButton.transform.localPosition += Vector3.down * 1.6f;
        var pasteButtonPassive = pasteButton.GetComponent<PassiveButton>();
        var pasteButtonRenderer = pasteButton.GetComponent<SpriteRenderer>();
        pasteButtonRenderer.sprite = UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.PasteButton.png", 175f);
        pasteButtonPassive.OnClick.RemoveAllListeners();
        pasteButtonPassive.OnClick = new Button.ButtonClickedEvent();
        pasteButtonPassive.OnClick.AddListener((Action)(() =>
        {
            pasteButtonRenderer.color = Color.yellow;
            var success = CustomOptionManager.Instance.pasteFromClipboard();
            pasteButtonRenderer.color = success ? Color.green : Color.red;
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>(p =>
            {
                if (p > 0.95)
                    pasteButtonRenderer.color = Color.white;
            })));
        }));
    }

    private static void createClassicTabs(GameOptionsMenu __instance)
    {
        var isReturn = setNames(
            new Dictionary<string, string>
            {
                ["TORSettings"] = "The Other Us Settings",
                ["ImpostorSettings"] = "Impostor Roles Settings",
                ["NeutralSettings"] = "Neutral Roles Settings",
                ["CrewmateSettings"] = "Crewmate Roles Settings",
                ["ModifierSettings"] = "Modifier Settings"
            });

        if (isReturn) return;

        // Setup TOR tab
        var template = Object.FindObjectsOfType<StringOption>().FirstOrDefault();
        if (template == null) return;
        var gameSettings = GameObject.Find("Game Settings");
        var gameSettingMenu = Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

        var torSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var torMenu = getMenu(torSettings, "TORSettings");

        var impostorSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var impostorMenu = getMenu(impostorSettings, "ImpostorSettings");

        var neutralSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var neutralMenu = getMenu(neutralSettings, "NeutralSettings");

        var crewmateSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var crewmateMenu = getMenu(crewmateSettings, "CrewmateSettings");

        var modifierSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var modifierMenu = getMenu(modifierSettings, "ModifierSettings");

        var roleTab = GameObject.Find("RoleTab");
        var gameTab = GameObject.Find("GameTab");

        var torTab = Object.Instantiate(roleTab, roleTab.transform.parent);
        var torTabHighlight = getTabHighlight(torTab, "TheOtherRolesTab", "TheOtherUs.Resources.TabIcon.png");

        var impostorTab = Object.Instantiate(roleTab, torTab.transform);
        var impostorTabHighlight =
            getTabHighlight(impostorTab, "ImpostorTab", "TheOtherUs.Resources.TabIconImpostor.png");

        var neutralTab = Object.Instantiate(roleTab, impostorTab.transform);
        var neutralTabHighlight =
            getTabHighlight(neutralTab, "NeutralTab", "TheOtherUs.Resources.TabIconNeutral.png");

        var crewmateTab = Object.Instantiate(roleTab, neutralTab.transform);
        var crewmateTabHighlight =
            getTabHighlight(crewmateTab, "CrewmateTab", "TheOtherUs.Resources.TabIconCrewmate.png");

        var modifierTab = Object.Instantiate(roleTab, crewmateTab.transform);
        var modifierTabHighlight =
            getTabHighlight(modifierTab, "ModifierTab", "TheOtherUs.Resources.TabIconModifier.png");

        // Position of Tab Icons
        gameTab.transform.position += Vector3.left * 3f;
        roleTab.transform.position += Vector3.left * 3f;
        torTab.transform.position += Vector3.left * 2f;
        impostorTab.transform.localPosition = Vector3.right * 1f;
        neutralTab.transform.localPosition = Vector3.right * 1f;
        crewmateTab.transform.localPosition = Vector3.right * 1f;
        modifierTab.transform.localPosition = Vector3.right * 1f;

        var tabs = new[] { gameTab, roleTab, torTab, impostorTab, neutralTab, crewmateTab, modifierTab };
        var settingsHighlightMap = new Dictionary<GameObject, SpriteRenderer>
        {
            [gameSettingMenu.RegularGameSettings] = gameSettingMenu.GameSettingsHightlight,
            [gameSettingMenu.RolesSettings.gameObject] = gameSettingMenu.RolesSettingsHightlight,
            [torSettings.gameObject] = torTabHighlight,
            [impostorSettings.gameObject] = impostorTabHighlight,
            [neutralSettings.gameObject] = neutralTabHighlight,
            [crewmateSettings.gameObject] = crewmateTabHighlight,
            [modifierSettings.gameObject] = modifierTabHighlight
        };
        for (var i = 0; i < tabs.Length; i++)
        {
            var button = tabs[i].GetComponentInChildren<PassiveButton>();
            if (button == null) continue;
            var copiedIndex = i;
            button.OnClick = new Button.ButtonClickedEvent();
            button.OnClick.AddListener((Action)(() => { setListener(settingsHighlightMap, copiedIndex); }));
        }

        destroyOptions([
            torMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            impostorMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            neutralMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            crewmateMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            modifierMenu.GetComponentsInChildren<OptionBehaviour>().ToList()
        ]);

        List<OptionBehaviour> torOptions = [];
        List<OptionBehaviour> impostorOptions = [];
        List<OptionBehaviour> neutralOptions = [];
        List<OptionBehaviour> crewmateOptions = [];
        List<OptionBehaviour> modifierOptions = [];


        List<Transform> menus =
        [
            torMenu.transform, impostorMenu.transform, neutralMenu.transform, crewmateMenu.transform,
            modifierMenu.transform
        ];
        List<List<OptionBehaviour>> optionBehaviours =
            [torOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions];

        for (var i = 0; i < CustomOption.options.Count; i++)
        {
            CustomOption option = CustomOption.options[i];
            if ((int)option.type > 4) continue;
            if (option.optionBehaviour == null)
            {
                var stringOption = Object.Instantiate(template, menus[(int)option.type]);
                optionBehaviours[(int)option.type].Add(stringOption);
                stringOption.OnValueChanged = new Action<OptionBehaviour>(o => { });
                stringOption.TitleText.text = option.name;
                stringOption.Value = stringOption.oldValue = option.OptionSelection;
                stringOption.ValueText.text = option.selections[option.OptionSelection].ToString();

                option.optionBehaviour = stringOption;
            }

            option.optionBehaviour.gameObject.SetActive(true);
        }

        setOptions(
            [torMenu, impostorMenu, neutralMenu, crewmateMenu, modifierMenu],
            [torOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions],
            [torSettings, impostorSettings, neutralSettings, crewmateSettings, modifierSettings]
        );

        adaptTaskCount(__instance);
    }

    private static void createGuesserTabs(GameOptionsMenu __instance)
    {
        var isReturn = setNames(
            new Dictionary<string, string>
            {
                ["TORSettings"] = "The Other Us Settings",
                ["GuesserSettings"] = "Guesser Mode Settings",
                ["ImpostorSettings"] = "Impostor Roles Settings",
                ["NeutralSettings"] = "Neutral Roles Settings",
                ["CrewmateSettings"] = "Crewmate Roles Settings",
                ["ModifierSettings"] = "Modifier Settings"
            });

        if (isReturn) return;

        // Setup TOR tab
        var template = Object.FindObjectsOfType<StringOption>().FirstOrDefault();
        if (template == null) return;
        var gameSettings = GameObject.Find("Game Settings");
        var gameSettingMenu = Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

        var torSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var torMenu = getMenu(torSettings, "TORSettings");

        var guesserSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var guesserMenu = getMenu(guesserSettings, "GuesserSettings");

        var impostorSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var impostorMenu = getMenu(impostorSettings, "ImpostorSettings");

        var neutralSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var neutralMenu = getMenu(neutralSettings, "NeutralSettings");

        var crewmateSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var crewmateMenu = getMenu(crewmateSettings, "CrewmateSettings");

        var modifierSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var modifierMenu = getMenu(modifierSettings, "ModifierSettings");

        var roleTab = GameObject.Find("RoleTab");
        var gameTab = GameObject.Find("GameTab");

        var torTab = Object.Instantiate(roleTab, gameTab.transform.parent);
        var torTabHighlight = getTabHighlight(torTab, "TheOtherRolesTab", "TheOtherUs.Resources.TabIcon.png");

        var guesserTab = Object.Instantiate(roleTab, torTab.transform);
        var guesserTabHighlight =
            getTabHighlight(guesserTab, "GuesserTab", "TheOtherUs.Resources.TabIconGuesserSettings.png");

        var impostorTab = Object.Instantiate(roleTab, guesserTab.transform);
        var impostorTabHighlight =
            getTabHighlight(impostorTab, "ImpostorTab", "TheOtherUs.Resources.TabIconImpostor.png");

        var neutralTab = Object.Instantiate(roleTab, impostorTab.transform);
        var neutralTabHighlight =
            getTabHighlight(neutralTab, "NeutralTab", "TheOtherUs.Resources.TabIconNeutral.png");

        var crewmateTab = Object.Instantiate(roleTab, neutralTab.transform);
        var crewmateTabHighlight =
            getTabHighlight(crewmateTab, "CrewmateTab", "TheOtherUs.Resources.TabIconCrewmate.png");

        var modifierTab = Object.Instantiate(roleTab, crewmateTab.transform);
        var modifierTabHighlight =
            getTabHighlight(modifierTab, "ModifierTab", "TheOtherUs.Resources.TabIconModifier.png");

        roleTab.active = false;
        // Position of Tab Icons
        gameTab.transform.position += Vector3.left * 3f;
        torTab.transform.position += Vector3.left * 3f;
        guesserTab.transform.localPosition = Vector3.right * 1f;
        impostorTab.transform.localPosition = Vector3.right * 1f;
        neutralTab.transform.localPosition = Vector3.right * 1f;
        crewmateTab.transform.localPosition = Vector3.right * 1f;
        modifierTab.transform.localPosition = Vector3.right * 1f;

        var tabs = new[] { gameTab, torTab, impostorTab, neutralTab, crewmateTab, modifierTab, guesserTab };
        var settingsHighlightMap = new Dictionary<GameObject, SpriteRenderer>
        {
            [gameSettingMenu.RegularGameSettings] = gameSettingMenu.GameSettingsHightlight,
            [torSettings.gameObject] = torTabHighlight,
            [impostorSettings.gameObject] = impostorTabHighlight,
            [neutralSettings.gameObject] = neutralTabHighlight,
            [crewmateSettings.gameObject] = crewmateTabHighlight,
            [modifierSettings.gameObject] = modifierTabHighlight,
            [guesserSettings.gameObject] = guesserTabHighlight
        };
        for (var i = 0; i < tabs.Length; i++)
        {
            var button = tabs[i].GetComponentInChildren<PassiveButton>();
            if (button == null) continue;
            var copiedIndex = i;
            button.OnClick = new Button.ButtonClickedEvent();
            button.OnClick.AddListener((Action)(() => { setListener(settingsHighlightMap, copiedIndex); }));
        }

        destroyOptions([
            torMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            guesserMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            impostorMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            neutralMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            crewmateMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            modifierMenu.GetComponentsInChildren<OptionBehaviour>().ToList()
        ]);

        List<OptionBehaviour> torOptions = [];
        List<OptionBehaviour> guesserOptions = [];
        List<OptionBehaviour> impostorOptions = [];
        List<OptionBehaviour> neutralOptions = [];
        List<OptionBehaviour> crewmateOptions = [];
        List<OptionBehaviour> modifierOptions = [];


        List<Transform> menus =
        [
            torMenu.transform, impostorMenu.transform, neutralMenu.transform, crewmateMenu.transform,
            modifierMenu.transform, guesserMenu.transform
        ];
        List<List<OptionBehaviour>> optionBehaviours =
            [torOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions, guesserOptions];
        List<int> exludedIds = [310, 311, 312, 313, 314, 315, 316, 317, 318];

        for (var i = 0; i < CustomOption.options.Count; i++)
        {
            CustomOption option = CustomOption.options[i];
            if (exludedIds.Contains(option.id)) continue;
            if ((int)option.type > 5) continue;
            if (option.optionBehaviour == null)
            {
                var stringOption = Object.Instantiate(template, menus[(int)option.type]);
                optionBehaviours[(int)option.type].Add(stringOption);
                stringOption.OnValueChanged = new Action<OptionBehaviour>(o => { });
                stringOption.TitleText.text = option.name;
                stringOption.Value = stringOption.oldValue = option.OptionSelection;
                stringOption.ValueText.text = option.selections[option.OptionSelection].ToString();

                option.optionBehaviour = stringOption;
            }

            option.optionBehaviour.gameObject.SetActive(true);
        }

        setOptions(
            [torMenu, impostorMenu, neutralMenu, crewmateMenu, modifierMenu, guesserMenu],
            [torOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions, guesserOptions],
            [torSettings, impostorSettings, neutralSettings, crewmateSettings, modifierSettings, guesserSettings]
        );
    }

    private static void createHideNSeekTabs(GameOptionsMenu __instance)
    {
        var isReturn = setNames(
            new Dictionary<string, string>
            {
                ["TORSettings"] = "The Other Us Settings",
                ["HideNSeekSettings"] = "Hide 'N Seek Settings"
            });

        if (isReturn) return;

        // Setup TOR tab
        var template = Object.FindObjectsOfType<StringOption>().FirstOrDefault();
        if (template == null) return;
        var gameSettings = GameObject.Find("Game Settings");
        var gameSettingMenu = Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

        var torSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var torMenu = getMenu(torSettings, "TORSettings");

        var hideNSeekSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var hideNSeekMenu = getMenu(hideNSeekSettings, "HideNSeekSettings");

        var roleTab = GameObject.Find("RoleTab");
        var gameTab = GameObject.Find("GameTab");

        var torTab = Object.Instantiate(roleTab, gameTab.transform.parent);
        var torTabHighlight = getTabHighlight(torTab, "TheOtherRolesTab",
            "TheOtherUs.Resources.TabIconHideNSeekSettings.png");

        var hideNSeekTab = Object.Instantiate(roleTab, torTab.transform);
        var hideNSeekTabHighlight = getTabHighlight(hideNSeekTab, "HideNSeekTab",
            "TheOtherUs.Resources.TabIconHideNSeekRoles.png");

        roleTab.active = false;
        gameTab.active = false;

        // Position of Tab Icons
        torTab.transform.position += Vector3.left * 3f;
        hideNSeekTab.transform.position += Vector3.right * 1f;

        var tabs = new[] { torTab, hideNSeekTab };
        var settingsHighlightMap = new Dictionary<GameObject, SpriteRenderer>
        {
            [torSettings.gameObject] = torTabHighlight,
            [hideNSeekSettings.gameObject] = hideNSeekTabHighlight
        };
        for (var i = 0; i < tabs.Length; i++)
        {
            var button = tabs[i].GetComponentInChildren<PassiveButton>();
            if (button == null) continue;
            var copiedIndex = i;
            button.OnClick = new Button.ButtonClickedEvent();
            button.OnClick.AddListener((Action)(() => { setListener(settingsHighlightMap, copiedIndex); }));
        }

        destroyOptions([
            torMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
            hideNSeekMenu.GetComponentsInChildren<OptionBehaviour>().ToList()
        ]);

        List<OptionBehaviour> torOptions = [];
        List<OptionBehaviour> hideNSeekOptions = [];

        List<Transform> menus = [torMenu.transform, hideNSeekMenu.transform];
        List<List<OptionBehaviour>> optionBehaviours = [torOptions, hideNSeekOptions];

        for (var i = 0; i < CustomOption.options.Count; i++)
        {
            CustomOption option = CustomOption.options[i];
            if (option.type != CustomOptionType.HideNSeekMain &&
                option.type != CustomOptionType.HideNSeekRoles) continue;
            if (option.optionBehaviour == null)
            {
                var index = (int)option.type - 6;
                var stringOption = Object.Instantiate(template, menus[index]);
                optionBehaviours[index].Add(stringOption);
                stringOption.OnValueChanged = new Action<OptionBehaviour>(o => { });
                stringOption.TitleText.text = option.name;
                stringOption.Value = stringOption.oldValue = option.OptionSelection;
                stringOption.ValueText.text = option.selections[option.OptionSelection].ToString();

                option.optionBehaviour = stringOption;
            }

            option.optionBehaviour.gameObject.SetActive(true);
        }

        setOptions(
            [torMenu, hideNSeekMenu],
            [torOptions, hideNSeekOptions],
            [torSettings, hideNSeekSettings]
        );

        torSettings.gameObject.SetActive(true);
        torTabHighlight.enabled = true;
        gameSettingMenu.RegularGameSettings.SetActive(false);
        gameSettingMenu.GameSettingsHightlight.enabled = false;
    }


    private static void createPropHuntTabs(GameOptionsMenu __instance)
    {
        var isReturn = setNames(
            new Dictionary<string, string>
            {
                ["TORSettings"] = "Prop Hunt Settings"
            });

        if (isReturn) return;

        // Setup TOR tab
        var template = Object.FindObjectsOfType<StringOption>().FirstOrDefault();
        if (template == null) return;
        var gameSettings = GameObject.Find("Game Settings");
        var gameSettingMenu = Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

        var torSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
        var torMenu = getMenu(torSettings, "TORSettings");
        var roleTab = GameObject.Find("RoleTab");
        var gameTab = GameObject.Find("GameTab");

        var torTab = Object.Instantiate(roleTab, gameTab.transform.parent);
        var torTabHighlight = getTabHighlight(torTab, "TheOtherRolesTab",
            "TheOtherUs.Resources.TabIconPropHuntSettings.png");

        roleTab.active = false;
        gameTab.active = false;

        // Position of Tab Icons
        torTab.transform.position += Vector3.left * 3f;

        var tabs = new[] { torTab };
        var settingsHighlightMap = new Dictionary<GameObject, SpriteRenderer>
        {
            [torSettings.gameObject] = torTabHighlight
        };
        for (var i = 0; i < tabs.Length; i++)
        {
            var button = tabs[i].GetComponentInChildren<PassiveButton>();
            if (button == null) continue;
            var copiedIndex = i;
            button.OnClick = new Button.ButtonClickedEvent();
            button.OnClick.AddListener((Action)(() => { setListener(settingsHighlightMap, copiedIndex); }));
        }

        destroyOptions([
            torMenu.GetComponentsInChildren<OptionBehaviour>().ToList()
        ]);

        List<OptionBehaviour> torOptions = [];

        List<Transform> menus = [torMenu.transform];
        List<List<OptionBehaviour>> optionBehaviours = [torOptions];

        for (var i = 0; i < CustomOption.options.Count; i++)
        {
            CustomOption option = CustomOption.options[i];
            if (option.type != CustomOptionType.PropHunt) continue;
            if (option.optionBehaviour == null)
            {
                var index = 0;
                var stringOption = Object.Instantiate(template, menus[index]);
                optionBehaviours[index].Add(stringOption);
                stringOption.OnValueChanged = new Action<OptionBehaviour>(o => { });
                stringOption.TitleText.text = option.name;
                stringOption.Value = stringOption.oldValue = option.OptionSelection;
                stringOption.ValueText.text = option.selections[option.OptionSelection].ToString();

                option.optionBehaviour = stringOption;
            }

            option.optionBehaviour.gameObject.SetActive(true);
        }

        setOptions(
            [torMenu],
            [torOptions],
            [torSettings]
        );

        torSettings.gameObject.SetActive(true);
        torTabHighlight.enabled = true;
        gameSettingMenu.RegularGameSettings.SetActive(false);
        gameSettingMenu.GameSettingsHightlight.enabled = false;
    }
    

    private static void destroyOptions(List<List<OptionBehaviour>> optionBehavioursList)
    {
        foreach (var optionBehaviours in optionBehavioursList)
        foreach (var option in optionBehaviours)
            Object.Destroy(option.gameObject);
    }

    private static bool setNames(Dictionary<string, string> gameObjectNameDisplayNameMap)
    {
        foreach (var entry in gameObjectNameDisplayNameMap)
            if (GameObject.Find(entry.Key) != null)
            {
                // Settings setup has already been performed, fixing the title of the tab and returning
                GameObject.Find(entry.Key).transform.FindChild("GameGroup").FindChild("Text")
                    .GetComponent<TextMeshPro>().SetText(entry.Value);
                return true;
            }

        return false;
    }

    private static GameOptionsMenu getMenu(GameObject setting, string settingName)
    {
        var menu = setting.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
        setting.name = settingName;

        return menu;
    }

    private static SpriteRenderer getTabHighlight(GameObject tab, string tabName, string tabSpritePath)
    {
        var tabHighlight = tab.transform.FindChild("Hat Button").FindChild("Tab Background")
            .GetComponent<SpriteRenderer>();
        tab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite =
            UnityHelper.loadSpriteFromResources(tabSpritePath, 100f);
        tab.name = "tabName";

        return tabHighlight;
    }

    private static void setOptions(List<GameOptionsMenu> menus, List<List<OptionBehaviour>> options,
        List<GameObject> settings)
    {
        if (!(menus.Count == options.Count && options.Count == settings.Count))
        {
            Error("List counts are not equal");
            return;
        }

        for (var i = 0; i < menus.Count; i++)
        {
            menus[i].Children = options[i].ToArray();
            settings[i].gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
    private static void OptionFix(GameOptionsMenu __instance)
    {
        // Adapt task count for main options
        var list = new List<(string, FloatRange)>
        {
            ("NumCommonTasks", new FloatRange(0f, 4f)),
            ("NumShortTasks", new FloatRange(0f, 23f)),
            ("NumLongTasks", new FloatRange(0f, 15f))
        };

        list.Do(tuple => SetAUNumberOption(__instance, tuple.Item1, tuple.Item2));
    }

    public static void SetAUNumberOption(GameOptionsMenu menu, string OptionName, FloatRange range)
    {
        var option = menu.Children.FirstOrDefault(x => x.name == OptionName)?.TryCast<NumberOption>();
        if (option != null) 
            option.ValidRange = new FloatRange(0f, 15f);
    }
}

[Harmony]
public class StringOptionPatches
{
    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    private static bool StringOption_OnEnable(StringOption __instance)
    {
        if (!CustomOptionManager.Instance.TryGetOption(__instance, out var option)) return true;
        __instance.OnValueChanged = new Action<OptionBehaviour>(o => { });
        __instance.TitleText.text = option.Title;
        __instance.Value = __instance.oldValue = option.OptionSelection;
        __instance.ValueText.text = option.OptionSelection.GetString();
        __instance.ValueText.color = __instance.TitleText.color = option.Color;

        return false;
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    private static bool StringOption_Increase(StringOption __instance)
    {
        if (!CustomOptionManager.Instance.TryGetOption(__instance, out var option)) return true;
        option.OptionSelection.Increase();
        return false;
    }
    
    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    private static bool StringOption_Decrease(StringOption __instance)
    {
        if (!CustomOptionManager.Instance.TryGetOption(__instance, out var option)) return true;
        option.OptionSelection.Decrease();
        return false;
    }
}

/*[HarmonyPatch(typeof(StringOption), nameof(StringOption.FixedUpdate))]
public class StringOptionFixedUpdate {
    public static void Postfix(StringOption __instance) {
        if (!IL2CPPChainloader.Instance.Plugins.TryGetValue("com.DigiWorm.LevelImposter", out _)) return;
        CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
        if (option == null || !CustomOptionHolder.isMapSelectionOption(option)) return;
        if (GameOptionsManager.Instance.CurrentGameOptions.MapId == 6)
            if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption) {
                stringOption.ValueText.text = option.selections[option.selection].ToString();
            }
            else if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOptionToo) {
                stringOptionToo.oldValue = stringOptionToo.Value = option.selection;
                stringOptionToo.ValueText.text = option.selections[option.selection].ToString();
            }
    }
}*/

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
public class RpcSyncSettingsPatch
{
    public static void Postfix()
    {
        CustomOptionManager.Instance.ShareAllOptionSelection();
        CustomOptionManager.Instance.saveVanillaOptions();
    }
}

/*[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
public class AmongUsClientOnPlayerJoinedPatch {
    public static void Postfix() {
        if (PlayerControl.LocalPlayer != null && AmongUsClient.Instance.AmHost) {
            GameManager.Instance.LogicOptions.SyncOptions();
            CustomOption.ShareOptionSelections();
        }
    }
}*/

[HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
internal class GameOptionsMenuUpdatePatch
{
    private static float timer = 1f;

    public static void Postfix(GameOptionsMenu __instance)
    {
        // Return Menu Update if in normal among us settings 
        var gameSettingMenu = Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
        if (gameSettingMenu.RegularGameSettings.active || gameSettingMenu.RolesSettings.gameObject.active) return;

        __instance.GetComponentInParent<Scroller>().ContentYBounds.max = -0.5F + (__instance.Children.Length * 0.55F);
        timer += Time.deltaTime;
        if (timer < 0.1f) return;
        timer = 0f;

        var offset = 2.75f;
        foreach (CustomOption option in CustomOption.options)
        {
            if (GameObject.Find("TORSettings") && option.type != CustomOption.CustomOptionType.General &&
                option.type != CustomOptionType.HideNSeekMain && option.type != CustomOptionType.PropHunt)
                continue;
            if (GameObject.Find("ImpostorSettings") && option.type != CustomOption.CustomOptionType.Impostor)
                continue;
            if (GameObject.Find("NeutralSettings") && option.type != CustomOption.CustomOptionType.Neutral)
                continue;
            if (GameObject.Find("CrewmateSettings") && option.type != CustomOption.CustomOptionType.Crewmate)
                continue;
            if (GameObject.Find("ModifierSettings") && option.type != CustomOption.CustomOptionType.Modifier)
                continue;
            if (GameObject.Find("GuesserSettings") && option.type != CustomOption.CustomOptionType.Guesser)
                continue;
            if (GameObject.Find("HideNSeekSettings") && option.type != CustomOption.CustomOptionType.HideNSeekRoles)
                continue;
            if (option?.optionBehaviour != null && option.optionBehaviour.gameObject != null)
            {
                var enabled = true;
                var parent = option.parent;
                while (parent != null && enabled)
                {
                    enabled = parent.selection != 0;
                    parent = parent.parent;
                }

                option.optionBehaviour.gameObject.SetActive(enabled);
                if (enabled)
                {
                    offset -= option.isHeader ? 0.75f : 0.5f;
                    option.optionBehaviour.transform.localPosition = new Vector3(
                        option.optionBehaviour.transform.localPosition.x, offset,
                        option.optionBehaviour.transform.localPosition.z);
                }
            }
        }
    }
}

[HarmonyPatch]
internal class GameOptionsDataPatch
{
    public static int maxPage = 7;

    private static string buildRoleOptions()
    {
        var impRoles = "<size=150%><color=#ff1c1c>Impostors</color></size>" +
                       buildOptionsOfType(CustomOption.CustomOptionType.Impostor, true) + "\n";
        var neutralRoles = "<size=150%><color=#50544c>Neutrals</color></size>" +
                           buildOptionsOfType(CustomOption.CustomOptionType.Neutral, true) + "\n";
        var crewRoles = "<size=150%><color=#08fcfc>Crewmates</color></size>" +
                        buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, true) + "\n";
        var modifiers = "<size=150%><color=#ffec04>Modifiers</color></size>" +
                        buildOptionsOfType(CustomOption.CustomOptionType.Modifier, true);
        return impRoles + neutralRoles + crewRoles + modifiers;
    }

    private static string buildModifierExtras(CustomOption customOption)
    {
        // find options children with quantity
        var children = CustomOption.options.Where(o => o.parent == customOption);
        var quantity = children.Where(o => o.name.Contains("Quantity")).ToList();
        if (customOption.getSelection() == 0) return "";
        if (quantity.Count == 1) return $" ({quantity[0].getQuantity()})";
        if (customOption == CustomOptionHolder.modifierLover)
            return $" (1 Evil: {CustomOptionHolder.modifierLoverImpLoverRate.getSelection() * 10}%)";
        return "";
    }

    private static string buildOptionsOfType(CustomOption.CustomOptionType type, bool headerOnly)
    {
        var sb = new StringBuilder("\n");
        var options = CustomOption.options.Where(o => o.type == type);
        if (MapOptions.gameMode == CustomGameModes.Guesser)
        {
            if (type == CustomOption.CustomOptionType.General)
                options = CustomOption.options.Where(o =>
                    o.type == type || o.type == CustomOption.CustomOptionType.Guesser);
            List<int> remove = [308, 310, 311, 312, 313, 314, 315, 316, 317, 318];
            options = options.Where(x => !remove.Contains(x.id));
        }
        else if (MapOptions.gameMode == CustomGameModes.Classic)
        {
            options = options.Where(x =>
                !(x.type == CustomOption.CustomOptionType.Guesser || x == CustomOptionHolder.crewmateRolesFill));
        }
        else if (MapOptions.gameMode == CustomGameModes.HideNSeek)
        {
            options = options.Where(x =>
                x.type == CustomOption.CustomOptionType.HideNSeekMain ||
                x.type == CustomOption.CustomOptionType.HideNSeekRoles);
        }
        else if (MapOptions.gameMode == CustomGameModes.PropHunt)
        {
            options = options.Where(x => x.type == CustomOption.CustomOptionType.PropHunt);
        }

        foreach (var option in options)
            if (option.parent == null)
            {
                var line = $"{option.name}: {option.selections[option.selection].ToString()}";
                if (type == CustomOption.CustomOptionType.Modifier) line += buildModifierExtras(option);
                sb.AppendLine(line);
            }
            else if (option.parent.getSelection() > 0)
            {
                if (option.id == 103) //Deputy
                    sb.AppendLine(
                        $"- {Helpers.cs(Deputy.color, "Deputy")}: {option.selections[option.selection].ToString()}");
                else if (option.id == 224) //Sidekick
                    sb.AppendLine(
                        $"- {Helpers.cs(Sidekick.color, "Sidekick")}: {option.selections[option.selection].ToString()}");
                else if (option.id == 358) //Prosecutor
                    sb.AppendLine(
                        $"- {Helpers.cs(Lawyer.color, "Prosecutor")}: {option.selections[option.selection].ToString()}");

                else if (option.id == 3642134) //Can Swoop
                    sb.AppendLine(
                        $"- {Helpers.cs(Swooper.color, "Swooper")}: {option.selections[option.selection].ToString()}");
            }

        if (headerOnly) return sb.ToString();
        sb = new StringBuilder();

        foreach (CustomOption option in options)
        {
            if (MapOptions.gameMode == CustomGameModes.HideNSeek && option.type != CustomOptionType.HideNSeekMain &&
                option.type != CustomOptionType.HideNSeekRoles) continue;
            if (MapOptions.gameMode == CustomGameModes.PropHunt &&
                option.type != CustomOptionType.PropHunt) continue;
            if (option.parent != null)
            {
                var isIrrelevant = option.parent.getSelection() == 0 ||
                                   (option.parent.parent != null && option.parent.parent.getSelection() == 0);

                var c = isIrrelevant ? Color.grey : Color.white; // No use for now
                if (isIrrelevant) continue;
                sb.AppendLine(Helpers.cs(c, $"{option.name}: {option.selections[option.OptionSelection].ToString()}"));
            }
            else
            {
                if (option == CustomOptionHolder.crewmateRolesCountMin)
                {
                    var optionName =
                        CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Crewmate Roles");
                    var min = CustomOptionHolder.crewmateRolesCountMin.getSelection();
                    var max = CustomOptionHolder.crewmateRolesCountMax.getSelection();
                    var optionValue = "";
                    if (CustomOptionHolder.crewmateRolesFill.getBool())
                    {
                        var crewCount = PlayerControl.AllPlayerControls.Count -
                                        GameOptionsManager.Instance.currentGameOptions.NumImpostors;
                        int minNeutral = CustomOptionHolder.neutralRolesCountMin.getSelection();
                        int maxNeutral = CustomOptionHolder.neutralRolesCountMax.getSelection();
                        if (minNeutral > maxNeutral) minNeutral = maxNeutral;
                        min = crewCount - maxNeutral;
                        max = crewCount - minNeutral;
                        if (min < 0) min = 0;
                        if (max < 0) max = 0;
                        optionValue = "Fill: ";
                    }

                    if (min > max) min = max;
                    optionValue += min == max ? $"{max}" : $"{min} - {max}";
                    sb.AppendLine($"{optionName}: {optionValue}");
                }
                else if (option == CustomOptionHolder.neutralRolesCountMin)
                {
                    var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Neutral Roles");
                    var min = CustomOptionHolder.neutralRolesCountMin.getSelection();
                    var max = CustomOptionHolder.neutralRolesCountMax.getSelection();
                    if (min > max) min = max;
                    var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                    sb.AppendLine($"{optionName}: {optionValue}");
                }
                else if (option == CustomOptionHolder.impostorRolesCountMin)
                {
                    var optionName =
                        CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Impostor Roles");
                    var min = CustomOptionHolder.impostorRolesCountMin.getSelection();
                    var max = CustomOptionHolder.impostorRolesCountMax.getSelection();
                    if (max > GameOptionsManager.Instance.currentGameOptions.NumImpostors)
                        max = GameOptionsManager.Instance.currentGameOptions.NumImpostors;
                    if (min > max) min = max;
                    var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                    sb.AppendLine($"{optionName}: {optionValue}");
                }
                else if (option == CustomOptionHolder.modifiersCountMin)
                {
                    var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Modifiers");
                    var min = CustomOptionHolder.modifiersCountMin.getSelection();
                    var max = CustomOptionHolder.modifiersCountMax.getSelection();
                    if (min > max) min = max;
                    var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                    sb.AppendLine($"{optionName}: {optionValue}");
                }
                else if (option == CustomOptionHolder.crewmateRolesCountMax ||
                         option == CustomOptionHolder.neutralRolesCountMax ||
                         option == CustomOptionHolder.impostorRolesCountMax ||
                         option == CustomOptionHolder.modifiersCountMax)
                {
                }
                else
                {
                    sb.AppendLine($"\n{option.name}: {option.selections[option.OptionSelection].ToString()}");
                }
            }
        }

        return sb.ToString();
    }

    public static string buildAllOptions(string vanillaSettings = "", bool hideExtras = false)
    {
        if (vanillaSettings == "")
            vanillaSettings =
                GameOptionsManager.Instance.CurrentGameOptions.ToHudString(PlayerControl.AllPlayerControls.Count);
        var counter = TheOtherRolesPlugin.optionsPage;
        var hudString = counter != 0 && !hideExtras
            ? Helpers.cs(DateTime.Now.Second % 2 == 0 ? Color.white : Color.red, "(Use scroll wheel if necessary)\n\n")
            : "";

        if (MapOptions.gameMode == CustomGameModes.HideNSeek)
        {
            if (TheOtherRolesPlugin.optionsPage > 1) TheOtherRolesPlugin.optionsPage = 0;
            maxPage = 2;
            switch (counter)
            {
                case 0:
                    hudString += "Page 1: Hide N Seek Settings \n\n" +
                                 buildOptionsOfType(CustomOption.CustomOptionType.HideNSeekMain, false);
                    break;
                case 1:
                    hudString += "Page 2: Hide N Seek Role Settings \n\n" +
                                 buildOptionsOfType(CustomOption.CustomOptionType.HideNSeekRoles, false);
                    break;
            }
        }
        else if (MapOptions.gameMode == CustomGameModes.PropHunt)
        {
            maxPage = 1;
            switch (counter)
            {
                case 0:
                    hudString += "Page 1: Prop Hunt Settings \n\n" +
                                 buildOptionsOfType(CustomOption.CustomOptionType.PropHunt, false);
                    break;
            }
        }
        else
        {
            maxPage = 7;
            switch (counter)
            {
                case 0:
                    hudString += (!hideExtras ? "" : "Page 1: Vanilla Settings \n\n") + vanillaSettings;
                    break;
                case 1:
                    hudString += "Page 2: The Other Us Settings \n" +
                                 buildOptionsOfType(CustomOption.CustomOptionType.General, false);
                    break;
                case 2:
                    hudString += "Page 3: Role and Modifier Rates \n" + buildRoleOptions();
                    break;
                case 3:
                    hudString += "Page 4: Impostor Role Settings \n" +
                                 buildOptionsOfType(CustomOption.CustomOptionType.Impostor, false);
                    break;
                case 4:
                    hudString += "Page 5: Neutral Role Settings \n" +
                                 buildOptionsOfType(CustomOption.CustomOptionType.Neutral, false);
                    break;
                case 5:
                    hudString += "Page 6: Crewmate Role Settings \n" +
                                 buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, false);
                    break;
                case 6:
                    hudString += "Page 7: Modifier Settings \n" +
                                 buildOptionsOfType(CustomOption.CustomOptionType.Modifier, false);
                    break;
            }
        }

        if (!hideExtras || counter != 0)
            hudString += $"\n Press TAB or Page Number for more... ({counter + 1}/{maxPage})";
        return hudString;
    }

    public static readonly OptionTextBuilder builder = new(CustomOptionManager.Instance.options);
    public static int CurrentPage = 0;
    
    [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
    private static void Postfix(ref string __result)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek)
            return; // Allow Vanilla Hide N Seek
        
        __result = builder
            .SetParentText(__result)
            .BuildAll()
            .GetPageText(CurrentPage);
    }
}

[HarmonyPatch]
public class AddToKillDistanceSetting
{
    [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.AreInvalid))]
    [HarmonyPrefix]
    public static bool Prefix(GameOptionsData __instance, ref int maxExpectedPlayers)
    {
        //making the killdistances bound check higher since extra short is added
        return __instance.MaxPlayers > maxExpectedPlayers || __instance.NumImpostors < 1
                                                          || __instance.NumImpostors > 3 || __instance.KillDistance < 0
                                                          || __instance.KillDistance >=
                                                          GameOptionsData.KillDistances.Count
                                                          || __instance.PlayerSpeedMod <= 0f ||
                                                          __instance.PlayerSpeedMod > 3f;
    }

    [HarmonyPatch(typeof(NormalGameOptionsV07), nameof(NormalGameOptionsV07.AreInvalid))]
    [HarmonyPrefix]
    public static bool Prefix(NormalGameOptionsV07 __instance, ref int maxExpectedPlayers)
    {
        return __instance.MaxPlayers > maxExpectedPlayers || __instance.NumImpostors < 1
                                                          || __instance.NumImpostors > 3 || __instance.KillDistance < 0
                                                          || __instance.KillDistance >=
                                                          GameOptionsData.KillDistances.Count
                                                          || __instance.PlayerSpeedMod <= 0f ||
                                                          __instance.PlayerSpeedMod > 3f;
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    [HarmonyPrefix]
    public static void Prefix(StringOption __instance)
    {
        //prevents indexoutofrange exception breaking the setting if long happens to be selected
        //when host opens the laptop
        if (__instance.Title != StringNames.GameKillDistance || __instance.Value != 3) return;
        __instance.Value = 1;
        GameOptionsManager.Instance.currentNormalGameOptions.KillDistance = 1;
        GameManager.Instance.LogicOptions.SyncOptions();
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    [HarmonyPostfix]
    public static void Postfix(StringOption __instance)
    {
        if (__instance.Title == StringNames.GameKillDistance && __instance.Values.Count == 3)
            __instance.Values = new Il2CppStructArray<StringNames>(
                [(StringNames)49999, StringNames.SettingShort, StringNames.SettingMedium, StringNames.SettingLong]);
    }

    [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.AppendItem),
        [typeof(Il2CppSystem.Text.StringBuilder), typeof(StringNames), typeof(string)])]
    [HarmonyPrefix]
    public static void Prefix(ref StringNames stringName, ref string value)
    {
        if (stringName != StringNames.GameKillDistance) return;
        var index = GameOptionsManager.Instance.currentNormalGameOptions.KillDistance;
        value = GameOptionsData.KillDistanceStrings[index];
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
        [typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>)])]
    [HarmonyPriority(Priority.Last)]
    public static bool Prefix(ref string __result, ref StringNames id)
    {
        if ((int)id != 49999) return true;
        __result = "Very Short";
        return false;

    }

    public static void addKillDistance()
    {
        GameOptionsData.KillDistances = new Il2CppStructArray<float>([0.5f, 1f, 1.8f, 2.5f]);
        GameOptionsData.KillDistanceStrings = new Il2CppStringArray(["Very Short", "Short", "Medium", "Long"]);
    }
}

[HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
public static class GameOptionsNextPagePatch
{
    public static void Postfix(KeyboardJoystick __instance)
    {
        var page = TheOtherRolesPlugin.optionsPage;
        if (Input.GetKeyDown(KeyCode.Tab)) TheOtherRolesPlugin.optionsPage = (TheOtherRolesPlugin.optionsPage + 1) % 7;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) TheOtherRolesPlugin.optionsPage = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) TheOtherRolesPlugin.optionsPage = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) TheOtherRolesPlugin.optionsPage = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) TheOtherRolesPlugin.optionsPage = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) TheOtherRolesPlugin.optionsPage = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) TheOtherRolesPlugin.optionsPage = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) TheOtherRolesPlugin.optionsPage = 6;
        if (Input.GetKeyDown(KeyCode.F1))
            HudManagerUpdate.ToggleSettings(HudManager.Instance);
        if (TheOtherRolesPlugin.optionsPage >= GameOptionsDataPatch.maxPage) TheOtherRolesPlugin.optionsPage = 0;

        if (page != TheOtherRolesPlugin.optionsPage)
        {
            var position =
                (Vector3)FastDestroyableSingleton<HudManager>.Instance?.GameSettings?.transform.localPosition;
            FastDestroyableSingleton<HudManager>.Instance.GameSettings.transform.localPosition =
                new Vector3(position.x, 2.9f, position.z);
        }
    }
}

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
public class GameSettingsScalePatch
{
    public static void Prefix(HudManager __instance)
    {
        if (__instance.GameSettings != null) __instance.GameSettings.fontSize = 1.2f;
    }
}

// This class is taken and adapted from Town of Us Reactivated, https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/CustomOption/Patches.cs, Licensed under GPLv3
[HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
public class HudManagerUpdate
{
    private static float
        MinX,
        MinY = 2.9F;


    public static Scroller Scroller;
    private static Vector3 LastPosition;
    private static float lastAspect;
    private static bool setLastPosition;

    private static readonly TextMeshPro[] settingsTMPs = new TextMeshPro[4];
    private static GameObject settingsBackground;

    private static PassiveButton toggleSettingsButton;
    private static GameObject toggleSettingsButtonObject;

    public static void Prefix(HudManager __instance)
    {
        if (__instance.GameSettings?.transform == null) return;

        // Sets the MinX position to the left edge of the screen + 0.1 units
        var safeArea = Screen.safeArea;
        var aspect = Mathf.Min(Camera.main.aspect, safeArea.width / safeArea.height);
        var safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
        MinX = 0.1f - (safeOrthographicSize * aspect);

        if (!setLastPosition || aspect != lastAspect)
        {
            LastPosition = new Vector3(MinX, MinY);
            lastAspect = aspect;
            setLastPosition = true;
            if (Scroller != null) Scroller.ContentXBounds = new FloatRange(MinX, MinX);
        }

        CreateScroller(__instance);

        Scroller.gameObject.SetActive(__instance.GameSettings.gameObject.activeSelf);

        if (!Scroller.gameObject.active) return;

        var rows = __instance.GameSettings.text.Count(c => c == '\n');
        var LobbyTextRowHeight = 0.06F;
        var maxY = Mathf.Max(MinY, (rows * LobbyTextRowHeight) + ((rows - 38) * LobbyTextRowHeight));

        Scroller.ContentYBounds = new FloatRange(MinY, maxY);

        // Prevent scrolling when the player is interacting with a menu
        if (CachedPlayer.LocalPlayer?.Control.CanMove != true)
        {
            __instance.GameSettings.transform.localPosition = LastPosition;

            return;
        }

        if (__instance.GameSettings.transform.localPosition.x != MinX ||
            __instance.GameSettings.transform.localPosition.y < MinY) return;

        LastPosition = __instance.GameSettings.transform.localPosition;
    }

    private static void CreateScroller(HudManager __instance)
    {
        if (Scroller != null) return;

        var target = __instance.GameSettings.transform;

        Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
        Scroller.transform.SetParent(__instance.GameSettings.transform.parent);
        Scroller.gameObject.layer = 5;

        Scroller.transform.localScale = Vector3.one;
        Scroller.allowX = false;
        Scroller.allowY = true;
        Scroller.active = true;
        Scroller.velocity = new Vector2(0, 0);
        Scroller.ScrollbarYBounds = new FloatRange(0, 0);
        Scroller.ContentXBounds = new FloatRange(MinX, MinX);
        Scroller.enabled = true;

        Scroller.Inner = target;
        target.SetParent(Scroller.transform);
    }

    [HarmonyPrefix]
    public static void Prefix2(HudManager __instance)
    {
        if (!settingsTMPs[0]) return;
        foreach (var tmp in settingsTMPs) tmp.text = "";
        var settingsString = GameOptionsDataPatch.buildAllOptions(hideExtras: true);
        var blocks = settingsString.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        ;
        var curString = "";
        string curBlock;
        var j = 0;
        for (var i = 0; i < blocks.Length; i++)
        {
            curBlock = blocks[i];
            if (Helpers.lineCount(curBlock) + Helpers.lineCount(curString) < 43)
            {
                curString += curBlock + "\n\n";
            }
            else
            {
                settingsTMPs[j].text = curString;
                j++;

                curString = "\n" + curBlock + "\n\n";
                if (curString.Substring(0, 2) != "\n\n") curString = "\n" + curString;
            }
        }

        if (j < settingsTMPs.Length) settingsTMPs[j].text = curString;
        var blockCount = 0;
        foreach (var tmp in settingsTMPs)
            if (tmp.text != "")
                blockCount++;
        for (var i = 0; i < blockCount; i++)
            settingsTMPs[i].transform.localPosition = new Vector3((-blockCount * 1.2f) + (2.7f * i), 2.2f, -500f);
    }

    public static void OpenSettings(HudManager __instance)
    {
        if (__instance.FullScreen == null || (MapBehaviour.Instance && MapBehaviour.Instance.IsOpen)
                                          /*|| AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started*/
                                          || GameOptionsManager.Instance.currentGameOptions.GameMode ==
                                          GameModes.HideNSeek) return;
        settingsBackground = Object.Instantiate(__instance.FullScreen.gameObject, __instance.transform);
        settingsBackground.SetActive(true);
        var renderer = settingsBackground.GetComponent<SpriteRenderer>();
        renderer.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        renderer.enabled = true;

        for (var i = 0; i < settingsTMPs.Length; i++)
        {
            settingsTMPs[i] = Object.Instantiate(__instance.KillButton.cooldownTimerText, __instance.transform);
            settingsTMPs[i].alignment = TextAlignmentOptions.TopLeft;
            settingsTMPs[i].enableWordWrapping = false;
            settingsTMPs[i].transform.localScale = Vector3.one * 0.25f;
            settingsTMPs[i].gameObject.SetActive(true);
        }
    }

    public static void CloseSettings()
    {
        foreach (var tmp in settingsTMPs)
            if (tmp)
                tmp.gameObject.Destroy();

        if (settingsBackground) settingsBackground.Destroy();
    }

    public static void ToggleSettings(HudManager __instance)
    {
        if (settingsTMPs[0]) CloseSettings();
        else OpenSettings(__instance);
    }

    [HarmonyPostfix]
    public static void Postfix(HudManager __instance)
    {
        if (!toggleSettingsButton || !toggleSettingsButtonObject)
        {
            // add a special button for settings viewing:
            toggleSettingsButtonObject =
                Object.Instantiate(__instance.MapButton.gameObject, __instance.MapButton.transform.parent);
            toggleSettingsButtonObject.transform.localPosition =
                __instance.MapButton.transform.localPosition + new Vector3(0, -0.66f, -500f);
            var renderer = toggleSettingsButtonObject.GetComponent<SpriteRenderer>();
            renderer.sprite =
                UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.CurrentSettingsButton.png", 180f);
            toggleSettingsButton = toggleSettingsButtonObject.GetComponent<PassiveButton>();
            toggleSettingsButton.OnClick.RemoveAllListeners();
            toggleSettingsButton.OnClick.AddListener((Action)(() => ToggleSettings(__instance)));
        }

        toggleSettingsButtonObject.SetActive(__instance.MapButton.gameObject.active &&
                                             !(MapBehaviour.Instance && MapBehaviour.Instance.IsOpen) &&
                                             GameOptionsManager.Instance.currentGameOptions.GameMode !=
                                             GameModes.HideNSeek);
        toggleSettingsButtonObject.transform.localPosition =
            __instance.MapButton.transform.localPosition + new Vector3(0, -0.66f, -500f);
    }
}