using System.Collections.Generic;
using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Eraser : RoleBase
{
    public List<byte> alreadyErased = [];


    private readonly ResourceSprite buttonSprite = new("EraserButton.png");
    public bool canEraseAnyone;
    public float cooldown = 30f;
    public PlayerControl currentTarget;
    public PlayerControl eraser;
    private CustomButton eraserButton;
    public CustomOption eraserCanEraseAnyone;
    public CustomOption eraserCooldown;


    public List<PlayerControl> futureErased = [];

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Eraser),
        RoleClassType = typeof(Eraser),
        Color = Palette.ImpostorRed,
        GetRole = Get<Eraser>,
        RoleType = CustomRoleType.Main,
        RoleId = RoleId.Eraser,
        RoleTeam = RoleTeam.Impostor,
        IntroInfo = "Kill the Crewmates and erase their roles",
        DescriptionText = "Erase the roles of your enemies",
        CreateRoleController = player => new EraserController(player)
    };
    
    public class EraserController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Eraser>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        eraser = null;
        futureErased = [];
        currentTarget = null;
        cooldown = eraserCooldown;
        canEraseAnyone = eraserCanEraseAnyone;
        alreadyErased = [];
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        eraserCooldown = roleOption.AddChild("Eraser Cooldown", new FloatOptionSelection(30f, 10f, 120f, 5f));
        eraserCanEraseAnyone = roleOption.AddChild("Eraser Can Erase Anyone", new BoolOptionSelection(false));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Eraser erase button
        eraserButton = new CustomButton(
            () =>
            {
                /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);*/
                eraserButton.MaxTimer += 10;
                eraserButton.Timer = eraserButton.MaxTimer;

                var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.SetFutureErased, SendOption.Reliable);
                writer.Write(currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                /*RPCProcedure.setFutureErased(currentTarget.PlayerId);*/
                SoundEffectsManager.play("eraserErase");
            },
            () => eraser != null && eraser == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, eraserButton, "ERASE");
                return LocalPlayer.Control.CanMove && currentTarget != null;
            },
            () => { eraserButton.Timer = eraserButton.MaxTimer; },
            buttonSprite,
            DefButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        eraserButton.MaxTimer = cooldown;
    }
}