using System.Collections.Generic;
using System.Linq;
using TheOtherUs.Modules.Randoms;

namespace TheOtherUs.Roles.Assigns;

public class RoleAssigner : IRoleAssign
{
    public List<RoleBase> AllAssignRole { get; set; } = [];
    public IRandom<int> Random { get; set; }

    public bool Deduplication { get; set; } = false;
    
    public RoleBase Assign()
    {
        return null;
    }

    public IRoleAssign SetAssignRole(RoleBase role, PlayerControl player)
    {
        return this;
    }

    public RoleControllerBase AssignTo<T>(PlayerControl player) where T : RoleBase
    {
        CustomRoleManager.Instance.ShifterRole(player, Get<T>());
        return player.TryGetController<T>(out var control) ? control : null;
    }

    public IRoleAssign SetAssign(IEnumerable<RoleBase> bases)
    {
        AllAssignRole = bases.ToList();
        return this;
    }

    public IRoleAssign SetRandom(IRandom<int> random)
    {
        Random = random;
        return this;
    }

    public void Dispose()
    {
    }
}