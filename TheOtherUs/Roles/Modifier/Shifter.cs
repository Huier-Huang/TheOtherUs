using System;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Shifter : RoleBase
{
    private ResourceSprite buttonSprite = new("ShiftButton.png");
    public PlayerControl currentTarget;

    public PlayerControl futureShift;
    public PlayerControl shifter;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Shifter),
        RoleClassType = typeof(Shifter),
        Color= Color.gray,
        RoleId = RoleId.Shifter,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Shifter>,
        IntroInfo = "Shift your role",
        DescriptionText = "Shift your role",
        CreateRoleController = player => new ShifterController(player)
    };
    public class ShifterController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Shifter>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        shifter = null;
        currentTarget = null;
        futureShift = null;
    }
}