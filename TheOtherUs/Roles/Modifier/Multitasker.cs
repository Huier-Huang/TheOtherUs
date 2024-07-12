using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Multitasker : RoleBase
{
    public List<PlayerControl> multitasker = [];

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Multitasker),
        RoleClassType = typeof(Multitasker),
        Color= Color.yellow,
        RoleId = RoleId.Multitasker,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Multitasker>,
        IntroInfo = "Your task windows are transparent",
        DescriptionText = "Your task windows are transparent",
        CreateRoleController = player => new MultitaskerController(player)
    };
    public class MultitaskerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Multitasker>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        multitasker = [];
    }
}