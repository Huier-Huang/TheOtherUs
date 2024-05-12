using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BepInEx;
using TheOtherRoles.CustomCosmetics.Configs;
using UnityEngine;

namespace TheOtherRoles.CustomCosmetics;

public class CosmeticsManager : ManagerBase<CosmeticsManager>
{
    public List<Sprite> sprites = [];
    public HashSet<Sprite> HatSprites => sprites.Where(n => n.name.StartsWith("Hats/")).ToHashSet();
    public HashSet<Sprite> VisorSprites => sprites.Where(n => n.name.StartsWith("Visors/")).ToHashSet();
    public HashSet<Sprite> NamePlateSprites => sprites.Where(n => n.name.StartsWith("NamePlates/")).ToHashSet();
    
    public static readonly string CosmeticDir = Path.Combine(Paths.GameRootPath, "Cosmetics");
    public static readonly string HatDir = Path.Combine(CosmeticDir, "Hats");
    public static readonly string VisorDir = Path.Combine(CosmeticDir, "Visors");
    public static readonly string NamePlateDir = Path.Combine(CosmeticDir, "NamePlates");
    public static readonly string ManagerConfigDir = Path.Combine(CosmeticDir, "ManagerConfig");

    static CosmeticsManager()
    {
        string[] list = [CosmeticDir, HatDir, VisorDir, NamePlateDir, ManagerConfigDir];
        foreach (var path in list)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path!);
        }
    }
    
    public static readonly CosmeticsManagerConfig DefConfig = new()
    {
        RootUrl = "https://raw.githubusercontent.com/TheOtherRolesAU/TheOtherHats/master",
        hasCosmetics = CustomCosmeticsFlags.Hat
    };
    
    public HashSet<CosmeticsManagerConfig> configs = [];

    public List<ICustomCosmetic> customCosmetics = [];
    public HashSet<CustomHat> CustomHats => customCosmetics.Where(n => n is CustomHat).Cast<CustomHat>().ToHashSet();
    public HashSet<CustomVisor> CustomVisors => customCosmetics.Where(n => n is CustomVisor).Cast<CustomVisor>().ToHashSet();
    public HashSet<CustomNamePlate> CustomNamePlates => customCosmetics.Where(n => n is CustomNamePlate).Cast<CustomNamePlate>().ToHashSet();

    public void DefConfigCreateAndInit()
    {
        InitManager();
        Instance.Init(DefConfig);
    }

    public void InitManager()
    {
        LoadConfigFormDisk(new DirectoryInfo(ManagerConfigDir));
        LoadSpriteFormDisk();
    }
    
    public void LoadConfigFormDisk(DirectoryInfo dir)
    {
        var files = dir.GetFiles(".json");
        foreach (var file in files)
        {
            var str = File.ReadAllText(file.FullName);
            var config = JsonSerializer.Deserialize<CosmeticsManagerConfig>(str);
            Init(config);
        }
    }
    
    public void LoadSpriteFormDisk()
    {
        string[] spriteDir = [HatDir, VisorDir, NamePlateDir];
        foreach (var spDir in spriteDir)
        {
            sprites.AddRange(new SpriteLoader(spDir).LoadAllHatSprite(".png"));
        }
    }
    
    public void Init(CosmeticsManagerConfig config)
    {
        configs.Add(config);
        Task.Factory.StartNew(() => Start(config));
    }

    private readonly HttpClient client = new();
    
    #nullable enable
    public Sprite? GetSprite(CustomCosmeticsFlags flags, string name)
    {
        var AllSprite = flags switch
        {
            CustomCosmeticsFlags.Hat => HatSprites,
            CustomCosmeticsFlags.Visor => VisorSprites,
            CustomCosmeticsFlags.NamePlate => NamePlateSprites,
            _ => sprites.ToHashSet()
        };

        return AllSprite.FirstOrDefault(n => n.name.EndsWith(name));
    }
    #nullable disable


    public async void DownLoadSprite(CustomCosmeticsFlags flag, string root, string dir, string[] res)
    {
        try
        {
            var LocalDir = flag switch
            {
                CustomCosmeticsFlags.Hat => HatDir,
                CustomCosmeticsFlags.Visor => VisorDir,
                CustomCosmeticsFlags.NamePlate => NamePlateDir,
                _ => string.Empty
            };
            if (LocalDir == string.Empty) return;
            foreach (var r in res)
            {
                var localPath = Path.Combine(LocalDir, r);
                if (File.Exists(localPath) || sprites.Exists(n => n.name.EndsWith(r.Replace(".png", string.Empty))))
                    continue;
            
                var url = Path.Combine(root, dir, r);
                var stream = await client.GetStreamAsync(url);
                var file = File.Create(localPath);
                await stream.CopyToAsync(file);
            }
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }
    
    public async void Start(CosmeticsManagerConfig config)
    {
        if (config.hasCosmetics.HasFlag(CustomCosmeticsFlags.Hat))
        {
            var ConfigStream = await client.GetStreamAsync(Path.Combine(config.RootUrl, config.HatFileName));
            var json = JsonDocument.Parse(ConfigStream.ReadToEnd());
            var hats = json.RootElement.GetProperty(config.HatPropertyName).Deserialize<List<CustomHatConfig>>();
            foreach (var hatConfig in hats)
            {
                var res = new[]
                {
                    hatConfig.Resource, hatConfig.BackResource, hatConfig.BackFlipResource, hatConfig.ClimbResource,
                    hatConfig.FlipResource
                }.Where(n => n != string.Empty).ToArray();
                DownLoadSprite(CustomCosmeticsFlags.Hat, config.RootUrl, config.HatDirName, res);
                
                var hat = new CustomHat
                {
                    ManagerConfig = config,
                    config = hatConfig,
                    Resource = GetSprite(CustomCosmeticsFlags.Hat, hatConfig.Resource),
                    BackSprite = GetSprite(CustomCosmeticsFlags.Hat, hatConfig.BackResource),
                    BackFlipSprite = GetSprite(CustomCosmeticsFlags.Hat, hatConfig.BackFlipResource),
                    ClimbSprite = GetSprite(CustomCosmeticsFlags.Hat, hatConfig.ClimbResource),
                    FlipSprite = GetSprite(CustomCosmeticsFlags.Hat, hatConfig.FlipResource)
                };
                hat.data = hatConfig.createHatData(hat.Resource, hat.BackSprite, hat.ClimbSprite);
                customCosmetics.Add(hat);
            }
        }
        
        if (config.hasCosmetics.HasFlag(CustomCosmeticsFlags.Visor))
        {
            var ConfigStream = await client.GetStreamAsync(Path.Combine(config.RootUrl, config.VisorFileName));
            var json = JsonDocument.Parse(ConfigStream.ReadToEnd());
            var visors = json.RootElement.GetProperty(config.VisorPropertyName).Deserialize<List<CustomCosmeticConfig>>();
            foreach (var visorConfig in visors)
            {
                var res = new[] { visorConfig.Resource }.Where(n => n != string.Empty).ToArray();
                DownLoadSprite(CustomCosmeticsFlags.Visor, config.RootUrl, config.VisorDirName, res);
                
                var visor = new CustomVisor
                {
                    ManagerConfig = config,
                    config = visorConfig,
                    Resource = GetSprite(CustomCosmeticsFlags.Visor, visorConfig.Resource),
                };
                visor.data = visorConfig.createVisorData(visor.Resource);
                
                customCosmetics.Add(visor);
            }
        }
        
        if (config.hasCosmetics.HasFlag(CustomCosmeticsFlags.NamePlate))
        {
            var ConfigStream = await client.GetStreamAsync(Path.Combine(config.RootUrl, config.NamePlateFileName));
            var json = JsonDocument.Parse(ConfigStream.ReadToEnd());
            var namePlates = json.RootElement.GetProperty(config.NamePlatePropertyName).Deserialize<List<CustomCosmeticConfig>>();
            foreach (var namePlateConfig in namePlates)
            {
                var res = new[] { namePlateConfig.Resource }.Where(n => n != string.Empty).ToArray();
                DownLoadSprite(CustomCosmeticsFlags.NamePlate, config.RootUrl, config.NamePlateDirName, res);
                
                var NamePlate = new CustomNamePlate
                {
                    ManagerConfig = config,
                    config = namePlateConfig,
                    Resource = GetSprite(CustomCosmeticsFlags.NamePlate, namePlateConfig.Resource)
                };
                NamePlate.data = namePlateConfig.createNamePlateData(NamePlate.Resource);
                
                customCosmetics.Add(NamePlate);
            }
        }
    }
}

public class CosmeticsManagerConfig
{
    public string RootUrl { get; set; }
    public CustomCosmeticsFlags hasCosmetics { get; set; }
    public string HatDirName { get; set; } = "hats";
    public string VisorDirName { get; set; } = "Visors";
    public string NamePlateDirName { get; set; } = "NamePlates";
    public string HatFileName { get; set; } = "CustomHats.json";
    public string VisorFileName { get; set; } ="CustomVisors.json";
    public string NamePlateFileName { get; set; } ="CustomNamePlates.json";
    
    public string HatPropertyName { get; set; } = "hats";
    public string VisorPropertyName { get; set; } ="Visors";
    public string NamePlatePropertyName { get; set; } ="nameplates";
}

[Flags]
public enum CustomCosmeticsFlags
{
    Hat,
    Skin,
    Visor,
    NamePlate,
    Pet
}