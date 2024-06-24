using System;
using System.Collections.Generic;
using TheOtherUs.Options;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Crewmates;

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

    public bool isRevealed;

    public Mode mode = Mode.Chat;
    public bool needsUpdate = true;
    public Dictionary<byte, byte> playerRoomMap = new();

    public PlayerControl snitch;
    public CustomOption snitchLeftTasksForReveal;
    public CustomOption snitchMode;

    
    public CustomOption snitchTargets;
    public Targets targets = Targets.EvilPlayers;
    public int taskCountForReveal = 1;
    public TextMeshPro text;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(snitch),
        RoleClassType = typeof(Snitch),
        RoleType = CustomRoleType.Main,
        CreateRoleController = n => new SnitchController(n),
        Color = new Color32(184, 251, 79, byte.MaxValue),
        DescriptionText = "Finish your tasks",
        IntroInfo = "Finish your tasks to find the <color=#FF1919FF>Impostors</color>",
        RoleId = RoleId.Snitch,
        RoleTeam = RoleTeam.Crewmate,
        GetRole = Get<Snitch>
    };
    
    public class SnitchController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Snitch>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        taskCountForReveal = Mathf.RoundToInt(snitchLeftTasksForReveal);
        snitch = null;
        isRevealed = false;
        playerRoomMap = new Dictionary<byte, byte>();
        if (text != null) Object.Destroy(text);
        text = null;
        needsUpdate = true;
        mode = snitchMode.CastEnum<Mode>();
        targets = snitchTargets.CastEnum<Targets>();
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        snitchLeftTasksForReveal = roleOption.AddChild(
            "Task Count Where The Snitch Will Be Revealed", new FloatOptionSelection(5f, 0f, 25f, 1f));
        
        snitchMode = roleOption.AddChild("Information Mode", new StringOptionSelection(["Chat", "Map", "Chat & Map"]));
        
        snitchTargets = roleOption.AddChild("Targets", new StringOptionSelection(["All Evil Players", "Killing Players"]));
    }
}