using System;
using System.Collections.Generic;
using System.IO;

namespace TheOtherUs.Configs;

public sealed class VanillaConfigManager : ManagerBase<VanillaConfigManager>
{
    public const string FileName = "VanillaConfigs.json";
    public const string SaveDirName = "VanillaGroups";

    public const string PostfixName = "amogus";
    public static readonly HashSet<string> VanillaConfigNames = 
        [
            "player",
            "settings"
        ];
    
    public static readonly string AUDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "LOW", "Innersloth", "Among Us");
    public static readonly string ConfigDir = Path.Combine(AUDataPath, SaveDirName);
    public static readonly string GroupConfigPath = Path.Combine(ConfigDir, FileName);
    public List<VanillaConfigGroup> Groups = [];
    public Version CurrentAmongUsVersion { get; private set; }

    static VanillaConfigManager()
    {
        if (!Directory.Exists(ConfigDir))
            Directory.CreateDirectory(ConfigDir);

        if (!File.Exists(GroupConfigPath))
            File.Create(GroupConfigPath);
    }
    
    public VanillaConfigManager SetConfigInfo()
    {
        return this;
    }
}

public class VanillaConfigGroup
{
    public HashSet<Version> SupportedVersions { get; set; } = [];
    public HashSet<(string, string)> NameAndConfigFile { get; set; } = [];


    public VanillaConfigGroup Register(string name, string configFile)
    {
        NameAndConfigFile.Add((name, configFile));
        return this;
    }
}