using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Cursed : RoleBase
{
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Cursed),
        RoleClassType = typeof(Cursed),
        Color= new Color32(0, 247, 255, byte.MaxValue),
        RoleId = RoleId.Cursed,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Cursed>,
        IntroInfo = "You are crewmate....for now",
        DescriptionText = "Discover your true potential",
        CreateRoleController = player => new CursedController(player)
    };
    public class CursedController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Cursed>();
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