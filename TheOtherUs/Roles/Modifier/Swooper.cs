using System;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Swooper : RoleBase
{
    public PlayerControl swooper;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Swooper),
        RoleClassType = typeof(Swooper),
        Color= new Color32(224, 197, 219, byte.MaxValue),
        RoleId = RoleId.Swooper,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Swooper>,
        IntroInfo = "Turn Invisible and kill everyone",
        DescriptionText = "Turn Invisible",
        CreateRoleController = player => new SwooperController(player)
    };
    public class SwooperController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Swooper>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }


    public override void ClearAndReload()
    {
        swooper = null;
    }
}