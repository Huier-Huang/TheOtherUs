using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Slueth : RoleBase
{
    public List<PlayerControl> reported = [];
    public PlayerControl slueth;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Slueth),
        RoleClassType = typeof(Slueth),
        Color= new Color32(48, 21, 89, byte.MaxValue),
        RoleId = RoleId.Slueth,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Slueth>,
        IntroInfo = "Learn the roles of bodies you report",
        DescriptionText = "You know the roles of bodies you report",
        CreateRoleController = player => new SluethController(player)
    };
    public class SluethController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Slueth>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        slueth = null;
        reported = [];
    }
}