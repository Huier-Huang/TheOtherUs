using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherUs.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Cultist : RoleBase
{
    public readonly ResourceSprite buttonSprite = new("SidekickButton.png");
    public bool chatTarget = true;

    public bool chatTarget2 = true;

    //public PlayerControl currentFollower;
    public Color color = Palette.ImpostorRed;
    public PlayerControl cultist;

    public CustomOption cultistSpawnRate;
    private CustomButton cultistTurnButton;
    public PlayerControl currentTarget;
    public bool isCultistGame = false;
    public List<Arrow> localArrows = [];

    public bool needsFollower = true;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Cultist),
        RoleClassType = typeof(Cultist),
        Color = Palette.ImpostorRed,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        RoleId = RoleId.Cultist,
        GetRole = Get<Cultist>,
        DescriptionText = "Recruit for your cause",
        IntroInfo = "Recruit for your cause",
        CreateRoleController = player => new CultistController(player)
    };
    public class CultistController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Cultist>();
    }
    public override CustomRoleOption roleOption { get; set; }


    public override void ClearAndReload()
    {
        if (localArrows != null)
            foreach (var arrow in localArrows.Where(arrow => arrow?.arrow != null))
                Object.Destroy(arrow.arrow);
        localArrows = [];
        cultist = null;
        currentTarget = null;
        needsFollower = true;
        chatTarget = true;
        chatTarget2 = true;
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        cultistTurnButton = new CustomButton(
            () =>
            {
                /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);*/
                var writer = AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.Control.NetId,
                    (byte)CustomRPC.CultistCreateImposter, SendOption.Reliable);
                writer.Write(currentTarget.PlayerId);
                /*AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.cultistCreateImposter(currentTarget.PlayerId);*/
                SoundEffectsManager.play("jackalSidekick");
            },
            () =>
            {
                return needsFollower && cultist != null &&
                       cultist == LocalPlayer.Control &&
                       !LocalPlayer.IsDead;
            },
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, cultistTurnButton,
                    "Convert"); // Show now text since the button already says sidekick
                return needsFollower && currentTarget != null &&
                       LocalPlayer.Control.CanMove;
            },
            () =>
            {
                /*HudManagerStartPatch.jackalSidekickButton.Timer = HudManagerStartPatch.jackalSidekickButton.MaxTimer;*/
            },
            buttonSprite,
            DefButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.F
        );
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }
}