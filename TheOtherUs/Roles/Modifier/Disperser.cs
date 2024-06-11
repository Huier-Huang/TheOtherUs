using System;
using TheOtherUs.Roles.Impostors;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Disperser : RoleBase
{
    private ResourceSprite buttonSprite = new("Disperse.png");
    public Color color = new Color32(48, 21, 89, byte.MaxValue);

    public float cooldown = 30f;
    public PlayerControl disperser;
    public bool dispersesToVent;
    public int remainingDisperses = 1;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        disperser = null;
        cooldown = CustomOptionHolder.modifierDisperserCooldown;
        remainingDisperses = CustomOptionHolder.modifierDisperserNumberOfUses.OptionSelection.Selection;
        dispersesToVent = CustomOptionHolder.modifierDisperserDispersesToVent;
    }

    public void disperse()
    {
        Get<AntiTeleport>().setPosition();
        Helpers.showFlash(Cleaner.color);
        if (Get<AntiTeleport>().antiTeleport.FindAll(x => x.PlayerId == CachedPlayer.LocalPlayer.Control.PlayerId)
                .Count != 0 || CachedPlayer.LocalPlayer.Data.IsDead) return;

        if (MapBehaviour.Instance)
            MapBehaviour.Instance.Close();

        if (Minigame.Instance)
            Minigame.Instance.ForceClose();

        MapData.AllPlayerExitVent();
        if (CustomOptionHolder.modifierDisperserDispersesToVent)
            MapData.RandomSpawnAllPlayersToVent();
        else
            MapData.RandomSpawnAllPlayersToMap();
        remainingDisperses--;
    }
}