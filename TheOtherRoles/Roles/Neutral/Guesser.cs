using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheOtherRoles.Roles.Neutral;

[RegisterRole]
public class Guesser : RoleBase
{
    public PlayerControl niceGuesser;

    //public static PlayerControl evilGuesser;
    public List<PlayerControl> evilGuesser = new();
    public Color color = new Color32(255, 255, 0, byte.MaxValue);

    public int remainingShotsEvilGuesser = 2;
    public int remainingShotsNiceGuesser = 2;
    public bool hasMultipleShotsPerMeeting;
    public bool assassinMultipleShotsPerMeeting;
    public bool showInfoInGhostChat = true;
    public bool killsThroughShield = true;
    public bool assassinKillsThroughShield = true;
    public bool evilGuesserCanGuessSpy = true;
    public bool guesserCantGuessSnitch;
    public bool evilGuesserCanGuessCrewmate = true;

    public bool isGuesser(byte playerId)
    {
        if (evilGuesser.Any(item => item.PlayerId == playerId && evilGuesser != null))
        {
            return true;
        }
        return niceGuesser != null && niceGuesser.PlayerId == playerId;
    }

    public void clear(byte playerId)
    {
        if (niceGuesser != null && niceGuesser.PlayerId == playerId) niceGuesser = null;
        foreach (var item in evilGuesser.Where(item => item.PlayerId == playerId && evilGuesser != null))
            evilGuesser = null;
    }

    public int remainingShots(byte playerId, bool shoot = false)
    {
        var result = remainingShotsEvilGuesser;
        if (niceGuesser != null && niceGuesser.PlayerId == playerId)
        {
            result = remainingShotsNiceGuesser;
            if (shoot) remainingShotsNiceGuesser = Mathf.Max(0, remainingShotsNiceGuesser - 1);
        }
        else if (shoot)
        {
            remainingShotsEvilGuesser = Mathf.Max(0, remainingShotsEvilGuesser - 1);
        }

        return result;
    }

    public override void ClearAndReload()
    {
        niceGuesser = null;
        evilGuesser = new List<PlayerControl>();
        guesserCantGuessSnitch = CustomOptionHolder.guesserCantGuessSnitchIfTaksDone.getBool();
        remainingShotsEvilGuesser = Mathf.RoundToInt(CustomOptionHolder.modifierAssassinNumberOfShots.getFloat());
        remainingShotsNiceGuesser = Mathf.RoundToInt(CustomOptionHolder.guesserNumberOfShots.getFloat());
        hasMultipleShotsPerMeeting = CustomOptionHolder.guesserHasMultipleShotsPerMeeting.getBool();
        assassinMultipleShotsPerMeeting = CustomOptionHolder.modifierAssassinMultipleShotsPerMeeting.getBool();
        showInfoInGhostChat = CustomOptionHolder.guesserShowInfoInGhostChat.getBool();
        killsThroughShield = CustomOptionHolder.guesserKillsThroughShield.getBool();
        assassinKillsThroughShield = CustomOptionHolder.modifierAssassinKillsThroughShield.getBool();
        evilGuesserCanGuessSpy = CustomOptionHolder.guesserEvilCanKillSpy.getBool();
        evilGuesserCanGuessCrewmate = CustomOptionHolder.guesserEvilCanKillCrewmate.getBool();
    }

    public override RoleInfo RoleInfo { get; protected set; }
    public override Type RoleType { get; protected set; }
}