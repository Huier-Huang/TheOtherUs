using System;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmate;

[RegisterRole]
public class Portalmaker : RoleBase
{
    public static readonly RoleInfo roleInfo = new()
    {
        Name = nameof(Portalmaker),
        Color = new Color32(69, 69, 169, byte.MaxValue),
        Description = "You can create portals",
        IntroInfo = "You can create portals",
        GetRole = Get<Portalmaker>,
        RoleClassType = typeof(Portalmaker),
        RoleId = RoleId.Portalmaker,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };

    public bool canPortalFromAnywhere;
    public Color color = new Color32(69, 69, 169, byte.MaxValue);

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

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override Type RoleType { get; protected set; }

    public Sprite getUsePortalSpecialButtonSprite(bool first)
    {
        return first ? usePortalSpecialButtonSprite1 : usePortalSpecialButtonSprite2;
    }

    public override void ClearAndReload()
    {
        portalmaker = null;
        cooldown = CustomOptionHolder.portalmakerCooldown.getFloat();
        usePortalCooldown = CustomOptionHolder.portalmakerUsePortalCooldown.getFloat();
        logOnlyHasColors = CustomOptionHolder.portalmakerLogOnlyColorType.getBool();
        logShowsTime = CustomOptionHolder.portalmakerLogHasTime.getBool();
        canPortalFromAnywhere = CustomOptionHolder.portalmakerCanPortalFromAnywhere.getBool();
    }
}