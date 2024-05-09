using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using Hazel;
using UnityEngine;

namespace TheOtherRoles.Modules.Options;

public class CustomOptionManager : ManagerBase<CustomOptionManager>
{
    public List<CustomOption> options = [];
    public string vanillaSettings;
    public int CurrentPresetId;
    public readonly HashSet<OptionPreset> presets = [];
    public OptionPreset currentPreset => presets.FirstOrDefault(n => n.PresetId == CurrentPresetId);
    
    public void Register(CustomOption option)
    {
        option.optionInfo.Id = assignOptionId();
        options.Add(option);
    }

    public void UnRegister(CustomOption option)
    {
        releaseOptionId(option);
        options.Remove(option);
    }
    
    // 获取选项
    public bool TryGetOption(int id, [MaybeNullWhen(false)] out CustomOption option)
    {
        var varOption = options.FirstOrDefault(n => n.optionInfo.Id == id);
        option = varOption;
        return varOption == null;
    }
    
    public bool TryGetOption(OptionBehaviour behaviour, [MaybeNullWhen(false)] out CustomOption option)
    {
        var varOption = options.FirstOrDefault(n => n.optionBehaviour == behaviour);
        option = varOption;
        return varOption == null;
    }
    
    // 分配选项Id
    private int LastId;
    private readonly Queue<int> noAssignId = [];
    public int assignOptionId()
    {
        if (noAssignId.TryDequeue(out var id))
            return id;
        assign:
        if (options.All(n => n.optionInfo.Id != LastId)) return LastId;
        LastId++;
        goto assign;
    }
    
    // 释放选项Id
    public void releaseOptionId(CustomOption option) => 
        noAssignId.Enqueue(option.optionInfo.Id);
    
    public void switchToPreset(int PresetId) {
        saveVanillaOptions();
    }

    public void SavePreset(OptionPreset preset)
    {
        
    }

    public OptionPreset LoadPreset(string Preset)
    {
        var preset = new OptionPreset();
        presets.Add(preset);
        return preset;
    }

    public string SettingSplitString = "!/!/!";
    
    public void copyToClipboard() 
    {
        GUIUtility.systemCopyBuffer = string.Join("!/!/!", Main.Version, currentPreset.Serialize(), vanillaSettings);
    }

    public bool pasteFromClipboard() 
    {
        var allSettings = GUIUtility.systemCopyBuffer;
        try {
            var settingsSplit = allSettings.Split(SettingSplitString);
            var versionInfo = Version.Parse(settingsSplit[0]);
            if (versionInfo.CompareTo(Main.Version) != 0)
                return false;
            var Preset = JsonSerializer.Deserialize<OptionPreset>(settingsSplit[1]);
            presets.Add(Preset);
            CurrentPresetId = Preset.PresetId;
            var vanillaSetting = settingsSplit[2];

            vanillaSettings = vanillaSetting;
            loadVanillaOptions();
            return true;
        } catch (Exception e) {
            Warn($"{e}: tried to paste invalid settings!");
            SoundEffectsManager.Load();
            SoundEffectsManager.play("fail");
            return false;
        }
    }
    
    public void saveVanillaOptions() =>
        vanillaSettings = Convert.ToBase64String(GameOptionsManager.Instance.gameOptionsFactory.ToBytes(GameManager.Instance.LogicOptions.currentGameOptions, false));

    public void loadVanillaOptions() {
        var optionsString = vanillaSettings;
        if (optionsString == "") return;
        GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.gameOptionsFactory.FromBytes(Convert.FromBase64String(optionsString));
        GameOptionsManager.Instance.CurrentGameOptions = GameOptionsManager.Instance.GameHostOptions;
        GameManager.Instance.LogicOptions.SetGameOptions(GameOptionsManager.Instance.CurrentGameOptions);
        GameManager.Instance.LogicOptions.SyncOptions();
    }

    public int OptionSplit = 35;
    public void ShareAllOption()
    {
        var max = options.Count / OptionSplit;
        var remainder = options.Count % OptionSplit;
        if (max > 0)
        {
            for (var i = 1; i < max; i++)
            {
                SendSerializeOption((i - 1) * OptionSplit, (i * OptionSplit) - 1, OptionSplit);
            }

            if (remainder != 0)
            {
                SendSerializeOption(max * OptionSplit, (max * OptionSplit) + remainder - 1, OptionSplit);
            }
        }
        else
        {
            SendSerializeOption(0, options.Count - 1, options.Count);
        }
    }
    
    public void ShareAllOptionSelection()
    {
        var writer = FastRpcWriter.StartNewRpcWriter(CustomRPC.Option, GameData.Instance);
        writer
            .WritePacked((int)CustomOption.Option_Flag.ShareAll)
            .WritePacked(options.Count);
        foreach (var option in options)
        {
            writer.WritePacked(option.optionInfo.Id);
            writer.Write(option.selection.Selection);
        }
    }

    public void SendSerializeOption(int Start, int End, int Count)
    {
        var writer = FastRpcWriter.StartNewRpcWriter(CustomRPC.Option, GameData.Instance);
        writer
            .WritePacked((int)CustomOption.Option_Flag.ShareAll)
            .WritePacked(Count);
        for (var j = Start; j < End; j++)
        {
            var option = options[j];
            writer.Write(option.optionInfo.Id);
            option.Serialize(writer);
        }
    }
    
    [RPCListener(CustomRPC.Option)]
    public static void ShareOption_Listener(MessageReader reader)
    {
        var flag = reader.ReadPackedInt32();
        switch ((CustomOption.Option_Flag)flag)
        {
            case CustomOption.Option_Flag.Share:
            {
                var id = reader.ReadPackedInt32();
                var selection = reader.ReadInt32();
                Instance.TryGetOption(id, out var option);
                option.selection.Selection = selection;
                break;
            }
            
            case CustomOption.Option_Flag.ShareAll:
            {
                var count = reader.ReadInt32();
                for (var i = 1; i < count; i++)
                {
                    var id = reader.ReadInt32();
                    Instance.TryGetOption(id, out var option);
                    option.Deserialize(reader);
                }
                break;
            }

            case CustomOption.Option_Flag.ShareAllSelection:
            {
                var count = reader.ReadInt32();
                for (var i = 1; i < count; i++)
                {
                    var id = reader.ReadInt32();
                    Instance.TryGetOption(id, out var option);
                    option.selection.Selection = reader.ReadInt32();
                }
                break;
            }
        }
    }
}