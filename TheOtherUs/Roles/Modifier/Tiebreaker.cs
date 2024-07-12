using System;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Tiebreaker : RoleBase
{
    public bool isTiebreak;
    public PlayerControl tiebreaker;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Tiebreaker),
        RoleClassType = typeof(Tiebreaker),
        Color= Color.cyan,
        RoleId = RoleId.Tiebreaker,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Tiebreaker>,
        IntroInfo = "Your vote breaks the tie",
        DescriptionText = "Break the tie",
        CreateRoleController = player => new TiebreakerController(player)
    };
    public class TiebreakerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Tiebreaker>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        tiebreaker = null;
        isTiebreak = false;
    }
}