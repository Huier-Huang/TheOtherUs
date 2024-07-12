using System;

namespace TheOtherUs.Roles.Vanilla;

[RegisterRole]
public class Impostor : VanillaRole
{
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Impostor),
        RoleClassType = typeof(Impostor),
        Color= Palette.ImpostorRed,
        RoleId = RoleId.Impostor,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Impostor,
        GetRole = Get<Impostor>,
        IntroInfo = "Impostor",
        DescriptionText = "Impostor",
        CreateRoleController = player => new ImpostorController(player)
    };
    public class ImpostorController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Impostor>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override bool IsVanilla { get; set; } = true;
    public override Type RoleType { get; set; } = typeof(ImpostorRole);

}