using System;
using System.Collections.Generic;

namespace TheOtherRoles.Roles;

public abstract class RoleBase : IDisposable
{
    public int RoleIndex => CustomRoleManager.Instance._RoleBases.IndexOf(this);

    public abstract RoleInfo RoleInfo { get; protected set; }
    public abstract Type RoleType { get; protected set; }
    public List<RoleControllerBase> Controllers { get; protected set; } = [];
    public string ClassName { get; set; }

    public virtual void Dispose()
    {
    }

    public virtual bool CanAssign()
    {
        return true;
    }

    public virtual void ClearAndReload()
    {
    }

    public virtual void OptionCreate()
    {
    }

    public virtual void ButtonCreate(HudManager manager)
    {
    }


    public virtual void ResetCustomButton()
    {
    }
#nullable enable
    public Type? PathType { get; protected set; } = null;
}

public interface Invisable
{
    public bool isInvisable { get; set; }
}