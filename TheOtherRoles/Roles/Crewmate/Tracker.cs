using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Tracker : RoleBase
{
    public PlayerControl tracker;
    public Color color = new Color32(100, 58, 220, byte.MaxValue);
    public List<Arrow> localArrows = new();

    public float updateIntervall = 5f;
    public bool resetTargetAfterMeeting;
    public bool canTrackCorpses;
    public float corpsesTrackingCooldown = 30f;
    public float corpsesTrackingDuration = 5f;
    public float corpsesTrackingTimer;
    public List<Vector3> deadBodyPositions = new();

    public PlayerControl currentTarget;
    public PlayerControl tracked;
    public bool usedTracker;
    public float timeUntilUpdate;
    public Arrow arrow = new(Color.blue);

    private ResourceSprite trackCorpsesButtonSprite = new ("PathfindButton.png");

    private ResourceSprite buttonSprite = new ("TrackerButton.png");

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
        deadBodyPositions = new List<Vector3>();
        corpsesTrackingTimer = 0f;
        corpsesTrackingCooldown = CustomOptionHolder.trackerCorpsesTrackingCooldown.getFloat();
        corpsesTrackingDuration = CustomOptionHolder.trackerCorpsesTrackingDuration.getFloat();
        canTrackCorpses = CustomOptionHolder.trackerCanTrackCorpses.getBool();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}