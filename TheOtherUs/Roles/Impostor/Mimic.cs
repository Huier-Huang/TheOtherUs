using System;
using System.Collections.Generic;
using TheOtherUs.Options;
using TheOtherUs.Utilities;
using UnityEngine;

namespace TheOtherUs.Roles.Impostor;

[RegisterRole]
public class Mimic : RoleBase
{
    public Color color = Palette.ImpostorRed;
    public bool hasMimic;
    public List<PlayerControl> killed = [];
    public PlayerControl mimic;

    public CustomOption mimicSpawnRate;
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }


    public override void ClearAndReload()
    {
        mimic = null;
        hasMimic = false;
    }


    public override void OptionCreate()
    {
        mimicSpawnRate = new CustomOption(8835, "Mimic".ColorString(color), CustomOptionHolder.rates, null, true);
    }
}