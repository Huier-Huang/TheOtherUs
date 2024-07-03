using Hazel;
using TheOtherUs.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Bomber : RoleBase
{
    public Bomb bomb;
    public float bombActiveAfter = 3f;
    public float bombCooldown = 15f;
    public PlayerControl bomber;
    public CustomButton bomberButton;

    private readonly ResourceSprite buttonSprite = new("Bomb_Button_Plant.png");

    public CustomButton defuseButton;
    public float defuseDuration = 3f;
    public float destructionRange = 2f;
    public float destructionTime = 20f;
    public float hearRange = 30f;
    public bool isActive;
    public bool isPlanted;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Bomber),
        RoleClassType = typeof(Bomber),
        Color = Palette.ImpostorRed,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        RoleId = RoleId.Bomber,
        GetRole = Get<Bomber>,
        DescriptionText = "Bomb all Crewmates",
        IntroInfo = "Bomb all Crewmates",
        CreateRoleController = n => new BomberController(n)
    };
    public class BomberController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Bomber>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public void clearBomb(bool flag = true)
    {
        if (bomb != null)
        {
            Object.Destroy(bomb.bomb);
            Object.Destroy(bomb.background);
            bomb = null;
        }

        isPlanted = false;
        isActive = false;
        if (flag) SoundEffectsManager.stop("bombFuseBurning");
    }

    public override void ClearAndReload()
    {
        clearBomb(false);
        bomber = null;
        bomb = null;
        isPlanted = false;
        isActive = false;
        destructionTime = CustomOptionHolder.bomberBombDestructionTime;
        destructionRange = CustomOptionHolder.bomberBombDestructionRange/ 10f;
        hearRange = CustomOptionHolder.bomberBombHearRange / 10f;
        defuseDuration = CustomOptionHolder.bomberDefuseDuration;
        bombCooldown = CustomOptionHolder.bomberBombCooldown;
        bombActiveAfter = CustomOptionHolder.bomberBombActiveAfter;
        Bomb.clearBackgroundSprite();
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Bomber button
        bomberButton = new CustomButton(
            () =>
            {
                /*if (Helpers.checkMuderAttempt(bomber, bomber) != MurderAttemptResult.BlankKill)
                {
                    var pos = LocalPlayer.transform.position;
                    var buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    var writer = AmongUsClient.Instance.StartRpc(LocalPlayer.Control.NetId,
                        (byte)CustomRPC.PlaceBomb);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.placeBomb(buff);

                    SoundEffectsManager.play("trapperTrap");
                }*/

                bomberButton.Timer = bomberButton.MaxTimer;
                isPlanted = true;
            },
            () =>
            {
                return bomber != null && bomber == LocalPlayer.Control &&
                       !LocalPlayer.IsDead;
            },
            () => LocalPlayer.Control.CanMove && !isPlanted,
            () => { bomberButton.Timer = bomberButton.MaxTimer; },
            buttonSprite,
            DefButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F,
            true,
            destructionTime,
            () =>
            {
                bomberButton.Timer = bomberButton.MaxTimer;
                bomberButton.isEffectActive = false;
                bomberButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            }
        );
        defuseButton = new CustomButton(
            () => { defuseButton.HasEffect = true; },
            () =>
            {
                defuseButton.PositionOffset = true /*Get<Shifter>().shifterShiftButton.HasButton()*/
                    ? new Vector3(0f, 2f, 0f)
                    : new Vector3(0f, 1f, 0f);
                return bomb != null && Bomb.canDefuse && !LocalPlayer.IsDead;
            },
            () =>
            {
                if (defuseButton.isEffectActive && !Bomb.canDefuse)
                {
                    defuseButton.Timer = 0f;
                    defuseButton.isEffectActive = false;
                }

                return LocalPlayer.Control.CanMove;
            },
            () =>
            {
                defuseButton.Timer = 0f;
                defuseButton.isEffectActive = false;
            },
            Bomb.getDefuseSprite(),
            new Vector3(0f, 1f, 0),
            _hudManager,
            null,
            true,
            defuseDuration,
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.DefuseBomb, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                /*RPCProcedure.defuseBomb();*/

                defuseButton.Timer = 0f;
                Bomb.canDefuse = false;
            },
            true
        );
    }

    public override void ResetCustomButton()
    {
        bomberButton.MaxTimer = bombCooldown;
        defuseButton.EffectDuration = defuseDuration;
        bomberButton.EffectDuration = destructionTime + bombActiveAfter;
    }
}