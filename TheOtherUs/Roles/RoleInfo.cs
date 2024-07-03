using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles;

public class RoleInfo
{
    private static readonly List<RoleInfo> _AllRoleInfo = [];

    public RoleInfo()
    {
        _AllRoleInfo.Add(this);
    }

    public static IReadOnlyList<RoleInfo> AllRoleInfo => _AllRoleInfo;

    public Color Color { get; set; }
    public string Name { get; set; }
    public RoleId RoleId { get; set; }
    public string DescriptionText { get; set; } = string.Empty;
    public string IntroInfo { get; set; } = string.Empty;
    public RoleTeam RoleTeam { get; set; }
    public string RoleClassName => RoleClassType.Name;

    public string InfoStringNode => Enum.GetName(RoleId) ?? RoleClassName + ".Info";

    public string ShowName
    { 
        get
        {
            var tr = Get(InfoStringNode,"Name");
            if (tr == string.Empty)
                tr = Get(RoleClassName);
            
            return tr != string.Empty ? tr : Name;
        } 
    }

    public string Intro
    {
        get
        {
            var tr = Get(InfoStringNode, "Intro");
            return tr != string.Empty ? tr : IntroInfo;
        }
    }

    public string Description
    {
        get
        {
            var tr = Get(InfoStringNode, "Description");
            return tr != string.Empty ? tr : DescriptionText;
        }
    }

    public CustomRoleType RoleType { get; set; } = CustomRoleType.Main;
    public Func<RoleBase> GetRole { get; set; }

    public Func<PlayerControl, RoleControllerBase> CreateRoleController { get; set; }

    public Type RoleClassType { get; set; }
}
[Flags]
public enum RoleTeam
{
    Crewmate = 1,
    Impostor = 2,
    Neutral = 5,
    Special = 10
}

[Flags]
public enum CustomRoleType
{
    Main = 1,
    Modifier = 2,
    ModeRole = 5,
    MainAndModifier = Main | Modifier 
}