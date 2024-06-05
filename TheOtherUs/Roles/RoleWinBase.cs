namespace TheOtherUs.Roles;

public abstract class RoleWinBase
{
    /// <summary>
    /// 0 最低
    /// </summary>
    public virtual int Priority { get; set; } = 0;
    
    public virtual void Check()
    {
    }

    public virtual void OnEnd()
    {
    }

    public static RoleWinBase GetDef(RoleBase @base) =>
        @base.Team == RoleTeam.Crewmate ? new CrewmateWin() : new ImpostorWin();
}

public class CrewmateWin : RoleWinBase
{
    
}

public class ImpostorWin : RoleWinBase
{
    
}