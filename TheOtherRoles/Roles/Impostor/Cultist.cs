using System;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Modules.Options;
using TheOtherRoles.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Cultist : RoleBase
{
    public PlayerControl cultist;
    public PlayerControl currentTarget;
    //public PlayerControl currentFollower;
    public Color color = Palette.ImpostorRed;
    public List<Arrow> localArrows = new();
    public bool chatTarget = true;
    public bool chatTarget2 = true;
    public bool isCultistGame = false;

    public bool needsFollower = true;

    public CustomOption cultistSpawnRate;
    private CustomButton cultistTurnButton;

    public ResourceSprite buttonSprite = new("SidekickButton.png");


    public override void ClearAndReload()
    {
        if (localArrows != null)
            foreach (var arrow in localArrows.Where(arrow => arrow?.arrow != null))
                Object.Destroy(arrow.arrow);
        localArrows = new List<Arrow>();
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

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void OptionCreate()
    {
        cultistSpawnRate = new CustomOption(3801, "Cultist".ColorString(color), CustomOptionHolder.rates, null, true);

    }
}