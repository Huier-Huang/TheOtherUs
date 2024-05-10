using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Snitch : RoleBase
{
    public enum Mode
    {
        Chat = 0,
        Map = 1,
        ChatAndMap = 2
    }

    public enum Targets
    {
        EvilPlayers = 0,
        Killers = 1
    }

    public static readonly RoleInfo roleInfo = new()
    {
        Name = nameof(snitch),
        Color = new Color32(184, 251, 79, byte.MaxValue),
        Description = "Finish your tasks",
        IntroInfo = "Finish your tasks to find the <color=#FF1919FF>Impostors</color>",
        RoleId = RoleId.Snitch,
        RoleTeam = RoleTeam.Crewmate,
        GetRole = Get<Snitch>
    };

    public bool isRevealed;

    public Mode mode = Mode.Chat;
    public bool needsUpdate = true;
    public Dictionary<byte, byte> playerRoomMap = new();

    public PlayerControl snitch;
    public CustomOption snitchLeftTasksForReveal;
    public CustomOption snitchMode;

    public CustomOption snitchSpawnRate;
    public CustomOption snitchTargets;
    public Targets targets = Targets.EvilPlayers;
    public int taskCountForReveal = 1;
    public TextMeshPro text;

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override Type RoleType { get; protected set; } = typeof(Snitch);

    public override void ClearAndReload()
    {
        taskCountForReveal = Mathf.RoundToInt(snitchLeftTasksForReveal);
        snitch = null;
        isRevealed = false;
        playerRoomMap = new Dictionary<byte, byte>();
        if (text != null) Object.Destroy(text);
        text = null;
        needsUpdate = true;
        mode = (Mode)snitchMode.getSelection();
        targets = (Targets)snitchTargets.getSelection();
    }

    public override void OptionCreate()
    {
        snitchSpawnRate =
            new CustomOption(210, "Snitch".ColorString(roleInfo.Color), CustomOptionHolder.rates, null, true);
        snitchLeftTasksForReveal = new CustomOption(219,
            "Task Count Where The Snitch Will Be Revealed", 5f, 0f, 25f, 1f, snitchSpawnRate);
        snitchMode = new CustomOption(211, "Information Mode", ["Chat", "Map", "Chat & Map"],
            snitchSpawnRate);
        snitchTargets = new CustomOption(212, "Targets",
            ["All Evil Players", "Killing Players"], snitchSpawnRate);
    }
}