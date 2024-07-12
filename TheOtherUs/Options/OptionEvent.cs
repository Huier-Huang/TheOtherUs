using System;
using System.Text.Json.Serialization;

namespace TheOtherUs.Options;

public  class OptionEvent
{
    [JsonIgnore] public CustomOption option;

    public event Action<object[], string> OptionEvents;

    public virtual void OnOptionChange(OptionSelectionBase selection)
    {
        option = selection.option;
        if (selection is BoolOptionSelection boolOptionSelection && option.IsHeader)
        {
            var op = (CustomParentOption)option;
            foreach (var child in op.Child)
            {
                child.Enabled = boolOptionSelection.GetBool();
            }
        }
        
        
        option.ShareOptionChange();
        OptionEvents?.Invoke([selection], "OptionChange");
    }

    public virtual void OnOptionCreate(CustomOption CreateOption)
    {
        option = CreateOption;
        option.ShareOptionChange();
        OptionEvents?.Invoke([CreateOption], "optionCreate");
    }
}