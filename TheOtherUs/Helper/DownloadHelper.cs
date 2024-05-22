using System.Globalization;
using System.IO;
using AmongUs.Data.Legacy;
using TheOtherUs.Modules.Languages;

namespace TheOtherUs.Helper;

public static class DownloadHelper
{
    public static string FastUrl => "https://github.moeyy.xyz";

    public static bool IsCN()
    {
        return RegionInfo.CurrentRegion.ThreeLetterISORegionName == "CHN"
               ||
               (SupportedLangs)LegacySaveManager.LastLanguage == SupportedLangs.SChinese
               ||
               LanguageManager.Instance.CurrentLang == SupportedLangs.SChinese;
    }

    public static string GithubUrl(this string url)
    {
        if (IsCN() && !url.Contains(FastUrl))
            return url
                .Replace("https://github.com", $"{FastUrl}/https://github.com")
                .Replace("https://raw.githubusercontent.com", $"{FastUrl}/https://raw.githubusercontent.com");

        return url;
    }

    public static string ReadToEnd(this Stream stream)
    {
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}