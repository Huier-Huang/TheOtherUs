using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Miner : RoleBase
{
    public readonly List<Vent> Vents = [];
    public KillButton _mineButton;

    private readonly ResourceSprite buttonSprite = new("Mine.png");

    public float cooldown = 30f;
    public DateTime LastMined;
    public PlayerControl miner;
    public CustomOption minerCooldown;

    private CustomButton minerMineButton;

    public bool CanPlace { get; set; }
    public Vector2 VentSize { get; set; }

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Miner),
        RoleClassType = typeof(Miner),
        Color = Palette.ImpostorRed,
        RoleId = RoleId.Miner,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Impostor,
        GetRole = Get<Miner>,
        DescriptionText = "Create Vents",
        IntroInfo = "Make new Vents",
        CreateRoleController = player => new MinerController(player)
    };
    
    public class MinerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Miner>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        miner = null;
        cooldown = minerCooldown;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        minerCooldown = roleOption.AddChild("Mine Cooldown", new FloatOptionSelection(25f, 10f, 60f, 2.5f));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        minerMineButton = new CustomButton(
            () =>
            {
                /* On Use */
                minerMineButton.Timer = minerMineButton.MaxTimer;

                var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.Mine, SendOption.Reliable);
                var pos = LocalPlayer.Control.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var id = MapData.GetAvailableVentId();
                writer.Write(id);
                writer.Write(LocalPlayer.PlayerId);


                writer.WriteBytesAndSize(buff);


                writer.Write(0.01f);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                /*RPCProcedure.Mine(id, miner, buff, 0.01f);*/
            },
            () => miner != null && miner == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                /* Can Use */
                var hits = Physics2D.OverlapBoxAll(LocalPlayer.Control.transform.position,
                    VentSize, 0);
                hits = hits.ToArray().Where(c =>
                    {
                        GameObject gameObject;
                        return (c.name.Contains("Vent") || !c.isTrigger) && (gameObject = c.gameObject).layer != 8 &&
                               gameObject.layer != 5;
                    })
                    .ToArray();
                return hits.Count == 0 && LocalPlayer.Control.CanMove;
            },
            () =>
            {
                /* On Meeting End */
                minerMineButton.Timer = minerMineButton.MaxTimer;
            },
            buttonSprite,
            DefButtonPositions.upperRowLeft, //brb
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