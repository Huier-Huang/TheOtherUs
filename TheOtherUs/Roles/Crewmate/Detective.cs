using System;
using TheOtherUs.Options;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmate;

[RegisterRole]
public class Detective : RoleBase
{
    public bool anonymousFootprints;
    public Color color = new Color32(8, 180, 180, byte.MaxValue);
    public PlayerControl detective;
    public CustomOption detectiveAnonymousFootprints;
    public CustomOption detectiveFootprintDuration;
    public CustomOption detectiveFootprintIntervall;
    public CustomOption detectiveReportColorDuration;
    public CustomOption detectiveReportNameDuration;

    public CustomOption detectiveSpawnRate;
    public float footprintDuration = 1f;

    public float footprintIntervall = 1f;
    public float reportColorDuration = 20f;
    public float reportNameDuration;
    public float timer = 6.2f;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        detective = null;
        anonymousFootprints = detectiveAnonymousFootprints.getBool();
        footprintIntervall = detectiveFootprintIntervall.getFloat();
        footprintDuration = detectiveFootprintDuration.getFloat();
        reportNameDuration = detectiveReportNameDuration.getFloat();
        reportColorDuration = detectiveReportColorDuration.getFloat();
        timer = 6.2f;
    }

    public override void OptionCreate()
    {
        detectiveSpawnRate =
            new CustomOption(120, "Investigator".ColorString(color), CustomOptionHolder.rates, null, true);
        detectiveAnonymousFootprints =
            new CustomOption(121, "Anonymous Footprints", false, detectiveSpawnRate);
        detectiveFootprintIntervall = new CustomOption(122, "Footprint Intervall", 0.5f, 0.25f, 10f,
            0.25f, detectiveSpawnRate);
        detectiveFootprintDuration = new CustomOption(123, "Footprint Duration", 5f, 0.25f, 10f,
            0.25f, detectiveSpawnRate);
        detectiveReportNameDuration = new CustomOption(124,
            "Time Where Investigator Reports Will Have Name", 0, 0, 60, 2.5f, detectiveSpawnRate);
        detectiveReportColorDuration = new CustomOption(125,
            "Time Where Investigator Reports Will Have Color Type", 20, 0, 120, 2.5f, detectiveSpawnRate);
    }
}