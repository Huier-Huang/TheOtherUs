using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Disperser : RoleBase
{
    private ResourceSprite buttonSprite = new("Disperse.png");

    public float cooldown = 30f;
    public PlayerControl disperser;
    public bool dispersesToVent;
    public int remainingDisperses = 1;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Disperser),
        RoleClassType = typeof(Disperser),
        Color= new Color32(48, 21, 89, byte.MaxValue),
        RoleId = RoleId.Disperser,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Disperser>,
        IntroInfo = "Separate the Crew",
        DescriptionText = "Separate the Crew",
        CreateRoleController = player => new DisperserController(player)
    };
    public class DisperserController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Disperser>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

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
        /*Helpers.showFlash(Cleaner.color);*/
        if (Get<AntiTeleport>().antiTeleport.FindAll(x => x.PlayerId == LocalPlayer.Control.PlayerId)
                .Count != 0 || LocalPlayer.IsDead) return;

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