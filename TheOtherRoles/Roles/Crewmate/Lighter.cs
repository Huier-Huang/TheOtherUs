using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Lighter : RoleBase
{
    public static PlayerControl lighter;
    public static Color color = new Color32(238, 229, 190, byte.MaxValue);

    public static float lighterModeLightsOnVision = 2f;
    public static float lighterModeLightsOffVision = 0.75f;
    public static float flashlightWidth = 0.75f;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        lighter = null;
        flashlightWidth = CustomOptionHolder.lighterFlashlightWidth.getFloat();
        lighterModeLightsOnVision = CustomOptionHolder.lighterModeLightsOnVision.getFloat();
        lighterModeLightsOffVision = CustomOptionHolder.lighterModeLightsOffVision.getFloat();
    }
}