using TheOtherUs.Objects;
using TMPro;
using Object = UnityEngine.Object;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class BountyHunter : RoleBase
{
    public Arrow arrow;
    public float arrowUpdateIntervall = 10f;

    public float arrowUpdateTimer;
    public PlayerControl bounty;
    public float bountyDuration = 30f;
    public PlayerControl bountyHunter;
    public CustomOption bountyHunterArrowUpdateIntervall;
    public CustomOption bountyHunterBountyDuration;
    public CustomOption bountyHunterPunishmentTime;
    public CustomOption bountyHunterReducedCooldown;
    public CustomOption bountyHunterShowArrow;
    
    public float bountyKillCooldown;
    public float bountyUpdateTimer;
    public TextMeshPro cooldownText;
    public float punishmentTime = 15f;
    public bool showArrow = true;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Color = Palette.ImpostorRed,
        Name = nameof(bountyHunter),
        RoleClassType = typeof(BountyHunter),
        RoleId = RoleId.BountyHunter,
        RoleTeam = RoleTeam.Impostor,
        RoleType = CustomRoleType.Main,
        GetRole = Get<BountyHunter>,
        DescriptionText = "Hunt your bounty down",
        IntroInfo = "Hunt your bounty down",
        CreateRoleController = n => new BountyHunterController(n)
    };
    
    public class BountyHunterController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<BountyHunter>();
    }
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        bountyHunterBountyDuration = roleOption.AddChild("Duration After Which Bounty Changes", new IntOptionSelection(60, 10, 180, 10));
        bountyHunterReducedCooldown = roleOption.AddChild( "Cooldown After Killing Bounty", new FloatOptionSelection(2.5f, 0f, 30f, 2.5f));
        bountyHunterPunishmentTime = roleOption.AddChild("Additional Cooldown After Killing Others", new FloatOptionSelection(20f, 0f, 60f,
            2.5f));
        bountyHunterShowArrow = roleOption.AddChild("Show Arrow Pointing Towards The Bounty", new BoolOptionSelection(true));
        bountyHunterArrowUpdateIntervall = bountyHunterShowArrow.AddChild("Arrow Update Intervall", new FloatOptionSelection(15f, 2.5f, 60f, 2.5f));
    }

    public override void ClearAndReload()
    {
        arrow = new Arrow(RoleInfo.Color);
        bountyHunter = null;
        bounty = null;
        arrowUpdateTimer = 0f;
        bountyUpdateTimer = 0f;
        if (arrow != null && arrow.arrow != null) Object.Destroy(arrow.arrow);
        arrow = null;
        if (cooldownText != null && cooldownText.gameObject != null) Object.Destroy(cooldownText.gameObject);
        cooldownText = null;
        /*foreach (var p in MapOptions.playerIcons.Values.Where(p => p != null && p.gameObject != null))
            p.gameObject.SetActive(false);*/


        bountyDuration = bountyHunterBountyDuration;
        bountyKillCooldown = bountyHunterReducedCooldown;
        punishmentTime = bountyHunterPunishmentTime;
        showArrow = bountyHunterShowArrow;
        arrowUpdateIntervall = bountyHunterArrowUpdateIntervall;
    }
}