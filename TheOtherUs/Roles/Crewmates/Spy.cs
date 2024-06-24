using System;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Spy : RoleBase
{
    public bool canEnterVents;
    public bool impostorsCanKillAnyone = true;
    public PlayerControl spy;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = Palette.ImpostorRed,
        CreateRoleController = n => new SpyController(n),
        DescriptionText = "Confuse the Impostors",
        IntroInfo = "Confuse the <color=#FF1919FF>Impostors</color>",
        GetRole = Get<Spy>,
        Name = nameof(Spy),
        RoleClassType = typeof(Spy),
        RoleId = RoleId.Spy,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class SpyController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Spy>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        spy = null;
        impostorsCanKillAnyone = CustomOptionHolder.spyImpostorsCanKillAnyone;
        canEnterVents = CustomOptionHolder.spyCanEnterVents;
    }
}