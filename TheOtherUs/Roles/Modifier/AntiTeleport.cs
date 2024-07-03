using System.Collections.Generic;
using TheOtherUs.Modules.Compatibility;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class AntiTeleport : RoleBase
{
    public List<PlayerControl> antiTeleport = [];
    public Vector3 position;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(AntiTeleport),
        RoleClassType = typeof(AntiTeleport),
        Color = Color.yellow,
        RoleId = RoleId.AntiTeleport,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<AntiTeleport>,
        IntroInfo = "You will not get teleported",
        DescriptionText = "You will not get teleported",
        CreateRoleController = player => new AntiTeleportController(player)
    };
    public class AntiTeleportController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<AntiTeleport>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        antiTeleport = [];
        position = Vector3.zero;
    }

    public void setPosition()
    {
        if (position == Vector3.zero)
            return; // Check if this has been set, otherwise first spawn on submerged will fail
        if (antiTeleport.FindAll(x => x.PlayerId == LocalPlayer.PlayerId).Count <= 0) return;

        LocalPlayer.NetTransform.RpcSnapTo(position);
        if (MapData.MapIs(Maps.Submerged)) SubmergedCompatibility.Instance.ChangeFloor(position.y > -7);
    }
}