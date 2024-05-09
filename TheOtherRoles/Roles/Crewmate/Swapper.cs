using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Swapper : RoleBase
{
    public PlayerControl swapper;
    public Color color = new Color32(134, 55, 86, byte.MaxValue);
    private ResourceSprite spriteCheck = new ("SwapperCheck.png", 150f);
    public bool canCallEmergency;
    public bool canOnlySwapOthers;
    public int charges;
    public float rechargeTasksNumber;
    public bool canFixSabotages;
    public float rechargedTasks;

    public byte playerId1 = byte.MaxValue;
    public byte playerId2 = byte.MaxValue;

    public override void ClearAndReload()
    {
        swapper = null;
        playerId1 = byte.MaxValue;
        playerId2 = byte.MaxValue;
        canCallEmergency = CustomOptionHolder.swapperCanCallEmergency.getBool();
        canOnlySwapOthers = CustomOptionHolder.swapperCanOnlySwapOthers.getBool();
        canFixSabotages = CustomOptionHolder.swapperCanFixSabotages.getBool();
        charges = Mathf.RoundToInt(CustomOptionHolder.swapperSwapsNumber.getFloat());
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.swapperRechargeTasksNumber.getFloat());
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.swapperRechargeTasksNumber.getFloat());
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}