using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Lawyer : RoleBase
{
    public PlayerControl lawyer;
    public PlayerControl target;
    public Color color = new Color32(134, 153, 25, byte.MaxValue);
    /*public ResourceSprite targetSprite;*/
    public bool triggerProsecutorWin;
    public bool isProsecutor;
    public bool canCallEmergency = true;
    public bool targetKnows;

    public float vision = 1f;
    public bool lawyerKnowsRole;
    public bool targetCanBeJester;
    public bool targetWasGuessed;

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

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}