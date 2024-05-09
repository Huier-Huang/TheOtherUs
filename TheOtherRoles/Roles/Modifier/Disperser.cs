using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Disperser : RoleBase
{
    public PlayerControl disperser;
    public Color color = new Color32(48, 21, 89, byte.MaxValue);

    public float cooldown = 30f;
    public int remainingDisperses = 1;
    public bool dispersesToVent;
    private ResourceSprite buttonSprite = new ("Disperse.png");

    public override void ClearAndReload()
    {
        disperser = null;
        cooldown = CustomOptionHolder.modifierDisperserCooldown;
        remainingDisperses = CustomOptionHolder.modifierDisperserNumberOfUses.selection.Selection;
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
        MapData.RandomSpawnAllPlayers();
        remainingDisperses--;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}