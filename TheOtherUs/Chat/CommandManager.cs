using System.Collections.Generic;

namespace TheOtherUs.Chat;

public class CommandManager : ManagerBase<CommandManager>
{
    public readonly List<CommandEvent> commandEvents = [];

    public bool Clear = false;

    public void Register(CommandEvent @event)
    {
        commandEvents.RemoveAll(n => n.Command == @event.Command);
        commandEvents.Add(@event);
    }

    public void Register(IEnumerable<CommandEvent> events) => events.Do(Register);

    /*public List<CommandEvent> Find(string[] texts)
    {
        var list = new List<CommandEvent>();
        foreach (var e in commandEvents)
        {
            var command = e.Command;
            for (var i = 0; i < texts.Length - 1; i++)
            {
                if (command[i] != texts[i])
                {
                    goto next;
                }
            }
            list.Add(e);
            
            next:;
        }

        return list;
    }*/
    
    public void FindStart(string command)
    {
    }
}