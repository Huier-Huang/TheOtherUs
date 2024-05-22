using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherUs.Helper;
using TheOtherUs.Modules;
using TheOtherUs.Objects;
using TheOtherUs.Options;
using TheOtherUs.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Impostor;

[RegisterRole]
public class Cultist : RoleBase
{
    public ResourceSprite buttonSprite = new("SidekickButton.png");
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

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


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
                if (Helpers.checkAndDoVetKill(currentTarget)) return;
                Helpers.checkWatchFlash(currentTarget);
                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.Control.NetId,
                    (byte)CustomRPC.CultistCreateImposter, SendOption.Reliable);
                writer.Write(currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.cultistCreateImposter(currentTarget.PlayerId);
                SoundEffectsManager.play("jackalSidekick");
            },
            () =>
            {
                return needsFollower && cultist != null &&
                       cultist == CachedPlayer.LocalPlayer.Control &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, cultistTurnButton,
                    "Convert"); // Show now text since the button already says sidekick
                return needsFollower && currentTarget != null &&
                       CachedPlayer.LocalPlayer.Control.CanMove;
            },
            () =>
            {
                HudManagerStartPatch.jackalSidekickButton.Timer = HudManagerStartPatch.jackalSidekickButton.MaxTimer;
            },
            buttonSprite,
            CustomButton.ButtonPositions.upperRowLeft, //brb
            _hudManager,
            KeyCode.F
        );
    }

    public override void OptionCreate()
    {
        cultistSpawnRate = new CustomOption(3801, "Cultist".ColorString(color), CustomOptionHolder.rates, null, true);
    }
}