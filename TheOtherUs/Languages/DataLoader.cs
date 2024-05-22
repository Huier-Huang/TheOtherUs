using System.IO;
using System.Linq;

namespace TheOtherUs.Modules.Languages;

public class DataLoader : LanguageLoaderBase
{
    public DataLoader()
    {
        Filter = [".dat", ".data"];
    }

    public override void Load(LanguageManager _manager, Stream stream, string fileName)
    {
        stream.StartRead((text, index) => Read(text, index, _manager, fileName.PareNameToLangId()),
            out var allText);
    }

    private void Read(string text, int index, LanguageManager currentManager, SupportedLangs lang)
    {
        if (text == string.Empty || text.StartsWith("#"))
            return;

        var texts = text.Split(":").Select(n => n.Replace("\"", "").Replace(" ", string.Empty)).ToArray();
        if (texts.Length < 2)
        {
            Info($"DataLoader {index} count < 2");
            return;
        }

        currentManager.AddToMap(lang, texts[0], texts[1], nameof(DataLoader));
    }
}