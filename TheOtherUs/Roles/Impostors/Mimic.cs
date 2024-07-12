using System.Collections.Generic;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Mimic : RoleBase
{
    public bool hasMimic;
    public List<PlayerControl> killed = [];
    public PlayerControl mimic;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Mimic),
        RoleClassType = typeof(Mimic),
        Color = Palette.ImpostorRed,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        RoleId = RoleId.Mimic,
        DescriptionText = "Pose as a crewmate",
        IntroInfo = "Pose as a crewmate by killing one",
        CreateRoleController = player => new MimicController(player)
    };
    
    public class MimicController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Mimic>();
    }
    public override CustomRoleOption roleOption { get; set; }


    public override void ClearAndReload()
    {
        mimic = null;
        hasMimic = false;
    }


    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }
}