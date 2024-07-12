using System.Collections.Generic;
using System.Linq;
using TheOtherUs.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Radar : RoleBase
{
    public PlayerControl ClosestPlayer;
    public List<Arrow> localArrows = [];
    public PlayerControl radar;
    public bool showArrows = true;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Radar),
        RoleClassType = typeof(Radar),
        Color= new Color32(255, 0, 128, byte.MaxValue),
        RoleId = RoleId.Radar,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Radar>,
        IntroInfo = "Be on high alert",
        DescriptionText = "Be on high alert",
        CreateRoleController = player => new RadarController(player)
    };
    public class RadarController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Radar>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        radar = null;
        showArrows = true;
        if (localArrows != null)
            foreach (var arrow in localArrows.Where(arrow => arrow?.arrow != null))
                Object.Destroy(arrow.arrow);
        localArrows = [];
    }
}