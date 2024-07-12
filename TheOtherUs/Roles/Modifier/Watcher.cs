using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Watcher : RoleBase
{
    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Watcher),
        RoleClassType = typeof(Watcher),
        RoleTeam = RoleTeam.Special,
        RoleType = CustomRoleType.Modifier,
        Color = new Color32(48, 21, 89, byte.MaxValue),
        RoleId = RoleId.Watcher,
        GetRole = Get<Watcher>,
        DescriptionText = "Pay close attention to the crew's votes",
        IntroInfo = "You can see everyone's votes during meetings",
        CreateRoleController = player => new WatcherController(player)
    };
    
    public class WatcherController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Watcher>();
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