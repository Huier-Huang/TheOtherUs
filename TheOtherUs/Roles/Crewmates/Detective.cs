using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Detective : RoleBase
{
    public bool anonymousFootprints;
    public PlayerControl detective;
    public CustomOption detectiveAnonymousFootprints;
    public CustomOption detectiveFootprintDuration;
    public CustomOption detectiveFootprintIntervall;
    public CustomOption detectiveReportColorDuration;
    public CustomOption detectiveReportNameDuration;

    public CustomOption detectiveSpawnRate;
    public float footprintDuration = 1f;

    public float footprintIntervall = 1f;
    public float reportColorDuration = 20f;
    public float reportNameDuration;
    public float timer = 6.2f;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(8, 180, 180, byte.MaxValue),
        GetRole = Get<Detective>,
        CreateRoleController = n => new DetectiveController(n),
        DescriptionText = "Examine footprints",
        IntroInfo = "Find the <color=#FF1919FF>Impostors</color> by examining footprints",
        Name = nameof(Detective),
        RoleClassType = typeof(Detective),
        RoleId = RoleId.Detective,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class DetectiveController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase  => Get<Detective>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        detective = null;
        anonymousFootprints = detectiveAnonymousFootprints;
        footprintIntervall = detectiveFootprintIntervall;
        footprintDuration = detectiveFootprintDuration;
        reportNameDuration = detectiveReportNameDuration;
        reportColorDuration = detectiveReportColorDuration;
        timer = 6.2f;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        detectiveAnonymousFootprints = roleOption.AddChild("Anonymous Footprints", new BoolOptionSelection(false));
        detectiveFootprintIntervall = roleOption.AddChild("Footprint Intervall", new FloatOptionSelection(0.5f, 0.25f, 10f, 0.25f));
        detectiveFootprintDuration = roleOption.AddChild("Footprint Duration", new FloatOptionSelection(5f, 0.25f, 10f, 0.25f));
        detectiveReportNameDuration = roleOption.AddChild("Time Where Investigator Reports Will Have Name", new FloatOptionSelection(0, 0, 60, 2.5f));
        detectiveReportColorDuration = roleOption.AddChild("Time Where Investigator Reports Will Have Color Type", new FloatOptionSelection(20, 0, 120, 2.5f));
    }
}