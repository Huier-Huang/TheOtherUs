
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;

namespace TheOtherUs.Roles;

public class CustomRoleManager : ManagerBase<CustomRoleManager>
{
    public readonly List<RoleBase> _AllActiveRole = [];

    public readonly List<RoleControllerBase> _AllControllerBases = [];
    public readonly List<RoleBase> _RoleBases = [];

    public readonly List<RoleControllerBase> LocalControllerBases = [];

    public readonly List<RoleBase> LocalRoleBases = [];

    public readonly Dictionary<RoleBase, List<PlayerControl>> PlayerAndRoles = new();

    #nullable enable
    public IRoleAssign? RoleAssigner { get; private set; }
    #nullable disable
    public IRoleAssign getOrCreateAssigner()
    {
        return RoleAssigner ?? new RoleAssigner();
    }

    public void SetAssigner(IRoleAssign assign)
    {
        if (RoleAssigner != null)
        {
            RoleAssigner.Dispose();
            RoleAssigner = null;
        }
        
        RoleAssigner = assign;
    }


    public readonly StreamWriter _Writer = new(File.Open(Path.Combine(Paths.GameRootPath, "RoleStrings.NexDat"), FileMode.OpenOrCreate));
    
    public void Register(RoleBase role)
    {
        _RoleBases.Add(role);
        if (role.IsVanilla) return;
        try
        {
            var info = role.RoleInfo;
            _Writer.WriteLine($"{info.InfoStringNode}.Name:{info.ShowName}");
            _Writer.WriteLine($"{info.InfoStringNode}.Intro:{info.Intro}");
            _Writer.WriteLine($"{info.InfoStringNode}.Description:{info.Description}");
        }
        catch (Exception e)
        {
            Error($"[Write Role String Error {role.ClassName}]:{e}");
        }
    }

    public void UnSetRole(RoleBase @base, PlayerControl player)
    {
        PlayerAndRoles[@base].Remove(player);
        UpdateActiveRole();
        if (player != LocalPlayer) return;
        var controllerBase = LocalControllerBases.FirstOrDefault(n => n._RoleBase == @base);
        if (controllerBase != null)
        {
            controllerBase.Dispose();
            LocalControllerBases.Remove(controllerBase);
        }

        LocalRoleBases.Remove(@base);
    }

    public void SetRole(RoleBase @base, PlayerControl player)
    {
        PlayerAndRoles[@base].Add(player);
        var controller = @base.RoleInfo.CreateRoleController(player);
        UpdateActiveRole();

        if (player != LocalPlayer) return;
        LocalRoleBases.Add(@base);
        LocalControllerBases.Add(controller);
    }

    public void UpdateActiveRole()
    {
        foreach (var dic in PlayerAndRoles)
        {
            if (dic.Value.Any() && !_AllActiveRole.Contains(dic.Key))
                _AllActiveRole.Add(dic.Key);

            if (!dic.Value.Any() && _AllActiveRole.Contains(dic.Key))
                _AllActiveRole.Remove(dic.Key);
        }
    }

    public void ShifterRole(PlayerControl player, RoleBase target)
    {
        UnSetRole(player.GetMainRole(), player);
        SetRole(target, player);
    }

    public void Update(HudManager __instance)
    {
        if (!_AllControllerBases.Any() || !_AllActiveRole.Any()) return;

        foreach (var Base in _AllControllerBases) Base.Update(__instance);
    }

    public void ClearAndReload()
    {
        foreach (var role in _RoleBases)
            role.ClearAndReload();
    }
}