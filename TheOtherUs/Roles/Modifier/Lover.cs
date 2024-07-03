using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Lover : RoleBase
{
    public bool bothDie = true;

    public bool enableChat = true;
    public PlayerControl lover1;
    public PlayerControl lover2;

    // Lovers save if next to be exiled is a lover, because RPC of ending game comes before RPC of exiled
    public bool notAckedExiledIsLover;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Lover),
        RoleClassType = typeof(Lover),
        Color=  new Color32(232, 57, 185, byte.MaxValue),
        RoleId = RoleId.Lover,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Lover>,
        IntroInfo = "You are in love",
        DescriptionText = "You are in love",
        CreateRoleController = player => new LoverController(player)
    };
    public class LoverController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Lover>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public bool existing()
    {
        return lover1 != null && lover2 != null && !lover1.Data.Disconnected && !lover2.Data.Disconnected;
    }

    public bool existingAndAlive()
    {
        return existing() && !lover1.Data.IsDead && !lover2.Data.IsDead &&
               !notAckedExiledIsLover; // ADD NOT ACKED IS LOVER
    }

    public PlayerControl otherLover(PlayerControl oneLover)
    {
        if (!existingAndAlive()) return null;
        if (oneLover == lover1) return lover2;
        return oneLover == lover2 ? lover1 : null;
    }

    public bool existingWithKiller()
    {
        return existing() && (lover1.Is<Jackal>() || lover2.Is<Jackal>()
                                                  || lover1.Is<Sidekick>() || lover2.Is<Sidekick>()
                                                  || lover1.Is<Werewolf>() || lover2.Is<Werewolf>()
                                                  || lover1.Data.Role.IsImpostor || lover2.Data.Role.IsImpostor);
    }

    public bool hasAliveKillingLover(PlayerControl player)
    {
        if (!existingAndAlive() || !existingWithKiller())
            return false;
        return player != null && (player == lover1 || player == lover2);
    }

    public override void ClearAndReload()
    {
        lover1 = null;
        lover2 = null;
        notAckedExiledIsLover = false;
        bothDie = CustomOptionHolder.modifierLoverBothDie;
        enableChat = CustomOptionHolder.modifierLoverEnableChat;
    }

    public PlayerControl getPartner(PlayerControl player)
    {
        if (player == null)
            return null;
        if (lover1 == player)
            return lover2;
        return lover2 == player ? lover1 : null;
    }
}