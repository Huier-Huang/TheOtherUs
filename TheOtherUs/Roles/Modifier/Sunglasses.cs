using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Sunglasses : RoleBase
{
    public List<PlayerControl> sunglasses = [];
    public int vision = 1;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Sunglasses),
        RoleClassType = typeof(Sunglasses),
        Color= Color.yellow,
        RoleId = RoleId.Sunglasses,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Sunglasses>,
        IntroInfo = "You got the sunglasses",
        DescriptionText = "Your vision is reduced",
        CreateRoleController = player => new SunglassesController(player)
    };
    public class SunglassesController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Sunglasses>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        sunglasses = [];
        vision = CustomOptionHolder.modifierSunglassesVision.Selection + 1;
    }
}