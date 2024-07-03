using System;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Tunneler : RoleBase
{
    public PlayerControl tunneler;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Tunneler),
        RoleClassType = typeof(Tunneler),
        Color= new Color32(48, 21, 89, byte.MaxValue),
        RoleId = RoleId.Tunneler,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Tunneler>,
        IntroInfo = "Complete your tasks to gain the ability to vent",
        DescriptionText = "Finish work so you can play",
        CreateRoleController = player => new TunnelerController(player)
    };
    public class TunnelerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Tunneler>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }


    public override void ClearAndReload()
    {
        tunneler = null;
    }
}