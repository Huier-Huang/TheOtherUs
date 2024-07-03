using UnityEngine;

namespace TheOtherUs;

public class RoleInfo
{
    //Neutral
    

    public static RoleInfo werewolf = new("Werewolf", Werewolf.color, "Rampage and kill everyone",
        "Rampage and kill everyone", RoleId.Werewolf, true);
    
    public Color color;
    public string introDescription;
    public bool isGuessable;
    public bool isImpostor;
    public bool isModifier;
    public bool isNeutral;
    public string name;
    public RoleId roleId;
    public string shortDescription;

    public RoleInfo(string name, Color color, string introDescription, string shortDescription, RoleId roleId,
        bool isNeutral = false, bool isModifier = false, bool isGuessable = false, bool isImpostor = false)
    {
        this.color = color;
        this.name = name;
        this.introDescription = introDescription;
        this.shortDescription = shortDescription;
        this.roleId = roleId;
        this.isNeutral = isNeutral;
        this.isModifier = isModifier;
        this.isGuessable = isGuessable;
        this.isImpostor = isImpostor;
    }
}