using System;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Lawyer : RoleBase
{
    public bool canCallEmergency = true;
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

    public override CustomRoleOption roleOption { get; set; }

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Lawyer),
        RoleClassType = typeof(Lawyer),
        RoleId = RoleId.Lawyer,
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Lawyer>,
        Color =  new Color32(134, 153, 25, byte.MaxValue),
        IntroInfo = "Defend your client",
        DescriptionText = "Defend your client",
        CreateRoleController = player => new LawyerController(player)
    };
    
    public class LawyerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Lawyer>();
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

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
        vision = CustomOptionHolder.lawyerVision;
        targetKnows = CustomOptionHolder.lawyerTargetKnows;
        lawyerKnowsRole = CustomOptionHolder.lawyerKnowsRole;
        targetCanBeJester = CustomOptionHolder.lawyerTargetCanBeJester;
        canCallEmergency = CustomOptionHolder.lawyerCanCallEmergency;
    }
}