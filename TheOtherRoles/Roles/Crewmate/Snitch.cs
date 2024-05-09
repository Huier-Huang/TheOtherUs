using System;
using System.Collections.Generic;
using TheOtherRoles.Modules.Options;
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

    public PlayerControl snitch;

    public Mode mode = Mode.Chat;
    public Targets targets = Targets.EvilPlayers;
    public int taskCountForReveal = 1;

    public bool isRevealed;
    public Dictionary<byte, byte> playerRoomMap = new();
    public TextMeshPro text;
    public bool needsUpdate = true;

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

    public readonly static RoleInfo roleInfo = new()
    {
        Name = nameof(snitch),
        Color = new Color32(184, 251, 79, byte.MaxValue),
        Description = "Finish your tasks",
        IntroInfo = "Finish your tasks to find the <color=#FF1919FF>Impostors</color>",
        RoleId = RoleId.Snitch,
        RoleTeam = RoleTeam.Crewmate,
        GetRole = Get<Snitch>
    };

    public CustomOption snitchSpawnRate;
    public CustomOption snitchLeftTasksForReveal;
    public CustomOption snitchMode;
    public CustomOption snitchTargets;
    public override void OptionCreate()
    {
        snitchSpawnRate = new CustomOption(210, "Snitch".ColorString(roleInfo.Color), CustomOptionHolder.rates, null, true);
        snitchLeftTasksForReveal = new CustomOption(219,
            "Task Count Where The Snitch Will Be Revealed", 5f, 0f, 25f, 1f, snitchSpawnRate);
        snitchMode = new CustomOption(211, "Information Mode", ["Chat", "Map", "Chat & Map"],
            snitchSpawnRate);
        snitchTargets = new CustomOption(212, "Targets",
            ["All Evil Players", "Killing Players"], snitchSpawnRate);
    }

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override Type RoleType { get; protected set; } = typeof(Snitch);
}