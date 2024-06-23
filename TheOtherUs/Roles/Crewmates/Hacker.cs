using System;
using TheOtherUs.Options;
using UnityEngine;

namespace TheOtherUs.Roles.Crewmates;

[RegisterRole]
public class Hacker : RoleBase
{
    public ResourceSprite adminSprite = new (onGetSprite: s =>
    {
        var fastUseSettings = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings;
        var imageName = (MapData.Maps)MapData.MapId switch
        {
            MapData.Maps.Skeld => ImageNames.AdminMapButton,
            MapData.Maps.Mira => ImageNames.MIRAAdminButton,
            MapData.Maps.Polus => ImageNames.PolusAdminButton,
            MapData.Maps.Fungle => ImageNames.AdminMapButton,
            MapData.Maps.Airship => ImageNames.AirshipAdminButton,
            _ => ImageNames.PolusAdminButton
        };
        var button = fastUseSettings[imageName];
        s.ReturnSprite = button.Image;
    });
    public ResourceSprite logSprite = new (onGetSprite: s =>
    {
        s.ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings[ImageNames.DoorLogsButton]
            .Image;
    });
    public ResourceSprite vitalsSprite = new(onGetSprite: s =>
    {
        s.ReturnSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings[ImageNames.VitalsButton]
            .Image;
    });
    public ResourceSprite buttonSprite = new ("HackerButton.png");
    
    public bool cantMove = true;
    public int chargesAdminTable = 1;
    public int chargesVitals = 1;

    public float cooldown = 30f;
    public Minigame doorLog;
    public float duration = 10f;
    public PlayerControl hacker;
    public float hackerTimer;
    public bool onlyColorType;
    public int rechargedTasks = 2;
    public int rechargeTasksNumber = 2;
    public float toolsNumber = 5f;
    public Minigame vitals;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = new Color32(117, 250, 76, byte.MaxValue),
        GetRole = Get<Hacker>,
        CreateRoleController = n => new HackerController(n),
        DescriptionText = "Hack to find the Impostors",
        IntroInfo = "Hack systems to find the <color=#FF1919FF>Impostors</color>",
        Name = nameof(Hacker),
        RoleClassType = typeof(Hacker),
        RoleId = RoleId.Hacker,
        RoleTeam = RoleTeam.Crewmate,
        RoleType = CustomRoleType.Main
    };
    
    public class HackerController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Hacker>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        hacker = null;
        vitals = null;
        doorLog = null;
        hackerTimer = 0f;
        adminSprite = null;
        cooldown = CustomOptionHolder.hackerCooldown;
        duration = CustomOptionHolder.hackerHackeringDuration;
        onlyColorType = CustomOptionHolder.hackerOnlyColorType;
        toolsNumber = CustomOptionHolder.hackerToolsNumber;
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber);
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber);
        chargesVitals = Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber) / 2;
        chargesAdminTable = Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber) / 2;
        cantMove = CustomOptionHolder.hackerNoMove;
    }
}