using System;
using System.Collections.Generic;
using TheOtherRoles.Options;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Poucher : RoleBase
{
    public Color color = Palette.ImpostorRed;
    public List<PlayerControl> killed = [];
    public PlayerControl poucher;

    public CustomOption poucherSpawnRate;

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void OptionCreate()
    {
        poucherSpawnRate = new CustomOption(8833, "Poucher".ColorString(color), CustomOptionHolder.rates, null, true);
    }


    public override void ClearAndReload()
    {
        poucher = null;
        killed = [];
    }
}