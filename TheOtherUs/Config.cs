using BepInEx.Configuration;

namespace TheOtherUs;

public static class TheOtherUsConfig
{
    public static BepInConfig<bool> DebugMode { get; private set; }
    public static BepInConfig<bool> GhostsSeeInformation { get; private set; }
    public static BepInConfig<bool> GhostsSeeRoles { get; private set; }
    public static BepInConfig<bool> GhostsSeeModifier { get; private set; }
    public static BepInConfig<bool> GhostsSeeVotes { get; private set; }
    public static BepInConfig<bool> ShowRoleSummary { get; private set; }
    public static BepInConfig<bool> ShowLighterDarker { get; private set; }
    public static BepInConfig<bool> EnableSoundEffects { get; private set; }
    public static BepInConfig<bool> EnableHorseMode { get; private set; }
    public static BepInConfig<bool> ToggleCursor { get; private set; }
    public static BepInConfig<bool> ShowVentsOnMap { get; private set; }
    public static BepInConfig<string> Ip { get; private set; }
    public static BepInConfig<ushort> Port { get; private set; }
    public static BepInConfig<string> ShowPopUpVersion { get; private set; }

    public static void Bind(ConfigFile Config)
    {
        DebugMode = new BepInConfig<bool>(Config, "Enable Debug Mode", false);
        GhostsSeeInformation = new BepInConfig<bool>(Config, "Ghosts See Remaining Tasks", true);
        GhostsSeeRoles = new BepInConfig<bool>(Config,  "Ghosts See Roles", true);
        GhostsSeeModifier = new BepInConfig<bool>(Config,  "Ghosts See Modifier", true);
        GhostsSeeVotes = new BepInConfig<bool>(Config, "Ghosts See Votes", true);
        ShowRoleSummary = new BepInConfig<bool>(Config,  "Show Role Summary", true);
        ShowLighterDarker = new BepInConfig<bool>(Config,  "Show Lighter / Darker", false);
        ToggleCursor = new BepInConfig<bool>(Config, "Better Cursor", true);
        EnableSoundEffects = new BepInConfig<bool>(Config,  "Enable Sound Effects", true);
        EnableHorseMode = new BepInConfig<bool>(Config,  "Enable Horse Mode", false);
        ShowPopUpVersion = new BepInConfig<string>(Config, "Show PopUp", "0");
        ShowVentsOnMap = new BepInConfig<bool>(Config, "Show vent positions on minimap", false);

        Ip = new BepInConfig<string>(Config,  "Custom Server IP", "127.0.0.1");
        Port = new BepInConfig<ushort>(Config,  "Custom Server Port", 22023); ;
    }
}

public sealed class BepInConfig<T>(ConfigFile configFile, string Key, T value)
{
    public static string Section { get; internal set; } = "Custom";

    private readonly ConfigEntry<T> entry = configFile.Bind(Section, Key, value);
    public static implicit operator T(BepInConfig<T> config) => config.entry.Value;
}