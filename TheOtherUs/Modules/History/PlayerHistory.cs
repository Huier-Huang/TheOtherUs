namespace TheOtherUs.Modules.History;

public class PlayerHistory(PlayerControl player) : IHistory
{
    public PlayerControl Player { get; } = player;
    
    public IHistory StartRecord()
    {
        HistoryManager.Instance.Register(this);
        return this;
    }

    public void Clear()
    {
    }

    public void Dispose()
    {
    }
}