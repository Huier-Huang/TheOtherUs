using System;
using System.Collections.Generic;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Bait : RoleBase
{
    public Dictionary<DeadPlayer, float> active = new();
    public List<PlayerControl> bait = [];
    public Color color = new Color32(0, 247, 255, byte.MaxValue);
    public float reportDelayMax;

    public float reportDelayMin;
    public bool showKillFlash = true;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        bait = [];
        active = new Dictionary<DeadPlayer, float>();
        reportDelayMin = CustomOptionHolder.modifierBaitReportDelayMin.getFloat();
        reportDelayMax = CustomOptionHolder.modifierBaitReportDelayMax.getFloat();
        if (reportDelayMin > reportDelayMax) reportDelayMin = reportDelayMax;
        showKillFlash = CustomOptionHolder.modifierBaitShowKillFlash.getBool();
    }
}