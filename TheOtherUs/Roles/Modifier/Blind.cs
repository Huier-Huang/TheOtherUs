using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Blind : RoleBase
{
    public static PlayerControl blind;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Blind),
        RoleClassType = typeof(Blind),
        Color= new Color32(48, 21, 89, byte.MaxValue),
        RoleId = RoleId.Blind,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Blind>,
        IntroInfo = "You cannot see your report button!",
        DescriptionText = "Was that a dead body?",
        CreateRoleController = player => new BlindController(player)
    };
    public class BlindController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Blind>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }


    public override void ClearAndReload()
    {
        blind = null;
    }
}