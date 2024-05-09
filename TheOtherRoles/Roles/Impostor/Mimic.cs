using System;
using System.Collections.Generic;
using Hazel;
using TheOtherRoles.Modules.Options;
using TheOtherRoles.Objects;
using UnityEngine;


namespace TheOtherRoles.Roles.Impostor;

[RegisterRole]
public class Mimic : RoleBase
{
    public PlayerControl mimic;
    public bool hasMimic;
    public Color color = Palette.ImpostorRed;
    public List<PlayerControl> killed = new();

    public  CustomOption mimicSpawnRate;


    public override void ClearAndReload()
    {
        mimic = null;
        hasMimic = false;
    }


    public override void OptionCreate()
    {
        mimicSpawnRate = new CustomOption(8835, "Mimic".ColorString(color), CustomOptionHolder.rates, null, true);
    }
    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
    
}