using System;
using System.Collections.Generic;
using Hazel;
using TheOtherRoles.Modules.Options;
using TheOtherRoles.Objects;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Escapist : RoleBase
{
    public PlayerControl escapist;
    public Color color = Palette.ImpostorRed;
    public float EscapeTime = 30f;
    public float ChargesOnPlace = 1f;
    public bool resetPlaceAfterMeeting;
    //public float escapistChargesGainOnMeeting = 2f;
    //public float escapistMaxCharges = 3f;
    public float escapistCharges = 1f;
    public Vector3 escapeLocation;

    private ResourceSprite escapeMarkButtonSprite = new ("Mark.png");
    private ResourceSprite escapeButtonSprite = new ("Recall.png");
    public bool usedPlace;

    public CustomOption escapistSpawnRate;
    public CustomOption escapistEscapeTime;
    public CustomOption escapistChargesOnPlace;
    public CustomOption escapistResetPlaceAfterMeeting;
    //public CustomOption escapistChargesGainOnMeeting;
    //public CustomOption escapistMaxCharges;

    private CustomButton escapistButton;

    public void resetPlaces()
    {
        escapistCharges = Mathf.RoundToInt(escapistChargesOnPlace.getFloat());
        escapeLocation = Vector3.zero;
        usedPlace = false;
    }

    public override void ClearAndReload()
    {
        resetPlaces();
        escapeLocation = Vector3.zero;
        escapist = null;
        resetPlaceAfterMeeting = true;
        escapistCharges = 1f;
        EscapeTime = escapistEscapeTime.getFloat();
        ChargesOnPlace = escapistChargesOnPlace.getFloat();
        //escapistChargesGainOnMeeting = escapistChargesGainOnMeeting.getFloat();
        //escapistMaxCharges = escapistMaxCharges.getFloat();
        usedPlace = false;
    }
    public override void OptionCreate()
    {
        escapistSpawnRate = new CustomOption(905000, "Escapist".ColorString(color), CustomOptionHolder.rates, null, true);
        escapistEscapeTime = new CustomOption(905100, "Mark and Escape Cooldown", 30, 0, 60, 5, escapistSpawnRate);
        escapistChargesOnPlace = new CustomOption(905200, "Charges On Place", 1, 1, 10, 1, escapistSpawnRate);
        //escapistResetPlaceAfterMeeting = new CustomOption(9052, "Reset Places After Meeting", true, jumperSpawnRate);
        //escapistChargesGainOnMeeting = new CustomOption(9053, "Charges Gained After Meeting", 2, 0, 10, 1, jumperSpawnRate);
        //escapistMaxCharges = new CustomOption(905400, "Maximum Charges", 3, 0, 10, 1, escapistSpawnRate);
    }
    public override void ButtonCreate(HudManager _hudManager)
    {

        // Escapist Escape
        escapistButton = new CustomButton(
            () =>
            {
                if (escapeLocation == Vector3.zero)
                {
                    //set location
                    escapeLocation = PlayerControl.LocalPlayer.transform.localPosition;
                    escapistButton.Sprite = escapeButtonSprite;
                    escapistCharges = escapistChargesOnPlace;
                }
                else if (escapistCharges >= 1f)
                {
                    //teleport to location if you have one
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.SetPositionESC, SendOption.Reliable);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write(escapeLocation.x);
                    writer.Write(escapeLocation.y);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    PlayerControl.LocalPlayer.transform.position = escapeLocation;


                    escapistCharges -= 1f;
                }

                if (escapistCharges > 0) escapistButton.Timer = escapistButton.MaxTimer;
            },
            () =>
            {
                return escapist != null && escapist == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                //   if (jumperChargesText != null) jumperChargesText.text = $"{Jumper.jumperCharges}";
                usedPlace = true;
                return (escapeLocation == Vector3.zero || escapistCharges >= 1f) &&
                       PlayerControl.LocalPlayer.CanMove;
            },
            () =>
            {
                if (resetPlaceAfterMeeting) resetPlaces();
                {
                    escapistButton.Sprite = escapeMarkButtonSprite;
                }
                //    Jumper.jumperCharges += Jumper.jumperChargesGainOnMeeting;
                //if (Escapist.escapistCharges > Escapist.escapistMaxCharges) Escapist.escapistCharges = Escapist.escapistMaxCharges;

                if (escapistCharges > 0) escapistButton.Timer = escapistButton.MaxTimer;
            },
            escapeMarkButtonSprite,
            CustomButton.ButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.F
        );
    }
    public override void ResetCustomButton()
    {
        escapistButton.MaxTimer = escapistEscapeTime;

    }
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    
}