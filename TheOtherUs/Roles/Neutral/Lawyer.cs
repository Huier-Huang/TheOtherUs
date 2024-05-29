using System;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Lawyer : RoleBase
{
    public bool canCallEmergency = true;
    public Color color = new Color32(134, 153, 25, byte.MaxValue);
    public bool isProsecutor;
    public PlayerControl lawyer;
    public bool lawyerKnowsRole;
    public PlayerControl target;
    public bool targetCanBeJester;
    public bool targetKnows;

    public bool targetWasGuessed;

    /*public ResourceSprite targetSprite;*/
    public bool triggerProsecutorWin;

    public float vision = 1f;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    /*public static Sprite getTargetSprite()
    {
        if (targetSprite) return targetSprite;
        targetSprite = Helpers.loadSpriteFromResources("", 150f);
        return targetSprite;
    }*/

    public override void ClearAndReload()
    {
        lawyer = null;

        isProsecutor = false;
        triggerProsecutorWin = false;
        vision = CustomOptionHolder.lawyerVision.getFloat();
        targetKnows = CustomOptionHolder.lawyerTargetKnows.getBool();
        lawyerKnowsRole = CustomOptionHolder.lawyerKnowsRole.getBool();
        targetCanBeJester = CustomOptionHolder.lawyerTargetCanBeJester.getBool();
        canCallEmergency = CustomOptionHolder.lawyerCanCallEmergency.getBool();
    }
}