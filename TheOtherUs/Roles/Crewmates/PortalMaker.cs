using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class PortalMaker : RoleBase
{
    public bool canPortalFromAnywhere;

    public float cooldown;
    public bool logOnlyHasColors;
    public bool logShowsTime;

    private ResourceSprite logSprite = new()
    {
        ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings[ImageNames.DoorLogsButton]
            .Image
    };

    private ResourceSprite placePortalButtonSprite = new("PlacePortalButton.png");
    public PlayerControl portalmaker;
    private ResourceSprite usePortalButtonSprite = new("UsePortalButton.png");
    public float usePortalCooldown;
    private readonly ResourceSprite usePortalSpecialButtonSprite1 = new("UsePortalSpecialButton1.png");
    private readonly ResourceSprite usePortalSpecialButtonSprite2 = new("UsePortalSpecialButton2.png");

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(PortalMaker),
        Color = new Color32(69, 69, 169, byte.MaxValue),
        CreateRoleController = n => new PortalMakerController(n),
        DescriptionText = "You can create portals",
        IntroInfo = "You can create portals",
        GetRole = Get<PortalMaker>,
        RoleClassType = typeof(PortalMaker),
        RoleId = RoleId.Portalmaker,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class PortalMakerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase { get; } = Get<PortalMaker>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public Sprite getUsePortalSpecialButtonSprite(bool first)
    {
        return first ? usePortalSpecialButtonSprite1 : usePortalSpecialButtonSprite2;
    }

    public override void ClearAndReload()
    {
        portalmaker = null;
        cooldown = CustomOptionHolder.portalmakerCooldown;
        usePortalCooldown = CustomOptionHolder.portalmakerUsePortalCooldown;
        logOnlyHasColors = CustomOptionHolder.portalmakerLogOnlyColorType;
        logShowsTime = CustomOptionHolder.portalmakerLogHasTime;
        canPortalFromAnywhere = CustomOptionHolder.portalmakerCanPortalFromAnywhere;
    }
}