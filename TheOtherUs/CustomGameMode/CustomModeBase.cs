using System.Collections.Generic;

namespace TheOtherUs.CustomGameMode;

public enum CustomGameModes
{
    Classic,
    Guesser,
    HideNSeek,
    PropHunt
}

public abstract class CustomModeBase
{
    public virtual CustomGameModes Mode { get; }
}

public sealed class CustomModeManager : ManagerBase<CustomModeManager>
{
    public CustomGameModes CurrentMode { get; set; } = CustomGameModes.Classic;
    public readonly List<CustomModeBase> CustomModes = [];
    public void Register(CustomModeBase @base)
    {
        CustomModes.Add(@base);
    }


    public static bool ModeIs(CustomGameModes mode) => mode == Instance.CurrentMode;
}