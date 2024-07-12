using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Bloody : RoleBase
{
    public Dictionary<byte, float> active = new();
    public List<PlayerControl> bloody = [];
    public Dictionary<byte, byte> bloodyKillerMap = new();

    public float duration = 5f;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Bloody),
        RoleClassType = typeof(Bloody),
        Color= Color.yellow,
        RoleId = RoleId.Bloody,
        RoleType = CustomRoleType.Modifier,
        RoleTeam = RoleTeam.Special,
        GetRole = Get<Bloody>,
        IntroInfo = "Your killer leaves a bloody trail",
        DescriptionText = "Your killer leaves a bloody trail",
        CreateRoleController = player => new BloodyController(player)
    };
    public class BloodyController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Bloody>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
    }

    public override void ClearAndReload()
    {
        bloody = [];
        active = new Dictionary<byte, float>();
        bloodyKillerMap = new Dictionary<byte, byte>();
        duration = CustomOptionHolder.modifierBloodyDuration;
    }
}