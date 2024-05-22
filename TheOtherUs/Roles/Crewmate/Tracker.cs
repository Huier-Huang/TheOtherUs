using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherUs.Modules;
using TheOtherUs.Objects;
using TheOtherUs.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Crewmate;

[RegisterRole]
public class Tracker : RoleBase
{
    public Arrow arrow = new(Color.blue);

    private ResourceSprite buttonSprite = new("TrackerButton.png");
    public bool canTrackCorpses;
    public Color color = new Color32(100, 58, 220, byte.MaxValue);
    public float corpsesTrackingCooldown = 30f;
    public float corpsesTrackingDuration = 5f;
    public float corpsesTrackingTimer;

    public PlayerControl currentTarget;
    public List<Vector3> deadBodyPositions = [];
    public List<Arrow> localArrows = [];
    public bool resetTargetAfterMeeting;
    public float timeUntilUpdate;

    private ResourceSprite trackCorpsesButtonSprite = new("PathfindButton.png");
    public PlayerControl tracked;
    public PlayerControl tracker;

    public float updateIntervall = 5f;
    public bool usedTracker;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public void resetTracked()
    {
        currentTarget = tracked = null;
        usedTracker = false;
        if (arrow?.arrow != null) Object.Destroy(arrow.arrow);
        arrow = new Arrow(Color.blue);
        if (arrow.arrow != null) arrow.arrow.SetActive(false);
    }

    public override void ClearAndReload()
    {
        tracker = null;
        resetTracked();
        timeUntilUpdate = 0f;
        updateIntervall = CustomOptionHolder.trackerUpdateIntervall.getFloat();
        resetTargetAfterMeeting = CustomOptionHolder.trackerResetTargetAfterMeeting.getBool();
        if (localArrows != null)
            foreach (var a in localArrows.Where(a => a?.arrow != null))
                Object.Destroy(a.arrow);
        deadBodyPositions = [];
        corpsesTrackingTimer = 0f;
        corpsesTrackingCooldown = CustomOptionHolder.trackerCorpsesTrackingCooldown.getFloat();
        corpsesTrackingDuration = CustomOptionHolder.trackerCorpsesTrackingDuration.getFloat();
        canTrackCorpses = CustomOptionHolder.trackerCanTrackCorpses.getBool();
    }
}