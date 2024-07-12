using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Escapist : RoleBase
{
    public float ChargesOnPlace = 1f;
    private readonly ResourceSprite escapeButtonSprite = new("Recall.png");
    public Vector3 escapeLocation;

    private readonly ResourceSprite escapeMarkButtonSprite = new("Mark.png");
    public float EscapeTime = 30f;

    public PlayerControl escapist;

    private CustomButton escapistButton;

    public float escapistCharges = 1f;
    public CustomOption escapistChargesOnPlace;
    public CustomOption escapistEscapeTime;
    public CustomOption escapistResetPlaceAfterMeeting;
    
    public bool resetPlaceAfterMeeting;
    public bool usedPlace;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Escapist),
        RoleClassType = typeof(Escapist),
        Color = Palette.ImpostorRed,
        RoleId = RoleId.Escapist,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Escapist>,
        DescriptionText = "Teleport to get away from bodies",
        IntroInfo = "Get away from kills with ease",
        CreateRoleController = player => new EscapistController(player)
    };
    
    public class EscapistController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Escapist>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public void resetPlaces()
    {
        escapistCharges = Mathf.RoundToInt(escapistChargesOnPlace);
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
        EscapeTime = escapistEscapeTime;
        ChargesOnPlace = escapistChargesOnPlace;
        usedPlace = false;
    }

    public override void OptionCreate()
    {
        escapistEscapeTime = roleOption.AddChild("Mark and Escape Cooldown", new IntOptionSelection(30, 0, 60, 5));
        escapistChargesOnPlace = roleOption.AddChild("Charges On Place", new IntOptionSelection(1, 1, 10, 1));

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
            DefButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        escapistButton.MaxTimer = escapistEscapeTime;
    }
}