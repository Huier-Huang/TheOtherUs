using TheOtherUs.Objects;
using UnityEngine;
using Object = UnityEngine.Object;
namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Ninja : RoleBase
{
    public Arrow arrow = new(Color.black);

    public float cooldown = 30f;
    public PlayerControl currentTarget;
    public float invisibleDuration = 5f;

    public float invisibleTimer;
    public bool isInvisble;
    private readonly ResourceSprite KillButtonSprite = new("NinjaAssassinateButton.png");

    public bool knowsTargetLocation;

    private readonly ResourceSprite MarkButtonSprite = new("NinjaMarkButton.png");
    public PlayerControl ninja;
    public CustomButton ninjaButton;
    public CustomOption ninjaCooldown;
    public CustomOption ninjaInvisibleDuration;
    public CustomOption ninjaKnowsTargetLocation;

    public PlayerControl ninjaMarked;

    public CustomOption ninjaTraceColorTime;
    public CustomOption ninjaTraceTime;
    public float traceTime = 1f;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Ninja),
        RoleClassType = typeof(Ninja),
        RoleId = RoleId.Ninja,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Ninja>,
        Color = Palette.ImpostorRed,
        IntroInfo = "Surprise and assassinate your foes",
        DescriptionText = "Surprise and assassinate your foes",
        CreateRoleController = player => new NinjaController(player)
    };
    
    public class NinjaController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Ninja>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        ninja = null;
        currentTarget = ninjaMarked = null;
        cooldown = ninjaCooldown;
        knowsTargetLocation = ninjaKnowsTargetLocation;
        traceTime = ninjaTraceTime;
        invisibleDuration = ninjaInvisibleDuration;
        invisibleTimer = 0f;
        isInvisble = false;
        if (arrow?.arrow != null) Object.Destroy(arrow.arrow);
        arrow = new Arrow(Color.black);
        if (arrow.arrow != null) arrow.arrow.SetActive(false);
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        ninjaCooldown = roleOption.AddChild("Ninja Mark Cooldown", new FloatOptionSelection(30f, 10f, 120f, 5f));
        ninjaKnowsTargetLocation = roleOption.AddChild("Ninja Knows Location Of Target", new BoolOptionSelection());
        ninjaTraceTime = roleOption.AddChild("Trace Duration", new FloatOptionSelection(5f, 1f, 20f, 0.5f));
        ninjaTraceColorTime = roleOption.AddChild("Time Till Trace Color Has Faded",  new FloatOptionSelection(5f, 0f, 20f, 0.5f));
        ninjaInvisibleDuration = roleOption.AddChild("Time The Ninja Is Invisible", new IntOptionSelection(3, 0, 20, 1));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Ninja mark and assassinate button 
        ninjaButton = new CustomButton(
            () =>
            {
                /*MessageWriter writer;
                if (ninjaMarked != null)
                {
                    // Murder attempt with teleport
                    var attempt = Helpers.checkMuderAttempt(ninja, ninjaMarked);
                    switch (attempt)
                    {
                        case MurderAttemptResult.BodyGuardKill:
                            Helpers.checkMuderAttemptAndKill(ninja, ninjaMarked);
                            return;
                        case MurderAttemptResult.PerformKill:
                        case MurderAttemptResult.ReverseKill:
                        {
                            // Create first trace before killing
                            var pos = LocalPlayer.transform.position;
                            var buff = new byte[sizeof(float) * 2];
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                            writer = AmongUsClient.Instance.StartRpc(LocalPlayer.Control.NetId,
                                (byte)CustomRPC.PlaceNinjaTrace);
                            writer.WriteBytesAndSize(buff);
                            writer.EndMessage();
                            RPCProcedure.placeNinjaTrace(buff);

                            var invisibleWriter = AmongUsClient.Instance.StartRpcImmediately(
                                LocalPlayer.Control.NetId, (byte)CustomRPC.SetInvisible,
                                SendOption.Reliable);
                            invisibleWriter.Write(ninja.PlayerId);
                            invisibleWriter.Write(byte.MinValue);
                            AmongUsClient.Instance.FinishRpcImmediately(invisibleWriter);
                            RPCProcedure.setInvisible(ninja.PlayerId, byte.MinValue);
                            if (!Helpers.checkAndDoVetKill(ninjaMarked))
                            {
                                // Perform Kill
                                var writer2 = AmongUsClient.Instance.StartRpcImmediately(
                                    LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                                    SendOption.Reliable);
                                writer2.Write(LocalPlayer.PlayerId);
                                writer2.Write(ninjaMarked.PlayerId);
                                writer2.Write(byte.MaxValue);
                                AmongUsClient.Instance.FinishRpcImmediately(writer2);
                                if (SubmergedCompatibility.IsSubmerged)
                                    SubmergedCompatibility.ChangeFloor(ninjaMarked.transform.localPosition.y > -7);
                                RPCProcedure.uncheckedMurderPlayer(LocalPlayer.PlayerId,
                                    ninjaMarked.PlayerId, byte.MaxValue);
                            }

                            // Create Second trace after killing
                            pos = ninjaMarked.transform.position;
                            buff = new byte[sizeof(float) * 2];
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                            var writer3 = AmongUsClient.Instance.StartRpc(LocalPlayer.Control.NetId,
                                (byte)CustomRPC.PlaceNinjaTrace);
                            writer3.WriteBytesAndSize(buff);
                            writer3.EndMessage();
                            RPCProcedure.placeNinjaTrace(buff);
                            break;
                        }
                    }

                    if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill)
                    {
                        ninjaButton.Timer = ninjaButton.MaxTimer;
                        ninja.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                    }
                    else if (attempt == MurderAttemptResult.SuppressKill)
                    {
                        ninjaButton.Timer = 0f;
                    }

                    ninjaMarked = null;
                    return;
                }

                if (currentTarget == null) return;
                if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(Get<Witch>().currentTarget);
                ninjaMarked = currentTarget;
                ninjaButton.Timer = 5f;
                SoundEffectsManager.play("warlockCurse");

                // Ghost Info
                writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(LocalPlayer.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.NinjaMarked);
                writer.Write(ninjaMarked.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);*/
            },
            () => ninja != null && ninja == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                // CouldUse
                /*ButtonHelper.showTargetNameOnButton(currentTarget, ninjaButton, "NINJA");
                ninjaButton.Sprite = ninjaMarked != null
                    ? KillButtonSprite
                    : MarkButtonSprite;
                return (currentTarget != null || (ninjaMarked != null &&
                                                  !TransportationToolPatches.isUsingTransportation(
                                                      ninjaMarked))) && LocalPlayer
                    .PlayerControl.CanMove;*/
                return true;
            },
            () =>
            {
                // on meeting ends
                ninjaButton.Timer = ninjaButton.MaxTimer;
                ninjaMarked = null;
            },
            MarkButtonSprite,
            DefButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        ninjaButton.MaxTimer = cooldown;
    }
}