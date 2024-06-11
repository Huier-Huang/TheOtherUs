using System;
using System.Net;

namespace TheOtherUs.Chat;

public class CommandEvent
{
    public string[] Command { get; init; }
    public Action<CommandInfo> Event { get; set; }
    public CommandPermissions permission { get; set; }

    public CommandEvent RegisterEvent(Action<CommandInfo> e)
    {
        Event += e;
        return this;
    }

    public void Start(string text)
    {
        
    }
}

[Flags]
public enum CommandPermissions
{
    All,
    Host,
    Player,
    Lobby,
    OnGame,
    Debug
}

public record CommandInfo();