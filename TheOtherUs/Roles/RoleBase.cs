using System;
using System.Collections.Generic;
using TheOtherUs.Options;

namespace TheOtherUs.Roles;

public abstract class RoleBase : IDisposable
{
    public int RoleIndex => CustomRoleManager.Instance._RoleBases.IndexOf(this);

    public abstract RoleInfo RoleInfo { get; protected set; }
    public abstract CustomRoleOption roleOption { get; set; }
    public List<RoleControllerBase> Controllers { get; protected set; } = [];
    public string ClassName => RoleInfo.RoleClassType.Name;

    public virtual bool HasImpostorVision { get; set; } = false;
    public virtual bool IsKiller { get; set; } = false;
    public virtual bool IsEvil { get; set; } = false;

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