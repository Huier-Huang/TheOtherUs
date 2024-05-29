using System.Collections.Generic;

namespace TheOtherUs.Roles.Assigns;

public class RoleAssigner : IRoleAssign
{
    public RoleBase Assign()
    {
        return null;
    }

    public IRoleAssign SetAssign(IEnumerable<RoleBase> bases)
    {
        return this;
    }

    public void Dispose()
    {
    }
}