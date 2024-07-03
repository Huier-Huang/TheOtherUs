using System.Collections.Generic;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Poucher : RoleBase
{
    public List<PlayerControl> killed = [];


    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Poucher),
        RoleClassType = typeof(Poucher),
        RoleId = RoleId.Poucher,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Crewmate,
        GetRole = Get<Poucher>,
        Color = Palette.ImpostorRed,
        IntroInfo = "Keep info on the players you kill",
        DescriptionText = "Investigate the kills",
        CreateRoleController = player => new PoucherController(player)
    };
    public class PoucherController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Poucher>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        killed = [];
    }
}