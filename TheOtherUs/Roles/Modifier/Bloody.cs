using System;
using System.Collections.Generic;
using TheOtherUs.Utilities;

namespace TheOtherUs.Roles.Modifier;

[RegisterRole]
public class Bloody : RoleBase
{
    public Dictionary<byte, float> active = new();
    public List<PlayerControl> bloody = [];
    public Dictionary<byte, byte> bloodyKillerMap = new();

    public float duration = 5f;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }

    public override void ClearAndReload()
    {
        bloody = [];
        active = new Dictionary<byte, float>();
        bloodyKillerMap = new Dictionary<byte, byte>();
        duration = CustomOptionHolder.modifierBloodyDuration.getFloat();
    }
}