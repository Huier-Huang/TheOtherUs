using System;
using System.Collections.Generic;
using TheOtherUs.Modules;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs.Roles.Neutral;

[RegisterRole]
public class Pursuer : RoleBase
{
    public ResourceSprite blank = new("PursuerButton.png");
    public List<PlayerControl> blankedList = [];
    public int blanks;
    public int blanksNumber = 5;
    public Color color = GetColor<Lawyer>();

    public float cooldown = 30f;
    public bool notAckedExiled;
    public PlayerControl pursuer;
    public PlayerControl target;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        pursuer = null;
        target = null;
        blankedList = [];
        blanks = 0;
        notAckedExiled = false;

        cooldown = CustomOptionHolder.pursuerCooldown.getFloat();
        blanksNumber = Mathf.RoundToInt(CustomOptionHolder.pursuerBlanksNumber.getFloat());
    }
}