using System;
using System.Collections.Generic;
using TheOtherRoles.Modules.Options;
using UnityEngine;

namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Poucher : RoleBase
{
    public PlayerControl poucher;
    public Color color = Palette.ImpostorRed;
    public List<PlayerControl> killed = new();

    public CustomOption poucherSpawnRate;


    public override void OptionCreate()
    {
        poucherSpawnRate = new CustomOption(8833, "Poucher".ColorString(color), CustomOptionHolder.rates, null, true);
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    

    public override void ClearAndReload()
    {
        poucher = null;
        killed = [];
    }
}