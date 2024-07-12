using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Prosecutor : RoleBase
{
    public override CustomRoleOption roleOption { get; set; }

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Prosecutor),
        RoleClassType = typeof(Prosecutor),
        RoleId = RoleId.Prosecutor,
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Prosecutor>,
        Color =  new Color32(134, 153, 25, byte.MaxValue),
        IntroInfo = "Vote out your target",
        DescriptionText = "Vote out your target",
        CreateRoleController = player => new ProsecutorController(player)
    };
    
    public class ProsecutorController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Prosecutor>();
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }
}