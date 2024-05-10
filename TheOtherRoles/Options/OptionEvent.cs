using System;
using System.Text.Json.Serialization;

namespace TheOtherRoles.Options;

public class OptionEvent
{
    [JsonIgnore]
    public CustomOption option;

    public event Action<object[], string> OptionEvents; 
    
    public virtual void OnOptionChange(OptionSelectionBase selection)
    {
        option.ShareOptionChange();
        OptionEvents?.Invoke([selection], "OptionChange");
    }
    
    public virtual void OnOptionCreate(CustomOption CreateOption)
    {
        option.ShareOptionChange();
        OptionEvents?.Invoke([option], "optionCreate");
    }
}