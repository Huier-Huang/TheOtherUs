using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Pursuer : RoleBase
{
    public PlayerControl pursuer;
    public PlayerControl target;
    public Color color = GetColor<Lawyer>();
    public List<PlayerControl> blankedList = [];
    public int blanks;
    public ResourceSprite blank = new("PursuerButton.png");
    public bool notAckedExiled;

    public float cooldown = 30f;
    public int blanksNumber = 5;
    

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

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}