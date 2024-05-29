using System;
using System.Collections.Generic;

namespace TheOtherUs.Roles.Assigns;

public interface IRoleAssign : IDisposable
{
    public RoleBase Assign();

    public IRoleAssign SetAssign(IEnumerable<RoleBase> bases);
}