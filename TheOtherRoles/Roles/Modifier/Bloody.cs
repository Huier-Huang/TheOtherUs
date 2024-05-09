using System;
using System.Collections.Generic;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Bloody : RoleBase
{
    public List<PlayerControl> bloody = new();
    public Dictionary<byte, float> active = new();
    public Dictionary<byte, byte> bloodyKillerMap = new();

    public float duration = 5f;

    public override void ClearAndReload()
    {
        bloody = new List<PlayerControl>();
        active = new Dictionary<byte, float>();
        bloodyKillerMap = new Dictionary<byte, byte>();
        duration = CustomOptionHolder.modifierBloodyDuration.getFloat();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}