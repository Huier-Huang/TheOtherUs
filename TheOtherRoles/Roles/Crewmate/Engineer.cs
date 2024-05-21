using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Engineer : RoleBase
{
    public static readonly RoleInfo Info = new()
    {
        Name = nameof(Engineer),
        Color = new Color32(0, 40, 245, byte.MaxValue),
        Description = "Repair the ship",
        IntroInfo = "Maintain important systems on the ship",
        GetRole = Get<Engineer>,
        RoleClassType = typeof(Engineer),
        RoleId = RoleId.Engineer,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };

    private Sprite buttonSprite;
    public Color color = new Color32(0, 40, 245, byte.MaxValue);
    public PlayerControl engineer;
    public bool highlightForImpostors = true;
    public bool highlightForTeamJackal = true;
    public int remainingFixes = 1;

    public bool remoteFix = true;

    public bool resetFixAfterMeeting;

    public bool usedFix;

    public override RoleInfo RoleInfo { get; protected set; } = Info;
    public override Type RoleType { get; protected set; }

    public Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = UnityHelper.loadSpriteFromResources("TheOtherRoles.Resources.RepairButton.png", 115f);
        return buttonSprite;
    }

    public void resetFixes()
    {
        remainingFixes = Mathf.RoundToInt(CustomOptionHolder.engineerNumberOfFixes.getFloat());
        usedFix = false;
    }

    public override void ClearAndReload()
    {
        engineer = null;
        resetFixes();
        remoteFix = CustomOptionHolder.engineerRemoteFix.getBool();
        //expertRepairs = CustomOptionHolder.engineerExpertRepairs.getBool();
        resetFixAfterMeeting = CustomOptionHolder.engineerResetFixAfterMeeting.getBool();
        remainingFixes = Mathf.RoundToInt(CustomOptionHolder.engineerNumberOfFixes.getFloat());
        highlightForImpostors = CustomOptionHolder.engineerHighlightForImpostors.getBool();
        highlightForTeamJackal = CustomOptionHolder.engineerHighlightForTeamJackal.getBool();
        usedFix = false;
    }
}