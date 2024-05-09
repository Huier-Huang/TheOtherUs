using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheOtherRoles.Roles.Crewmate;

[RegisterRole]
public class Portalmaker : RoleBase
{
    public PlayerControl portalmaker;
    public Color color = new Color32(69, 69, 169, byte.MaxValue);

    public float cooldown;
    public float usePortalCooldown;
    public bool logOnlyHasColors;
    public bool logShowsTime;
    public bool canPortalFromAnywhere;

    private ResourceSprite placePortalButtonSprite = new ("PlacePortalButton.png");
    private ResourceSprite usePortalButtonSprite = new ("UsePortalButton.png");
    private ResourceSprite usePortalSpecialButtonSprite1 = new ("UsePortalSpecialButton1.png");
    private ResourceSprite usePortalSpecialButtonSprite2 = new ("UsePortalSpecialButton2.png");
    private ResourceSprite logSprite = new ()
    {
        ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.DoorLogsButton]
            .Image
    };

    public Sprite getUsePortalSpecialButtonSprite(bool first) => first ? usePortalSpecialButtonSprite1 : usePortalSpecialButtonSprite2;

    public override void ClearAndReload()
    {
        portalmaker = null;
        cooldown = CustomOptionHolder.portalmakerCooldown.getFloat();
        usePortalCooldown = CustomOptionHolder.portalmakerUsePortalCooldown.getFloat();
        logOnlyHasColors = CustomOptionHolder.portalmakerLogOnlyColorType.getBool();
        logShowsTime = CustomOptionHolder.portalmakerLogHasTime.getBool();
        canPortalFromAnywhere = CustomOptionHolder.portalmakerCanPortalFromAnywhere.getBool();
    }

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

    public override RoleInfo RoleInfo { get; protected set; } = roleInfo;
    public override Type RoleType { get; protected set; }
}