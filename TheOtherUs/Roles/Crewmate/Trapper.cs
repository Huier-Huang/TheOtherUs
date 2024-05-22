using System;
using System.Collections.Generic;
using TheOtherUs.Modules;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmate;

[RegisterRole]
public class Trapper : RoleBase
{
    public bool anonymousMap;
    public int charges = 1;
    public Color color = new Color32(110, 57, 105, byte.MaxValue);

    public float cooldown = 30f;
    public int infoType; // 0 = Role, 1 = Good/Evil, 2 = Name
    public int maxCharges = 5;
    public List<PlayerControl> playersOnMap = [];
    public int rechargedTasks = 3;
    public int rechargeTasksNumber = 3;

    private ResourceSprite trapButtonSprite = new("Trapper_Place_Button.png");
    public int trapCountToReveal = 2;
    public float trapDuration = 5f;
    public PlayerControl trapper;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        trapper = null;
        cooldown = CustomOptionHolder.trapperCooldown.getFloat();
        maxCharges = Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges.getFloat());
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber.getFloat());
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.trapperRechargeTasksNumber.getFloat());
        charges = Mathf.RoundToInt(CustomOptionHolder.trapperMaxCharges.getFloat()) / 2;
        trapCountToReveal = Mathf.RoundToInt(CustomOptionHolder.trapperTrapNeededTriggerToReveal.getFloat());
        playersOnMap = [];
        anonymousMap = CustomOptionHolder.trapperAnonymousMap.getBool();
        infoType = CustomOptionHolder.trapperInfoType.getSelection();
        trapDuration = CustomOptionHolder.trapperTrapDuration.getFloat();
    }
}