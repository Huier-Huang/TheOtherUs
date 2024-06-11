using System;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Swapper : RoleBase
{
    public bool canCallEmergency;
    public bool canFixSabotages;
    public bool canOnlySwapOthers;
    public int charges;
    public Color color = new Color32(134, 55, 86, byte.MaxValue);

    public byte playerId1 = byte.MaxValue;
    public byte playerId2 = byte.MaxValue;
    public float rechargedTasks;
    public float rechargeTasksNumber;
    private ResourceSprite spriteCheck = new("SwapperCheck.png", 150f);
    public PlayerControl swapper;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

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
}