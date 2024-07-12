using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Jumper : RoleBase
{ // mint
    private ResourceSprite jumpButtonSprite = new ("JumperJumpButton.png");
    private ResourceSprite jumpMarkButtonSprite = new ("JumperButton.png");
    
    public PlayerControl jumper;

    //    public static float jumperChargesGainOnMeeting = 2f;
    //public static float jumperMaxCharges = 3f;
    public float jumperCharges = 1f;
    public float jumperChargesOnPlace = 1f;

    public float jumperJumpTime = 30f;

    public Vector3 jumpLocation;

    public bool resetPlaceAfterMeeting;
    public bool usedPlace;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(204, 155, 20, byte.MaxValue),
        GetRole = Get<Jumper>,
        CreateRoleController = n => new JumperController(n),
        DescriptionText = "Surprise the Impostors",
        IntroInfo = "Surprise the <color=#FF1919FF>Impostors</color>",
        Name = nameof(Jumper),
        RoleClassType = typeof(Jumper),
        RoleId = RoleId.Jumper,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class JumperController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Jumper>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public void resetPlaces()
    {
        jumperCharges = Mathf.RoundToInt(CustomOptionHolder.jumperChargesOnPlace);
        jumpLocation = Vector3.zero;
        usedPlace = false;
    }

    public override void ClearAndReload()
    {
        resetPlaces();
        jumpLocation = Vector3.zero;
        jumper = null;
        resetPlaceAfterMeeting = true;
        jumperCharges = 1f;
        jumperJumpTime = CustomOptionHolder.jumperJumpTime;
        jumperChargesOnPlace = CustomOptionHolder.jumperChargesOnPlace;
        usedPlace = false;
    }
}