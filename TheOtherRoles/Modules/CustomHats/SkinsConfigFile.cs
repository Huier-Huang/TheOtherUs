using System.Collections.Generic;
using System.Text.Json.Serialization;
using TheOtherRoles.CustomCosmetics;

namespace TheOtherRoles.Modules.CustomHats;

public class SkinsConfigFile
{
    [JsonPropertyName("hats")] public List<CustomHat> Hats { get; set; }
}