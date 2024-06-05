using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Legacy;
using Csv;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.CustomCosmetics;

public class DIYColor
{
    public static int StartIndex { get; private set; } = 18;

    public static readonly List<DIYColor> DIYColors =
    [
        new DIYColor
            (
                "Tamarind", 
                new Color32(48, 28, 34, byte.MaxValue), 
                new Color32(30, 11, 16, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Army", 
                new Color32(39, 45, 31, byte.MaxValue), 
                new Color32(11, 30, 24, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Olive", 
                new Color32(154, 140, 61, byte.MaxValue), 
                new Color32(104, 95, 40, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Turquoise", 
                new Color32(22, 132, 176, byte.MaxValue), 
                new Color32(15, 89, 117, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Mint", 
                new Color32(111, 192, 156, byte.MaxValue), 
                new Color32(65, 148, 111, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Lavender",
                new Color32(173, 126, 201, byte.MaxValue), 
                new Color32(131, 58, 203, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Nougat",
                new Color32(160, 101, 56, byte.MaxValue), 
                new Color32(115, 15, 78, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Peach", 
                new Color32(255, 164, 119, byte.MaxValue), 
                new Color32(238, 128, 100, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Wasabi", 
                new Color32(112, 143, 46, byte.MaxValue), 
                new Color32(72, 92, 29, byte.MaxValue)
                ),
        
        new DIYColor
            (
                "Hot Pink", 
                new Color32(255, 51, 102, byte.MaxValue), 
                new Color32(232, 0, 58, byte.MaxValue)
                ),
        
        new DIYColor
        (
            "Petrol",
            new Color32(0, 99, 105, byte.MaxValue), 
            new Color32(0, 61, 54, byte.MaxValue)
        ),
        
        new DIYColor
        (
            "Lemon",
            new Color32(0xDB, 0xFD, 0x2F, byte.MaxValue),
            new Color32(0x74, 0xE5, 0x10, byte.MaxValue)
        ),
        
        new DIYColor
        (
            "Signal\nOrange", 
            new Color32(0xF7, 0x44, 0x17, byte.MaxValue),
            new Color32(0x9B, 0x2E, 0x0F, byte.MaxValue)
        ),
        
        new DIYColor
        (
            "Teal", 
            new Color32(0x25, 0xB8, 0xBF, byte.MaxValue),
            new Color32(0x12, 0x89, 0x86, byte.MaxValue)
        ),
        
        new DIYColor
        (
            "Blurple", 
            new Color32(61, 44, 142, byte.MaxValue), 
            new Color32(25, 14, 90, byte.MaxValue)
        ),
        
        new DIYColor
        (
            "Sunrise",
            new Color32(0xFF, 0xCA, 0x19, byte.MaxValue),
            new Color32(0xDB, 0x44, 0x42, byte.MaxValue)
        ),
        
        new DIYColor
        (
            "Ice", 
            new Color32(0xA8, 0xDF, 0xFF, byte.MaxValue),
            new Color32(0x59, 0x9F, 0xC8, byte.MaxValue)
        ),

        new DIYColor
        (
            "Fuchsia",
            new Color32(164, 17, 129, byte.MaxValue),
            new Color32(104, 3, 79, byte.MaxValue)
        ),

        new DIYColor(
            "Royal\nGreen", //36
            new Color32(9, 82, 33, byte.MaxValue),
            new Color32(0, 46, 8, byte.MaxValue)
        ),

        new DIYColor(
            "Slime",
            new Color32(244, 255, 188, byte.MaxValue),
            new Color32(167, 239, 112, byte.MaxValue)
        ),

        new DIYColor(
            "Navy", //38
            new Color32(9, 43, 119, byte.MaxValue),
            new Color32(0, 13, 56, byte.MaxValue)
        ),

        new DIYColor(
            "Darkness", //39
            new Color32(36, 39, 40, byte.MaxValue),
            new Color32(10, 10, 10, byte.MaxValue)
        ),

        new DIYColor(
            "Ocean", //40
            new Color32(55, 159, 218, byte.MaxValue),
            new Color32(62, 92, 158, byte.MaxValue)
        ),

        new DIYColor(
            "Sundown", // 41
            new Color32(252, 194, 100, byte.MaxValue),
            new Color32(197, 98, 54, byte.MaxValue)
        )
    ];

    public static List<Color32> sortAllColor = [];
    public static List<VanillaColor> vanillaColors;
    
    public DIYColor(string text, Color32 color, Color32 shadow) : this(text, (Color)color, (Color)shadow)
    {
    }

    public DIYColor(string text, byte[] colorBytes, byte[] shadowBytes) : this(
        text,
        new Color32(colorBytes[0], colorBytes[1], colorBytes[2], colorBytes[3]),
        new Color32(shadowBytes[0], shadowBytes[1], shadowBytes[2], shadowBytes[3])
        )
    {
    }

    public DIYColor(string text, Color color, Color shadow)
    {
        Text = text;
        Color = color;
        Shadow = shadow;
        var gValue = (color.r * 0.299) + (color.g * 0.587) + (color.b * 0.114);
        Lighter = gValue < 125;
        Info($"DIYColor Text:{text} Color:{color} Shadow:{shadow} gValue:{gValue} Lighter:{Lighter}");
    }

    public Color Color { get; set; }
    public Color Shadow { get; set; }

    public string Text { get; set; }
    public string TranslateId { get; set; } = string.Empty;
    public int Index { get; set; }

    public bool Lighter { get; set; }

    public static void SetColors()
    {
        SetVanillaColors();
        SetDIYColors();
    }

    public static void SetVanillaColors()
    {
        for (var i = 0; i < Palette.PlayerColors.Length - 1; i++)
        {
            var color = Palette.PlayerColors[i];
            var shadow = Palette.ShadowColors[i];
            var name = Palette.ColorNames[i];
            vanillaColors.Add(new VanillaColor(color, shadow, i, name));
        }

        StartIndex = vanillaColors.Count;
    }

    public static void SetDIYColors()
    {
        var current = 0;
        foreach (var color in DIYColors)
        {
            color.Index = StartIndex + current;
            Palette.PlayerColors[color.Index] = color;
            Palette.ShadowColors[color.Index] = color.Shadow;
            current++;
        }

        sortAllColor = Palette.PlayerColors.ToList();
        sortAllColor.Sort((x, y) =>
        {
            var re = x.r.CompareTo(y.r);
            if (re == 0)
                re = x.g.CompareTo(y.g);

            if (re == 0)
                re = x.b.CompareTo(y.b);

            if (re == 0)
                re = x.a.CompareTo(y.a);

            return re;
        });
    }

    public static implicit operator Color32(DIYColor color)
    {
        return color.Color;
    }

    public static implicit operator Color(DIYColor color)
    {
        return color.Color;
    }

    public static int GeyIndex(Color32 color)
    {
        var value = vanillaColors.FirstOrDefault(n => Compare(n.Color, color));
        if (value != null)
            return value.Index;

        var diy = DIYColors.FirstOrDefault(n => Compare(n.Color, color));
        if (diy != null)
            return diy.Index;

        return -1;
    }

    public static bool Compare(Color32 color1, Color32 color2)
    {
        return color1.r == color2.r && color1.g == color2.g && color1.b == color2.g && color1.a == color2.a;
    }

    public static string DIYColorPath => Path.Combine(CosmeticsManager.CosmeticDir, "DIYColors.cs");
    public static void LoadDIYColor()
    {
        if (!File.Exists(DIYColorPath))
        {
            using var _ = File.Create(DIYColorPath);
            return;
        }

        using var stream = File.OpenRead(DIYColorPath);
        var options = new CsvOptions
        {
            HeaderMode = HeaderMode.HeaderPresent,
            AllowNewLineInEnclosedFieldValues = false
        };
        foreach (var line in CsvReader.ReadFromStream(stream, options))
        {
            try
            {
                var color1 = line.Values[1].Split(".").Select(byte.Parse).ToArray();
                var color2 = line.Values[2].Split(".").Select(byte.Parse).ToArray();
                DIYColors.Add(new DIYColor(line.Values[0], color1, color2));
            }
            catch (Exception ex)
            {
                Exception(ex);
            }
        }
    }

    public record VanillaColor(Color32 Color, Color32 Shadow, int Index, StringNames StringNames);
}

[Harmony]
public static class DIYColorPatch
{
    private static bool needsPatch;

    private static readonly uint pickableColors = (uint)Palette.ColorNames.Length;

    [HarmonyPatch(typeof(Palette), nameof(Palette.GetColorName))]
    [HarmonyPrefix]
    private static bool OnGetColorName(int colorId, ref string __result)
    {
        if (colorId < DIYColor.StartIndex || colorId > DIYColor.StartIndex + DIYColor.DIYColors.Count)
            return true;

        var color = DIYColor.DIYColors.FirstOrDefault(n => n.Index == colorId);
        if (color == null) return true;
        var text = color.TranslateId == string.Empty ? color.Text : color.TranslateId.Translate();
        __result = text;
        return false;
    }

    [HarmonyPatch(typeof(LegacySaveManager), nameof(LegacySaveManager.LoadPlayerPrefs))]
    [HarmonyPrefix]
    private static void LoadPrePrefix([HarmonyArgument(0)] bool overrideLoad)
    {
        if (!LegacySaveManager.loaded || overrideLoad)
            needsPatch = true;
    }

    [HarmonyPatch(typeof(LegacySaveManager), nameof(LegacySaveManager.LoadPlayerPrefs))]
    [HarmonyPostfix]
    private static void LoadPrePostfix()
    {
        if (!needsPatch) return;
        LegacySaveManager.colorConfig %= pickableColors;
        needsPatch = false;
    }

    private static bool isTaken(PlayerControl player, uint color)
    {
        return GameData.Instance.AllPlayers.GetFastEnumerator().Any(p =>
            !p.Disconnected && p.PlayerId != player.PlayerId && p.DefaultOutfit.ColorId == color);
    }

    /*[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckColor))]
    [HarmonyPrefix]
    private static bool CheckColorPrefix(PlayerControl __instance, [HarmonyArgument(0)] byte bodyColor)
    {
        // Fix incorrect color assignment
        uint color = bodyColor;
        if (isTaken(__instance, color) || color >= Palette.PlayerColors.Length)
        {
            var num = 0;
            while (num++ < 50 && (color >= pickableColors || isTaken(__instance, color)))
                color = (color + 1) % pickableColors;
        }

        __instance.RpcSetColor((byte)color);
        return false;
    }*/
    

    [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
    [HarmonyPrefix]
    private static bool PlayerTabOnEnable(PlayerTab __instance)
    {
        if (Palette.PlayerColors.Length < DIYColor.vanillaColors.Count + DIYColor.DIYColors.Count)
            DIYColor.SetColors();
        
        __instance.PlayerPreview.gameObject.SetActive(true);
        if (__instance.HasLocalPlayer())
            __instance.PlayerPreview.UpdateFromLocalPlayer(PlayerMaterial.MaskType.None);
        else
            __instance.PlayerPreview.UpdateFromDataManager(PlayerMaterial.MaskType.None);

        var num = Palette.PlayerColors.Length / 4f;
        for (var i = 0; i < DIYColor.sortAllColor.Count - 1; i++)
        {
            var num2 = __instance.XRange.Lerp(i % 4 / 3f);
            var num3 = __instance.YStart - ((float)i / 4 * __instance.YOffset);
            var colorChip = Object.Instantiate(__instance.ColorTabPrefab, __instance.ColorTabArea, true);
            colorChip.transform.localPosition = new Vector3(num2, num3, -1f);
            var color = DIYColor.sortAllColor[i];
            var id = DIYColor.GeyIndex(color);
            if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
            {
                colorChip.Button.OnMouseOver.AddListener(() => { __instance.SelectColor(id); });

                colorChip.Button.OnMouseOut.AddListener(() =>
                {
                    __instance.SelectColor(DataManager.Player.Customization.Color);
                });
                colorChip.Button.OnClick.AddListener(__instance.ClickEquip);
            }
            else
            {
                colorChip.Button.OnClick.AddListener(() => { __instance.SelectColor(id); });
            }

            colorChip.Inner.SpriteColor = color;
            colorChip.Tag = id;
            __instance.ColorChips.Add(colorChip);
        }

        __instance.currentColor = DataManager.Player.Customization.Color;
        return false;
    }
}