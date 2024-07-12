using System;

namespace TheOtherUs.Roles.Vanilla;

[RegisterRole]
public class Crewmate : VanillaRole
{
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Crewmate),
        RoleClassType = typeof(Crewmate),
        Color= Palette.CrewmateBlue,
        RoleId = RoleId.Crewmate,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Crewmate,
        GetRole = Get<Crewmate>,
        IntroInfo = "Crewmate",
        DescriptionText = "Crewmate",
        CreateRoleController = player => new CrewmateController(player)
    };
    public class CrewmateController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Crewmate>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override bool IsVanilla { get; set; } = true;
    public override Type RoleType { get; set; } = typeof(CrewmateRole);
}