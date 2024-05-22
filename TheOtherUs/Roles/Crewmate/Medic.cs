using System;
using TheOtherUs.Modules;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmate;

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

    public Color shieldedColor = new Color32(0, 221, 255, byte.MaxValue);
    public bool showAttemptToMedic;
    public bool showAttemptToShielded;
    public bool showShieldAfterMeeting;

    public int showShielded;
    public bool unbreakableShield = true;
    public bool usedShield;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

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
        reset = CustomOptionHolder.medicResetTargetAfterMeeting.getBool();
        showShielded = CustomOptionHolder.medicShowShielded.getSelection();
        showAttemptToShielded = CustomOptionHolder.medicShowAttemptToShielded.getBool();
        //      unbreakableShield = true; //CustomOptionHolder.medicBreakShield.getBool();
        unbreakableShield = CustomOptionHolder.medicBreakShield.getBool();
        showAttemptToMedic = CustomOptionHolder.medicShowAttemptToMedic.getBool();
        setShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.getSelection() == 2;
        showShieldAfterMeeting = CustomOptionHolder.medicSetOrShowShieldAfterMeeting.getSelection() == 1;
        meetingAfterShielding = false;
    }
}