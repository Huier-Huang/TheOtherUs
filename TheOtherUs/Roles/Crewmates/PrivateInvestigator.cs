using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class PrivateInvestigator : RoleBase
{
    private ResourceSprite buttonSprite = new("Watch.png");
    public PlayerControl currentTarget;
    public PlayerControl privateInvestigator;


    public bool seeFlashColor;
    public PlayerControl watching;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(77, 77, 255, byte.MaxValue),
        GetRole = Get<PrivateInvestigator>,
        CreateRoleController = n => new PrivateInvestigatorController(n),
        DescriptionText = "Spy on the ship.",
        IntroInfo = "See who is interacting with others",
        Name = nameof(PrivateInvestigator),
        RoleClassType = typeof(PrivateInvestigator),
        RoleId = RoleId.PrivateInvestigator,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class PrivateInvestigatorController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<PrivateInvestigator>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        privateInvestigator = null;
        watching = null;
        currentTarget = null;
        seeFlashColor = CustomOptionHolder.privateInvestigatorSeeColor;
    }
}