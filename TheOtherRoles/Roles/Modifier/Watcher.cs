using System;
using UnityEngine;

namespace TheOtherRoles.Roles.Modifier;

[RegisterRole]
public class Watcher : RoleBase
{
    public Color color = new Color32(48, 21, 89, byte.MaxValue);
    public PlayerControl watcher;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        watcher = null;
    }
}