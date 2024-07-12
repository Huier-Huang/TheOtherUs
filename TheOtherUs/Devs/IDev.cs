namespace TheOtherUs.Devs;

public interface IDev
{
    public string PUID { get; set; }
    public string FriendId { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }

    public bool Is(PlayerControl player);
}

public class Dev : IDev
{
    public string PUID { get; set; }
    public string FriendId { get; set; }
    public string Name { get; set; }
    public string Password { get; set; } = string.Empty;

    public virtual bool Is(PlayerControl player)
    {
        return player.FriendCode == FriendId && player.Puid == PUID;
    }
}

