using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hazel;
using Reactor.Utilities.Extensions;
using TheOtherUs.Modules.Components;
using UnityEngine;
using Object = UnityEngine.Object;
using Color = UnityEngine.Color;

namespace TheOtherUs.Options;

public enum CustomOptionTypes
{
    General,
    Role,
    Mode
}

public class CustomRoleOption : CustomParentOption
{
    public CustomRoleOption(RoleBase @base, bool enableNumber = true, bool enableRate = true, bool enableVision = false) 
        : 
        base(@base.RoleInfo.Name, CustomOptionTypes.Role, new StringOptionSelection(roleRateStrings),  color:@base.RoleInfo.Color)
    {
        roleBase = @base;
        EnabledTranslate = true;
        TabType = TabTypes.Classic;

        if (enableVision)
            visionOption = AddChild("Impostor Vision", new BoolOptionSelection(false));
        
        switch (enableNumber)
        {
            case true when enableRate:
                numberOption = AddChild("Role Number", new IntOptionSelection(0, 0, 5, 1));
                return;
            case true:
                OptionSelection = new IntOptionSelection(0, 0, 5, 1);
                break;
        }
    }
    
    
    public static readonly string[] roleRateStrings = ["0%","10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"];
    public int Rate => 10 * OptionSelection.Selection;
    public readonly RoleBase roleBase;
    public CustomOption numberOption;
    public CustomOption visionOption;
}

public class CustomModeOption : CustomParentOption
{
    public readonly CustomGameModes mode;

    public CustomModeOption(CustomGameModes mode, string Title, OptionSelectionBase selection, Color color = default) 
        : base(Title, CustomOptionTypes.Mode, selection, color)
    {
        this.mode = mode;
        TabType = this.mode switch
        {
            CustomGameModes.Guesser => TabTypes.Guesser,
            CustomGameModes.HideNSeek => TabTypes.HideNSeek,
            CustomGameModes.PropHunt => TabTypes.PropHunt,
            _ => TabTypes.Classic
        };
    }
}

public class CustomGeneralOption : CustomParentOption
{
    public CustomGeneralOption(string title, OptionSelectionBase selection, Color color = default) : base(title, CustomOptionTypes.General, selection, color)
    {
        TabType = TabTypes.Classic;
    }
}

public class CustomParentOption(string Title, CustomOptionTypes type, OptionSelectionBase selection, Color color)
    : CustomOption(Title, type, selection, null, color)
{
    public List<CustomOption> Child = [];
    public override CustomOption AddChild(string title, OptionSelectionBase selection, Color color = default)
    {
        var option = new CustomOption(title, CustomOptionType, selection, this, color == default ? Color : color);
        Child.Add(option);
        return option;
    }

    public static CustomParentOption Form(CustomOption option)
    {
        var po = (CustomParentOption)option;
        po.Child = option.optionInfo.Children.Select(n => n.option).ToList();
        return po;
    }
    
    public CustomOption this[Index index] => Child[index];
    public CustomOption this[string title] => Child.First(n => n.Title == title);
}


public class CustomOption
{
    [JsonIgnore] 
    #nullable enable
    public OptionBehaviour? optionBehaviour;
    #nullable disable

    public bool IsHeader => optionInfo.Parent == null;
    [JsonIgnore] 
    public Color Color { get; set; } = Color.white;

    public void SetSelection(int selection) => OptionSelection.Selection = selection;
    [JsonIgnore] public int Selection => OptionSelection.Selection;
    [JsonIgnore] public TabTypes TabType;

    public virtual void Create(OptionTabMenuBase optionTabMenuBase, Transform Parent)
    {
        var stringOption = Object.Instantiate(optionTabMenuBase.StringOptionTemplate, Parent);
        stringOption.OnValueChanged = new Action<OptionBehaviour>(o => { });
        if (EnabledTranslate)
        {
            stringOption.TitleText.Destroy();
            stringOption.TitleText = stringOption.gameObject.AddComponent<TranslateText>();
            var TitleText = (TranslateText)stringOption.TitleText;
            TitleText.Id = Title;

            if (OptionSelection is StringOptionSelection selection)
            {
                stringOption.ValueText.Destroy();
                stringOption.ValueText = stringOption.gameObject.AddComponent<TranslateText>();
                var ValueText = (TranslateText)stringOption.ValueText;
                ValueText.Id = selection.GetString();
            }
        }
        stringOption.TitleText.text = Title;
        stringOption.Value = stringOption.oldValue = OptionSelection.Selection;
        stringOption.ValueText.text = OptionSelection.GetString();

        optionBehaviour = stringOption;
    }
    
    public virtual CustomOption AddChild(string title, OptionSelectionBase selection, Color color = default)
    {
        var option = new CustomOption(title, CustomOptionType, selection, this, color == default ? Color : color);
        return option;
    }
    
    public CustomOption(string Title, CustomOption Parent, OptionSelectionBase selection, Color color = default)
        : this(Title, Parent.CustomOptionType, selection, Parent, color)
    {
    }
    
    public CustomOption(string Title, CustomOptionTypes type, OptionSelectionBase selection, CustomOption Parent, Color color = default)
    {
        CustomOptionType = type;
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
            TabType = Parent.TabType;
            Parent.optionInfo.Children.Add(optionInfo);
        }

        if (color != default)
            Color = color;

        CustomOptionManager.Instance.Register(this);
    }

    public CustomOptionTypes CustomOptionType { get; set; }

    [JsonIgnore] public OptionEvent optionEvent { get; set; } = new();

    public OptionSelectionBase OptionSelection { get; set; }
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
    public bool EnabledTranslate { get; set; } = true;


    /*/// <summary>
    ///     翻译Id
    /// </summary>
    [JsonIgnore]
    public string TranslateId { get; set; } = string.Empty;*/

    public void ShareOptionChange()
    {
        FastRpcWriter.StartNewRpcWriter(CustomRPC.Option, LobbyBehaviour.Instance)
            .WritePacked((int)Option_Flag.Share)
            .WritePacked(optionInfo.Id)
            .Write(OptionSelection.Selection)
            .RPCSend();
    }

    public string Title => optionInfo.Title;

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

    public T CastEnum<T>() where T : struct, Enum =>
        Enum.TryParse<T>(OptionSelection.GetString(), out var result) ? result : Enum.GetValues<T>()[Selection];

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

[Harmony]
internal static class GameSettingMenuPatch
{
    public static HashSet<OptionTabMenuBase> EnableOptionTab = 
        [
            new ClassicTabMenu(),
            new HideNSeekTabMenu(),
            new GuesserTabMenu(),
            new PropHuntTabMenu()
        ];
    
    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.OnEnable)), HarmonyPrefix]
    private static void OnEnable(GameSettingMenu __instance)
    {
        if (CheckHas(__instance))
            return;
        
        OptionTabMenuBase.CreateTabMenus(EnableOptionTab, __instance);
    }

    public static bool CheckHas(GameSettingMenu menu)
    {
        return false;
    }
}


[Harmony]
internal static class OptionTabPatches
{

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
    
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public static void RpcSyncSettingsPatch_Postfix()
    {
        CustomOptionManager.Instance.ShareAllOptionSelection();
        CustomOptionManager.Instance.saveVanillaOptions();
    }
}

