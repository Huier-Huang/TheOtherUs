using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Bait : RoleBase
{
    public List<PlayerControl> bait = new();
    public Dictionary<DeadPlayer, float> active = new();
    public Color color = new Color32(0, 247, 255, byte.MaxValue);

    public float reportDelayMin;
    public float reportDelayMax;
    public bool showKillFlash = true;

    public override void ClearAndReload()
    {
        bait = new List<PlayerControl>();
        active = new Dictionary<DeadPlayer, float>();
        reportDelayMin = CustomOptionHolder.modifierBaitReportDelayMin.getFloat();
        reportDelayMax = CustomOptionHolder.modifierBaitReportDelayMax.getFloat();
        if (reportDelayMin > reportDelayMax) reportDelayMin = reportDelayMax;
        showKillFlash = CustomOptionHolder.modifierBaitShowKillFlash.getBool();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}