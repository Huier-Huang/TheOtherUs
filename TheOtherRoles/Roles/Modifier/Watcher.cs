using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Watcher : RoleBase
{
    public PlayerControl watcher;
    public Color color = new Color32(48, 21, 89, byte.MaxValue);


    public override void ClearAndReload()
    {
        watcher = null;
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}