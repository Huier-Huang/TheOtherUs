using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Indomitable : RoleBase
{
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Indomitable),
        RoleClassType = typeof(Indomitable),
        Color= new Color32(0, 247, 255, byte.MaxValue),
        RoleId = RoleId.Indomitable,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Indomitable>,
        IntroInfo = "Your role cannot be guessed",
        DescriptionText = "You are Indomitable!",
        CreateRoleController = player => new IndomitableController(player)
    };
    public class IndomitableController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Indomitable>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }


    public override void ClearAndReload()
    {
    }
}