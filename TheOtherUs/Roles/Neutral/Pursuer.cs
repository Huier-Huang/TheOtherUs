using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Pursuer : RoleBase
{
    public ResourceSprite blank = new("PursuerButton.png");
    public List<PlayerControl> blankedList = [];
    public int blanks;
    public int blanksNumber = 5;

    public float cooldown = 30f;
    public bool notAckedExiled;
    public PlayerControl pursuer;
    public PlayerControl target;

    public override CustomRoleOption roleOption { get; set; }

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Pursuer),
        RoleClassType = typeof(Pursuer),
        RoleId = RoleId.Pursuer,
        RoleTeam = RoleTeam.Neutral,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Pursuer>,
        Color =  new Color32(134, 153, 25, byte.MaxValue),
        IntroInfo = "Blank the Impostors",
        DescriptionText = "Blank the Impostors",
        CreateRoleController = player => new PursuerController(player)
    };
    
    public class PursuerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Pursuer>();
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }


    public override void ClearAndReload()
    {
        pursuer = null;
        target = null;
        blankedList = [];
        blanks = 0;
        notAckedExiled = false;

        cooldown = CustomOptionHolder.pursuerCooldown;
        blanksNumber = Mathf.RoundToInt(CustomOptionHolder.pursuerBlanksNumber);
    }
}