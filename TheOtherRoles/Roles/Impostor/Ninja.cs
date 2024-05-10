using System;
using Hazel;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Ninja : RoleBase
{
    public Arrow arrow = new(Color.black);
    public Color color = Palette.ImpostorRed;

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

    public CustomOption ninjaSpawnRate;
    public CustomOption ninjaTraceColorTime;
    public CustomOption ninjaTraceTime;
    public float traceTime = 1f;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        ninja = null;
        currentTarget = ninjaMarked = null;
        cooldown = ninjaCooldown.getFloat();
        knowsTargetLocation = ninjaKnowsTargetLocation.getBool();
        traceTime = ninjaTraceTime.getFloat();
        invisibleDuration = ninjaInvisibleDuration.getFloat();
        invisibleTimer = 0f;
        isInvisble = false;
        if (arrow?.arrow != null) Object.Destroy(arrow.arrow);
        arrow = new Arrow(Color.black);
        if (arrow.arrow != null) arrow.arrow.SetActive(false);
    }

    public override void OptionCreate()
    {
        ninjaSpawnRate = new CustomOption(380, "Ninja".ColorString(color), CustomOptionHolder.rates, null, true);
        ninjaCooldown = new CustomOption(381, "Ninja Mark Cooldown", 30f, 10f, 120f, 5f, ninjaSpawnRate);
        ninjaKnowsTargetLocation = new CustomOption(382, "Ninja Knows Location Of Target", true, ninjaSpawnRate);
        ninjaTraceTime = new CustomOption(383, "Trace Duration", 5f, 1f, 20f, 0.5f, ninjaSpawnRate);
        ninjaTraceColorTime =
            new CustomOption(384, "Time Till Trace Color Has Faded", 2f, 0f, 20f, 0.5f, ninjaSpawnRate);
        ninjaInvisibleDuration = new CustomOption(385, "Time The Ninja Is Invisible", 3f, 0f, 20f, 1f, ninjaSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Ninja mark and assassinate button 
        ninjaButton = new CustomButton(
            () =>
            {
                MessageWriter writer;
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
                            var pos = CachedPlayer.LocalPlayer.transform.position;
                            var buff = new byte[sizeof(float) * 2];
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                            writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.Control.NetId,
                                (byte)CustomRPC.PlaceNinjaTrace);
                            writer.WriteBytesAndSize(buff);
                            writer.EndMessage();
                            RPCProcedure.placeNinjaTrace(buff);

                            var invisibleWriter = AmongUsClient.Instance.StartRpcImmediately(
                                CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.SetInvisible,
                                SendOption.Reliable);
                            invisibleWriter.Write(ninja.PlayerId);
                            invisibleWriter.Write(byte.MinValue);
                            AmongUsClient.Instance.FinishRpcImmediately(invisibleWriter);
                            RPCProcedure.setInvisible(ninja.PlayerId, byte.MinValue);
                            if (!Helpers.checkAndDoVetKill(ninjaMarked))
                            {
                                // Perform Kill
                                var writer2 = AmongUsClient.Instance.StartRpcImmediately(
                                    CachedPlayer.LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                                    SendOption.Reliable);
                                writer2.Write(CachedPlayer.LocalPlayer.PlayerId);
                                writer2.Write(ninjaMarked.PlayerId);
                                writer2.Write(byte.MaxValue);
                                AmongUsClient.Instance.FinishRpcImmediately(writer2);
                                if (SubmergedCompatibility.IsSubmerged)
                                    SubmergedCompatibility.ChangeFloor(ninjaMarked.transform.localPosition.y > -7);
                                RPCProcedure.uncheckedMurderPlayer(CachedPlayer.LocalPlayer.PlayerId,
                                    ninjaMarked.PlayerId, byte.MaxValue);
                            }

                            // Create Second trace after killing
                            pos = ninjaMarked.transform.position;
                            buff = new byte[sizeof(float) * 2];
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                            var writer3 = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.Control.NetId,
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
                writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.NinjaMarked);
                writer.Write(ninjaMarked.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            },
            () => ninja != null && ninja == CachedPlayer.LocalPlayer.Control &&
                  !CachedPlayer.LocalPlayer.Data.IsDead,
            () =>
            {
                // CouldUse
                ButtonHelper.showTargetNameOnButton(currentTarget, ninjaButton, "NINJA");
                ninjaButton.Sprite = ninjaMarked != null
                    ? KillButtonSprite
                    : MarkButtonSprite;
                return (currentTarget != null || (ninjaMarked != null &&
                                                  !TransportationToolPatches.isUsingTransportation(
                                                      ninjaMarked))) && CachedPlayer.LocalPlayer
                    .PlayerControl.CanMove;
            },
            () =>
            {
                // on meeting ends
                ninjaButton.Timer = ninjaButton.MaxTimer;
                ninjaMarked = null;
            },
            MarkButtonSprite,
            CustomButton.ButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        ninjaButton.MaxTimer = cooldown;
    }
}