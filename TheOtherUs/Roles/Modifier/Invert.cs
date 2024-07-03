using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Invert : RoleBase
{
    public List<PlayerControl> invert = [];
    public int meetings = 3;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Invert),
        RoleClassType = typeof(Invert),
        Color = Color.yellow,
        RoleId = RoleId.Invert,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Invert>,
        IntroInfo = "Your movement is inverted",
        DescriptionText = "Your movement is inverted",
        CreateRoleController = player => new InvertController(player)
    };
    public class InvertController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Invert>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        invert = [];
        meetings = (int)CustomOptionHolder.modifierInvertDuration;
    }
}