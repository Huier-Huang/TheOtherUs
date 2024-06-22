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
    public static CustomGameModes mode { get; set; }
}