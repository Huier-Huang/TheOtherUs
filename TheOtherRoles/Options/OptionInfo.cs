using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using UnityEngine;

namespace TheOtherRoles.Options;

public class OptionInfo
{
    [JsonIgnore] public CustomOption option;

    [JsonInclude] public string Title { get; set; }

    [JsonInclude] public int ParentId => Parent.Id;

    [JsonIgnore] public OptionInfo Parent { get; set; }

    [JsonIgnore] public HashSet<OptionInfo> Children { get; set; } = [];

    [JsonInclude] public int[] ChildIds => Children.Select(x => x.Id).ToArray();

    [JsonInclude] public int Id { get; set; }

    public void InitFormId()
    {
    }
}