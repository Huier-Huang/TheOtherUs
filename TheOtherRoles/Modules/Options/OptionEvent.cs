using System;
using System.Text.Json.Serialization;

namespace TheOtherRoles.Modules.Options;

public class OptionEvent
{
    [JsonIgnore]
    public CustomOption option;

    public event Action<OptionSelection, string> OptionEvents; 
    
    public virtual void OnOptionChange(OptionSelection selection)
    {
        option.ShareOptionChange();
        OptionEvents?.Invoke(selection, "OptionChange");
    }
    
    public virtual void OnOptionCreate(OptionSelection selection)
    {
        option.ShareOptionChange();
        OptionEvents?.Invoke(selection, "OptionChange");
    }
}