using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Medic : RoleBase
{
    private ResourceSprite buttonSprite = new("ShieldButton.png");

    public Color color = new Color32(126, 251, 194, byte.MaxValue);
    public PlayerControl currentTarget;
    public PlayerControl futureShielded;
    public PlayerControl medic;
    public bool meetingAfterShielding;
    public bool reset;
    public bool setShieldAfterMeeting;
    public PlayerControl shielded;
    
    public bool showAttemptToMedic;
    public bool showAttemptToShielded;
    public bool showShieldAfterMeeting;

    public int showShielded;
    public bool unbreakableShield = true;
    public bool usedShield;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(0, 221, 255, byte.MaxValue),
        GetRole = Get<Medic>,
        CreateRoleController = n => new MedicController(n),
        DescriptionText = "Protect other players",
        IntroInfo = "Protect someone with your shield",
        Name = nameof(Medic),
        RoleClassType = typeof(Medic),
        RoleId = RoleId.Medic,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class MedicController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Medic>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public void resetShielded()
    {
        currentTarget = shielded = null;
        usedShield = false;
    }

    public override void ClearAndReload()
    {
        medic = null;
        shielded = null;
        futureShielded = null;
        currentTarget = null;
        usedShield = false;
        reset = CustomOptionHolder.medicResetTargetAfterMeeting;
        showShielded = CustomOptionHolder.medicShowShielded;
        showAttemptToShielded = CustomOptionHolder.medicShowAttemptToShielded;
        unbreakableShield = CustomOptionHolder.medicBreakShield;
        showAttemptToMedic = CustomOptionHolder.medicShowAttemptToMedic;
        setShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.Selection == 2;
        showShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.Selection == 1;
        meetingAfterShielding = false;
    }
}