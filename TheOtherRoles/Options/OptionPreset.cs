using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheOtherRoles.Modules.Options;

public class OptionPreset
{
    [JsonInclude]
    public HashSet<CustomOption> options = [];

    [JsonInclude]
    public string PresetName { get; set; }
    
    [JsonInclude]
    public int PresetId { get; set; }

    public static OptionPreset Load(string preset) => JsonSerializer.Deserialize<OptionPreset>(preset);


    public string Serialize() => JsonSerializer.Serialize(this);
}