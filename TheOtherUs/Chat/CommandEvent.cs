using System;
using System.Collections.Generic;

namespace TheOtherUs.Chat;

public class CommandEvent(string command, Action<CommandEvent> onEvent, CommandPermissions permissions = CommandPermissions.All)
{
    public string Command { get; init; } = command;
    public CommandPermissions permission { get; set; } = permissions;
    public Action<CommandEvent> OnEvent = onEvent;
    public List<string> Context { get; set; }
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