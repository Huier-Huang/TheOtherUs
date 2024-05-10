using System;
using System.Text.Json.Serialization;

namespace TheOtherRoles.Options;

public interface IOptionSelectionValue
{
    public string GetString();
    
    public float GetFloat();
    
    public int GetInt();
    
    public bool GetBool();
}

public abstract class OptionSelectionBase
{
    [JsonIgnore]
    public CustomOption option { get; set; }
    
    public virtual void Decrease()
    {
        option.optionEvent.OnOptionChange(this);
    }

    public virtual void Increase()
    {
        option.optionEvent.OnOptionChange(this);
    }
    
    public virtual void InitFormJson()
    {
    }
}

public abstract class OptionSelection(object[] selections, int def)
    : IntOptionSelection(def, 0, selections.Length - 1, 1)
{
    
    [JsonInclude]
    public virtual int Quantity { get; set; } = selections.Length;

    [JsonIgnore]
    public object[] Selections { get; set; } = selections;

    public override string GetString() => Selections[Selection] as string;
    
    public override float GetFloat() => Selections[Selection] is float ? (float)Selections[Selection] : 0;
    
    public override int GetInt() => Selections[Selection] is int ? (int)Selections[Selection] : 0;
    
    public override bool GetBool() => Selections[Selection] is bool && (bool)Selections[Selection];

    public override void Increase()
    {
        if (Selection >= Max) return;
        Selection++;
        base.Increase();
    }

    public override void Decrease()
    {
        if (Selection <= Min) return;
        Selection--;
        base.Decrease();
    }

    // Getter
    public static implicit operator bool(OptionSelection option) => option.GetBool();
    
    public static implicit operator float(OptionSelection option) => option.GetFloat();
    
    public static implicit operator int(OptionSelection option) => option.GetInt();
    
    public static implicit operator string(OptionSelection option) => option.GetString();
}

public class BoolOptionSelection(bool Def = true) : OptionSelection(Switches.CastArray<object>(), Def ? 1 : 0)
{
    public static readonly string[] Switches = [ "False", "True"];
}

public class StringOptionSelection(int Def, params string[] strings) : OptionSelection(strings.CastArray<object>(), Def);

public abstract class StepOptionSelection<T>(T step, T min, T max, T def) : OptionSelectionBase, IOptionSelectionValue where T : struct, IConvertible
{
    public T Step { get; set; } = step;
    public T Min { get; set; } = min;
    public T Max { get; set; } = max;
    public T Selection { get; set; } = def;
    public T Def { get; set; } = def;
    
    public virtual string GetString() => Selection.ToString();
    public virtual float GetFloat() => Selection.ToSingle(null);
    public virtual int GetInt() => Selection.ToInt32(null);
    public virtual bool GetBool() => Selection.ToBoolean(null);
    
    // Getter
    public static implicit operator bool(StepOptionSelection<T> option) => option.GetBool();
    
    public static implicit operator float(StepOptionSelection<T> option) => option.GetFloat();
    
    public static implicit operator int(StepOptionSelection<T> option) => option.GetInt();
    
    public static implicit operator string(StepOptionSelection<T> option) => option.GetString();
}

public class FloatOptionSelection(float Def, float min, float max, float step)
    : StepOptionSelection<float>(step, min, max, Def)
{
    public override float GetFloat() => Selection;

    public override int GetInt() => (int)Selection;

    public override void Increase()
    {
        if (Selection >= Max) return;
        Selection += Step;
        base.Increase();
    }

    public override void Decrease()
    {
        if (Selection <= Min) return;
        Selection -= Step;
        base.Decrease();
    }
}

public class IntOptionSelection(int Def, int min, int max, int step)
    : StepOptionSelection<int>(step, min, max, Def)
{
    public override float GetFloat() => Selection;

    public override int GetInt() => Selection;

    public override void Increase()
    {
        if (Selection >= Max) return;
        Selection += Step;
        base.Increase();
    }

    public override void Decrease()
    {
        if (Selection <= Min) return;
        Selection -= Step;
        base.Decrease();
    }
}