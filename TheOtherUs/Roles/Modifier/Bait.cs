using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Bait : RoleBase
{
    public Dictionary<DeadPlayer, float> active = new();
    public List<PlayerControl> bait = [];
    public float reportDelayMax;

    public float reportDelayMin;
    public bool showKillFlash = true;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Bait),
        RoleClassType = typeof(Bait),
        Color = new Color32(0, 247, 255, byte.MaxValue),
        RoleId = RoleId.Bait,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Bait>,
        IntroInfo = "Bait your enemies",
        DescriptionText = "Bait your enemies",
        CreateRoleController = player => new BaitController(player)
    };
    public class BaitController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Bait>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        bait = [];
        active = new Dictionary<DeadPlayer, float>();
        reportDelayMin = CustomOptionHolder.modifierBaitReportDelayMin;
        reportDelayMax = CustomOptionHolder.modifierBaitReportDelayMax;
        if (reportDelayMin > reportDelayMax) reportDelayMin = reportDelayMax;
        showKillFlash = CustomOptionHolder.modifierBaitShowKillFlash;
    }
}