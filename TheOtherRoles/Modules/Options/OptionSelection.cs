using System.Text.Json.Serialization;

namespace TheOtherRoles.Modules.Options;

public abstract class OptionSelection
{
    [JsonIgnore]
    public CustomOption option { get; set; }
    
    [JsonInclude]
    public int Selection { get; set; }
    
    [JsonInclude]
    public int DefSelection { get; set; }
    
    [JsonInclude]
    public abstract int Quantity { get; set; }

    public virtual void InitFormJson()
    {
        
    }

    public virtual void Decrease()
    {
        option.optionEvent.OnOptionChange(this);
    }

    public virtual void Increase()
    {
        option.optionEvent.OnOptionChange(this);
    }

    public abstract T GetValue<T>();
}