using System.Collections.Generic;
using System.Linq;
using TheOtherUs.Objects;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Follower : RoleBase
{
    public bool chatTarget = true;
    public bool chatTarget2 = true;
    public PlayerControl currentTarget;
    public PlayerControl follower;
    public bool getsAssassin;
    public List<Arrow> localArrows = [];

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Follower),
        RoleClassType = typeof(Follower),
        Color= Palette.ImpostorRed,
        RoleId = RoleId.Follower,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Impostor,
        GetRole = Get<Follower>,
        IntroInfo = "Follow your leader",
        DescriptionText = "Follow your leader",
        CreateRoleController = player => new FollowerController(player)
    };
    public class FollowerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Follower>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        if (localArrows != null)
            foreach (var arrow in localArrows.Where(arrow => arrow?.arrow != null))
                Object.Destroy(arrow.arrow);
        localArrows = [];
        follower = null;
        currentTarget = null;
        chatTarget = true;
        chatTarget2 = true;
        getsAssassin = CustomOptionHolder.modifierAssassinCultist;
    }
}