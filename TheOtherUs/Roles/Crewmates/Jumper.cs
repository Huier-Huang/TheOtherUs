using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Jumper : RoleBase
{
    public Color color = new Color32(204, 155, 20, byte.MaxValue); // mint
    private Sprite jumpButtonSprite;
    public PlayerControl jumper;

    //    public static float jumperChargesGainOnMeeting = 2f;
    //public static float jumperMaxCharges = 3f;
    public float jumperCharges = 1f;
    public float jumperChargesOnPlace = 1f;

    public float jumperJumpTime = 30f;

    public Vector3 jumpLocation;

    private Sprite jumpMarkButtonSprite;

    public bool resetPlaceAfterMeeting;
    public bool usedPlace;

    public override RoleInfo RoleInfo { get; protected set; }
    public override CustomRoleOption roleOption { get; set; }

    public Sprite getJumpMarkButtonSprite()
    {
        if (jumpMarkButtonSprite) return jumpMarkButtonSprite;
        jumpMarkButtonSprite = UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.JumperButton.png", 115f);
        return jumpMarkButtonSprite;
    }

    public Sprite getJumpButtonSprite()
    {
        if (jumpButtonSprite) return jumpButtonSprite;
        jumpButtonSprite = UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.JumperJumpButton.png", 115f);
        return jumpButtonSprite;
    }

    public void resetPlaces()
    {
        jumperCharges = Mathf.RoundToInt(CustomOptionHolder.jumperChargesOnPlace.getFloat());
        jumpLocation = Vector3.zero;
        usedPlace = false;
    }

    public override void ClearAndReload()
    {
        resetPlaces();
        jumpLocation = Vector3.zero;
        jumper = null;
        resetPlaceAfterMeeting = true;
        jumperCharges = 1f;
        jumperJumpTime = CustomOptionHolder.jumperJumpTime;
        jumperChargesOnPlace = CustomOptionHolder.jumperChargesOnPlace;
        usedPlace = false;
    }
}