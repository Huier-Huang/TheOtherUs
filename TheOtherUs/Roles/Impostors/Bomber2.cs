using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Bomber2 : RoleBase
{
    public Color alertColor = Palette.ImpostorRed;

    public bool bombActive;
    public float bombDelay = 10f;

    public PlayerControl bomber2;
    //public CustomOption bomber2HotPotatoMode;

    private CustomButton bomber2BombButton;
    public CustomOption bomber2BombCooldown;
    public CustomOption bomber2Delay;
    private CustomButton bomber2KillButton;

    public CustomOption bomber2SpawnRate;
    public CustomOption bomber2Timer;
    public float bombTimer = 10f;


    private readonly ResourceSprite buttonSprite = new("Bomber2.png");

    public float cooldown = 30f;

    // public static bool hotPotatoMode = false;
    public PlayerControl currentBombTarget = null;
    public PlayerControl currentTarget = null;
    public bool hasAlerted = false;
    public PlayerControl hasBomb;
    public int timeLeft = 0;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Bomber2),
        RoleClassType = typeof(Bomber2),
        RoleId = RoleId.Bomber2,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        GetRole = Get<Bomber2>,
        Color = Palette.ImpostorRed,
        IntroInfo = "Give bombs to players",
        DescriptionText = "Bomb Everyone",
        CreateRoleController = player => new Bomber2Controller(player)
    };
    
    public class Bomber2Controller(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Bomber2>();
    }

    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        bomber2 = null;
        bombActive = false;
        cooldown = bomber2BombCooldown;
        bombDelay = bomber2Delay;
        bombTimer = bomber2Timer;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        bomber2BombCooldown = roleOption.AddChild("Bomber2 Cooldown",  new FloatOptionSelection(30f, 25f, 60f, 2.5f));
        bomber2Delay = roleOption.AddChild("Bomb Delay", new FloatOptionSelection(10f, 1f, 20f, 0.5f));
        bomber2Timer = roleOption.AddChild("Bomb Timer", new FloatOptionSelection(10f, 5f, 30f, 5f));

    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        bomber2BombButton = new CustomButton(
            () =>
            {
                /* On Use */
                /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);*/
                var bombWriter = AmongUsClient.Instance.StartRpcImmediately(
                    LocalPlayer.Control.NetId, (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                bombWriter.Write(currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(bombWriter);
                /*RPCProcedure.giveBomb(currentTarget.PlayerId);*/
                bomber2.killTimer = bombTimer + bombDelay;
                bomber2BombButton.Timer = bomber2BombButton.MaxTimer;
            },
            () => bomber2 != null && bomber2 == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () => currentTarget && LocalPlayer.Control.CanMove,
            () =>
            {
                /* On Meeting End */
                bomber2BombButton.Timer = bomber2BombButton.MaxTimer;
                bomber2BombButton.isEffectActive = false;
                bomber2BombButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                hasBomb = null;
            },
            buttonSprite,
            DefButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.V
        );

        bomber2KillButton = new CustomButton(
            () =>
            {
                /* On Use */
                if (currentBombTarget == bomber2)
                {
                    var killWriter = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.UncheckedMurderPlayer,
                        SendOption.Reliable);
                    killWriter.Write(bomber2.Data.PlayerId);
                    killWriter.Write(hasBomb.Data.PlayerId);
                    killWriter.Write(0);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    /*RPCProcedure.uncheckedMurderPlayer(bomber2.Data.PlayerId, hasBomb.Data.PlayerId, 0);*/

                    var bombWriter1 = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                    bombWriter1.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(bombWriter1);
                    /*RPCProcedure.giveBomb(byte.MaxValue);*/
                    return;
                }

                /*if (Helpers.checkAndDoVetKill(currentBombTarget)) return;
                if (Helpers.checkMuderAttemptAndKill(hasBomb, currentBombTarget) ==
                    MurderAttemptResult.SuppressKill) return;*/
                var bombWriter = AmongUsClient.Instance.StartRpcImmediately(
                    LocalPlayer.Control.NetId, (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                bombWriter.Write(byte.MaxValue);
                AmongUsClient.Instance.FinishRpcImmediately(bombWriter);
                /*RPCProcedure.giveBomb(byte.MaxValue);*/
            },
            () => bomber2 != null && hasBomb == LocalPlayer.Control &&
                  bombActive && !LocalPlayer.IsDead,
            () => currentBombTarget && LocalPlayer.Control.CanMove,
            () =>
            {
                /* On Meeting End */
            },
            buttonSprite,
            //          0, -0.06f, 0
            new Vector3(-4.5f, 1.5f, 0),
            _hudManager,
            KeyCode.B
        );
    }

    public override void ResetCustomButton()
    {
        bomber2KillButton.MaxTimer = 0f;
        bomber2KillButton.Timer = 0f;
        bomber2BombButton.MaxTimer = cooldown;
        bomber2BombButton.EffectDuration = bombDelay + bombTimer;
    }
}