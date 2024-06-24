using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Tracker : RoleBase
{
    public Arrow arrow = new(Color.blue);

    public ResourceSprite buttonSprite = new("TrackerButton.png");
    public ResourceSprite trackCorpsesButtonSprite = new("PathfindButton.png");
    public bool canTrackCorpses;
    public float corpsesTrackingCooldown = 30f;
    public float corpsesTrackingDuration = 5f;
    public float corpsesTrackingTimer;

    public PlayerControl currentTarget;
    public List<Vector3> deadBodyPositions = [];
    public List<Arrow> localArrows = [];
    public bool resetTargetAfterMeeting;
    public float timeUntilUpdate;
    
    public PlayerControl tracked;
    public PlayerControl tracker;

    public float updateIntervall = 5f;
    public bool usedTracker;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Tracker),
        RoleClassType = typeof(Tracker),
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Crewmate,
        RoleId = RoleId.Tracker,
        Color = new Color32(100, 58, 220, byte.MaxValue),
        GetRole = Get<Tracker>,
        DescriptionText = "Track the Impostors down",
        IntroInfo = "Track the <color=#FF1919FF>Impostors</color> down",
        CreateRoleController = n => new TrackerController(n)
    };
    
    public class TrackerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Tracker>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

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
        updateIntervall = CustomOptionHolder.trackerUpdateIntervall;
        resetTargetAfterMeeting = CustomOptionHolder.trackerResetTargetAfterMeeting;
        if (localArrows != null)
            foreach (var a in localArrows.Where(a => a?.arrow != null))
                Object.Destroy(a.arrow);
        deadBodyPositions = [];
        corpsesTrackingTimer = 0f;
        corpsesTrackingCooldown = CustomOptionHolder.trackerCorpsesTrackingCooldown;
        corpsesTrackingDuration = CustomOptionHolder.trackerCorpsesTrackingDuration;
        canTrackCorpses = CustomOptionHolder.trackerCanTrackCorpses;
    }
}