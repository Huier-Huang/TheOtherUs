using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Torch : RoleBase
{
    public List<PlayerControl> torch = [];
    public float vision = 1;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Torch),
        RoleClassType = typeof(Torch),
        Color= Color.yellow,
        RoleId = RoleId.Torch,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Torch>,
        IntroInfo = "You got the torch",
        DescriptionText = "You can see in the dark",
        CreateRoleController = player => new TorchController(player)
    };
    public class TorchController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Torch>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        torch = [];
        vision = CustomOptionHolder.modifierTorchVision;
    }
}