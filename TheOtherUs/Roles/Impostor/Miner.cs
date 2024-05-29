using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Impostor;

[RegisterRole]
public class Miner : RoleBase
{
    public readonly List<Vent> Vents = [];
    public KillButton _mineButton;

    private readonly ResourceSprite buttonSprite = new("Mine.png");
    public Color color = Palette.ImpostorRed;

    public float cooldown = 30f;
    public DateTime LastMined;
    public PlayerControl miner;
    public CustomOption minerCooldown;

    private CustomButton minerMineButton;

    public CustomOption minerSpawnRate;

    public bool CanPlace { get; set; }
    public Vector2 VentSize { get; set; }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        miner = null;
        cooldown = minerCooldown.getFloat();
    }

    public override void OptionCreate()
    {
        minerSpawnRate = new CustomOption(1120, "Miner".ColorString(color), CustomOptionHolder.rates, null, true);
        minerCooldown = new CustomOption(1121, "Mine Cooldown", 25f, 10f, 60f, 2.5f, minerSpawnRate);
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        minerMineButton = new CustomButton(
            () =>
            {
                /* On Use */
                minerMineButton.Timer = minerMineButton.MaxTimer;

                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.Mine, SendOption.Reliable);
                var pos = CachedPlayer.LocalPlayer.Control.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var id = Helpers.getAvailableId();
                writer.Write(id);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);


                writer.WriteBytesAndSize(buff);


                writer.Write(0.01f);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.Mine(id, miner, buff, 0.01f);
            },
            () => miner != null && miner == CachedPlayer.LocalPlayer.Control &&
                  !CachedPlayer.LocalPlayer.Data.IsDead,
            () =>
            {
                /* Can Use */
                var hits = Physics2D.OverlapBoxAll(CachedPlayer.LocalPlayer.Control.transform.position,
                    VentSize, 0);
                hits = hits.ToArray().Where(c =>
                    {
                        GameObject gameObject;
                        return (c.name.Contains("Vent") || !c.isTrigger) && (gameObject = c.gameObject).layer != 8 &&
                               gameObject.layer != 5;
                    })
                    .ToArray();
                return hits.Count == 0 && CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () =>
            {
                /* On Meeting End */
                minerMineButton.Timer = minerMineButton.MaxTimer;
            },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.V
        );
    }

    public override void ResetCustomButton()
    {
        minerMineButton.MaxTimer = cooldown;
        //minerMineButton.EffectDuration = Jackal.duration; // Jackal?
    }
}