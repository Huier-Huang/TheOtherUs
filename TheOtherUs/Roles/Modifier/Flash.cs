using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public sealed class Flash : RoleBase
{
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Flash),
        RoleClassType = typeof(Flash),
        Color= Color.yellow,
        RoleId = RoleId.Flash,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Flash>,
        IntroInfo = "Super speed!",
        DescriptionText = "Super speed!",
        CreateRoleController = player => new FlashController(player)
    };
    public class FlashController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Flash>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }
}