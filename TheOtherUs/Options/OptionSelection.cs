using System;
using System.Text.Json.Serialization;

namespace TheOtherUs.Options;

public interface IOptionSelectionValue
{
    public string GetString();

    public float GetFloat();

    public int GetInt();

    public bool GetBool();
}

public abstract class OptionSelectionBase : IOptionSelectionValue
{
    [JsonIgnore] public CustomOption option { get; set; }

    public virtual int Selection
    {
        get;
        set;
    }

    public virtual void Decrease()
    {
        option.optionEvent.OnOptionChange(this);
    }

    public virtual void Increase()
    {
        option.optionEvent.OnOptionChange(this);
    }

    public abstract string GetString();

    public abstract float GetFloat();

    public abstract int GetInt();

    public abstract bool GetBool();

    public virtual void InitFormJson()
    {
    }
    
    public static implicit operator bool(OptionSelectionBase option)
    {
        return option.GetBool();
    }

    public static implicit operator float(OptionSelectionBase option)
    {
        return option.GetFloat();
    }

    public static implicit operator int(OptionSelectionBase option)
    {
        return option.GetInt();
    }

    public static implicit operator string(OptionSelectionBase option)
    {
        return option.GetString();
    }
}

public abstract class OptionSelection(object[] selections, int def)
    : IntOptionSelection(def, 0, selections.Length - 1, 1)
{
    [JsonInclude] public virtual int Quantity { get; set; } = selections.Length;

    [JsonIgnore] public object[] Selections { get; set; } = selections;

    public override string GetString()
    {
        return Selections[Value] as string;
    }

    public override float GetFloat()
    {
        return Selections[Value] is float ? (float)Selections[Value] : 0;
    }

    public override int GetInt()
    {
        return Selections[Value] is int ? (int)Selections[Value] : 0;
    }

    public override bool GetBool()
    {
        return Selections[Value] is bool && (bool)Selections[Value];
    }

    // Getter
    public static implicit operator bool(OptionSelection option)
    {
        return option.GetBool();
    }

    public static implicit operator float(OptionSelection option)
    {
        return option.GetFloat();
    }

    public static implicit operator int(OptionSelection option)
    {
        return option.GetInt();
    }

    public static implicit operator string(OptionSelection option)
    {
        return option.GetString();
    }
}

public class BoolOptionSelection(bool Def = true) : StringOptionSelection(strings:Switches, Def ? 1 : 0)
{
    public static readonly string[] Switches = ["False", "True"];
}

public class StringOptionSelection(string[] strings, int Def = 0)
    : IntOptionSelection(Def, 0, strings.Length - 1, 1)
{
    public readonly string[] Strings = strings;
    
    public override string GetString()
    {
        return Strings[Value];
    }
}

public abstract class StepOptionSelection<T>(T step, T min, T max, T def)
    : OptionSelectionBase where T : struct, IConvertible
{
    public T Step { get; set; } = step;
    public T Min { get; set; } = min;
    public T Max { get; set; } = max;
    public T Value { get; set; } = def;
    public T Def { get; set; } = def;

    public override string GetString()
    {
        return Value.ToString();
    }

    public override float GetFloat()
    {
        return Value.ToSingle(null);
    }

    public override int GetInt()
    {
        return Value.ToInt32(null);
    }

    public override bool GetBool()
    {
        return Value.ToBoolean(null);
    }

    // Getter
    public static implicit operator bool(StepOptionSelection<T> option)
    {
        return option.GetBool();
    }

    public static implicit operator float(StepOptionSelection<T> option)
    {
        return option.GetFloat();
    }

    public static implicit operator int(StepOptionSelection<T> option)
    {
        return option.GetInt();
    }

    public static implicit operator string(StepOptionSelection<T> option)
    {
        return option.GetString();
    }
}

public class FloatOptionSelection(float Def, float min, float max, float step)
    : StepOptionSelection<float>(step, min, max, Def)
{
    public override int Selection => (int)((Value - Min) / step);

    public override float GetFloat()
    {
        return Value;
    }

    public override int GetInt()
    {
        return (int)Value;
    }

    public override void Increase()
    {
        if (Value >= Max) return;
        Value += Step;
        base.Increase();
    }

    public override void Decrease()
    {
        if (Value <= Min) return;
        Value -= Step;
        base.Decrease();
    }
}

public class IntOptionSelection(int Def, int min, int max, int step)
    : StepOptionSelection<int>(step, min, max, Def)
{
    public override int Selection
    {
        get
        {
            if (step == 1)
            {
                return Selection;
            }

            return (Value - Min) / step;
        }
    }

    public override float GetFloat()
    {
        return Value;
    }

    public override int GetInt()
    {
        return Value;
    }

    public override void Increase()
    {
        if (Value >= Max) return;
        Value += Step;
        base.Increase();
    }

    public override void Decrease()
    {
        if (Value <= Min) return;
        Value -= Step;
        base.Decrease();
    }
}