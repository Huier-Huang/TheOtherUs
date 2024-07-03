using System;

namespace TheOtherUs.Modules.History;

public class HistoryManager : ListManager<HistoryManager, IHistory>
{
    public HistoryManager Register(IHistory history)
    {
        List.Add(history);
        return this;
    }
    
    public bool TryGetCreatePlayerHistory(PlayerControl player, out PlayerHistory history)
    {
        if (List.TryGet(n => n is PlayerHistory _PH && _PH.Player == player, out var _OH))
        {
            history = _OH as PlayerHistory;
            return true;
        }
        
        history = new PlayerHistory(player);
        return false;
    }
}

public interface IHistory : IDisposable
{
    public IHistory StartRecord();
    public void Clear();
}