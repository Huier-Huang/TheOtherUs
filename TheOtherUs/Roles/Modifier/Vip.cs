using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Vip : RoleBase
{
    public bool showColor = true;
    public List<PlayerControl> vip = [];

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Vip),
        RoleClassType = typeof(Vip),
        Color= Color.yellow,
        RoleId = RoleId.Vip,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Vip>,
        IntroInfo = "You are the VIP",
        DescriptionText = "Everyone is notified when you die",
        CreateRoleController = player => new VipController(player)
    };
    public class VipController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Vip>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        vip = [];
        showColor = CustomOptionHolder.modifierVipShowColor;
    }
}