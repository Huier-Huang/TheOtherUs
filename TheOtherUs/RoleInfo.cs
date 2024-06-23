using System.Collections.Generic;
using UnityEngine;

namespace TheOtherUs;

public class RoleInfo
{
    public static RoleInfo assassin = new("Assassin", Color.red, "Guess and shoot", "Guess and shoot",
        RoleId.EvilGuesser, false, true);
    
    public static RoleInfo morphling = new("Morphling", Morphling.color, "Change your look to not get caught",
        "Change your look", RoleId.Morphling);

    public static RoleInfo bomber2 = new("Bomber", Bomber2.color, "Give bombs to players", "Bomb Everyone",
        RoleId.Bomber2);

    public static RoleInfo mimic = new("Mimic", Mimic.color, "Pose as a crewmate by killing one", "Pose as a crewmate",
        RoleId.Mimic);

    public static RoleInfo camouflager = new("Camouflager", Camouflager.color, "Camouflage and kill the Crewmates",
        "Hide among others", RoleId.Camouflager);

    public static RoleInfo miner = new("Miner", Miner.color, "Make new Vents", "Create Vents", RoleId.Miner);

    public static RoleInfo eraser = new("Eraser", Eraser.color, "Kill the Crewmates and erase their roles",
        "Erase the roles of your enemies", RoleId.Eraser);

    public static RoleInfo vampire = new("Vampire", Vampire.color, "Kill the Crewmates with your bites",
        "Bite your enemies", RoleId.Vampire);

    public static RoleInfo cleaner = new("Cleaner", Cleaner.color, "Kill everyone and leave no traces",
        "Clean up dead bodies", RoleId.Cleaner);

    public static RoleInfo undertaker = new("Undertaker", Undertaker.color, "Kill everyone and leave no traces",
        "Drag up dead bodies to hide them", RoleId.Undertaker);

    public static RoleInfo escapist = new("Escapist", Escapist.color, "Get away from kills with ease",
        "Teleport to get away from bodies", RoleId.Escapist);

    public static RoleInfo warlock = new("Warlock", Warlock.color, "Curse other players and kill everyone",
        "Curse and kill everyone", RoleId.Warlock);

    public static RoleInfo trickster = new("Trickster", Trickster.color,
        "Use your jack-in-the-boxes to surprise others", "Surprise your enemies", RoleId.Trickster);

    public static RoleInfo bountyHunter = new("Bounty Hunter", BountyHunter.color, "Hunt your bounty down",
        "Hunt your bounty down", RoleId.BountyHunter);

    public static RoleInfo cultist = new("Cultist", Cultist.color, "Recruit for your cause", "Recruit for your cause",
        RoleId.Cultist);

    public static RoleInfo follower = new("Follower", Cleaner.color, "Follow your leader", "Follow your leader",
        RoleId.Follower, true);

    public static RoleInfo bomber = new("Terrorist", Bomber.color, "Bomb all Crewmates", "Bomb all Crewmates",
        RoleId.Bomber);

    public static RoleInfo blackmailer = new("Blackmailer", Blackmailer.color, "Blackmail those who seek to hurt you",
        "Blackmail those who seek to hurt you", RoleId.Blackmailer);

    public static RoleInfo witch = new("Witch", Witch.color, "Cast a spell upon your foes",
        "Cast a spell upon your foes", RoleId.Witch);

    public static RoleInfo ninja = new("Ninja", Ninja.color, "Surprise and assassinate your foes",
        "Surprise and assassinate your foes", RoleId.Ninja);
    //Neutral

    public static RoleInfo amnisiac = new("Amnesiac", Amnisiac.color, "Steal roles from the dead",
        "Steal roles from the dead", RoleId.Amnisiac, true);

    public static RoleInfo jester = new("Jester", Jester.color, "Get voted out", "Get voted out", RoleId.Jester, true);

    public static RoleInfo vulture = new("Vulture", Vulture.color, "Eat corpses to win", "Eat dead bodies",
        RoleId.Vulture, true);

    public static RoleInfo lawyer = new("Lawyer", Lawyer.color, "Defend your client", "Defend your client",
        RoleId.Lawyer, true);

    public static RoleInfo prosecutor = new("Prosecutor", Lawyer.color, "Vote out your target", "Vote out your target",
        RoleId.Prosecutor, true);

    public static RoleInfo pursuer = new("Pursuer", Pursuer.color, "Blank the Impostors", "Blank the Impostors",
        RoleId.Pursuer);

    public static RoleInfo jackal = new("Jackal", Jackal.color,
        "Kill all Crewmates and <color=#FF1919FF>Impostors</color> to win", "Kill everyone", RoleId.Jackal, true);

    public static RoleInfo sidekick = new("Sidekick", Sidekick.color, "Help your Jackal to kill everyone",
        "Help your Jackal to kill everyone", RoleId.Sidekick, true);

    public static RoleInfo swooper = new("Swooper", Swooper.color, "Turn Invisible and kill everyone", "Turn Invisible",
        RoleId.Swooper, false, true);

    public static RoleInfo arsonist = new("Arsonist", Arsonist.color, "Let them burn", "Let them burn", RoleId.Arsonist,
        true);

    public static RoleInfo werewolf = new("Werewolf", Werewolf.color, "Rampage and kill everyone",
        "Rampage and kill everyone", RoleId.Werewolf, true);

    public static RoleInfo thief = new("Thief", Thief.color, "Steal a killers role by killing them",
        "Steal a killers role", RoleId.Thief, true);

    //Crewmate
    public static RoleInfo crewmate = new("Crewmate", Color.white, "Find the Impostors", "Find the Impostors",
        RoleId.Crewmate);

    public static RoleInfo goodGuesser =
        new("Vigilante", Guesser.color, "Guess and shoot", "Guess and shoot", RoleId.NiceGuesser);
    
    public static RoleInfo sheriff = new("Sheriff", Sheriff.color, "Shoot the <color=#FF1919FF>Impostors</color>",
        "Shoot the Impostors", RoleId.Sheriff);


    public static RoleInfo poucher = new("Poucher", Poucher.color, "Keep info on the players you kill",
        "Investigate the kills", RoleId.Poucher);
    

    public static RoleInfo timeMaster = new("Time Master", TimeMaster.color, "Save yourself with your time shield",
        "Use your time shield", RoleId.TimeMaster);

    public static RoleInfo veteren = new("Veteran", Veteren.color, "Protect yourself from other",
        "Protect yourself from others", RoleId.Veteren);

    public static RoleInfo swapper = new("Swapper", Swapper.color,
        "Swap votes to exile the <color=#FF1919FF>Impostors</color>", "Swap votes", RoleId.Swapper);

    public static RoleInfo tracker = new("Tracker", Tracker.color, "Track the <color=#FF1919FF>Impostors</color> down",
        "Track the Impostors down", RoleId.Tracker);

    public static RoleInfo snitch = new("Snitch", Snitch.color,
        "Finish your tasks to find the <color=#FF1919FF>Impostors</color>", "Finish your tasks", RoleId.Snitch);

    public static RoleInfo spy = new("Spy", Spy.color, "Confuse the <color=#FF1919FF>Impostors</color>",
        "Confuse the Impostors", RoleId.Spy);
    

    public static RoleInfo trapper = new("Trapper", Trapper.color, "Place traps to find the Impostors", "Place traps",
        RoleId.Trapper);

    // Modifier
    public static RoleInfo bloody = new("Bloody", Color.yellow, "Your killer leaves a bloody trail",
        "Your killer leaves a bloody trail", RoleId.Bloody, false, true);

    public static RoleInfo antiTeleport = new("Anti tp", Color.yellow, "You will not get teleported",
        "You will not get teleported", RoleId.AntiTeleport, false, true);

    public static RoleInfo tiebreaker = new("Tiebreaker", Color.yellow, "Your vote breaks the tie", "Break the tie",
        RoleId.Tiebreaker, false, true);

    public static RoleInfo bait = new("Bait", Color.yellow, "Bait your enemies", "Bait your enemies", RoleId.Bait,
        false, true);

    public static RoleInfo sunglasses = new("Sunglasses", Color.yellow, "You got the sunglasses",
        "Your vision is reduced", RoleId.Sunglasses, false, true);

    public static RoleInfo torch = new("Torch", Color.yellow, "You got the torch", "You can see in the dark",
        RoleId.Torch, false, true);

    public static RoleInfo flash = new("Flash", Color.yellow, "Super speed!", "Super speed!", RoleId.Flash, false,
        true);

    public static RoleInfo multitasker = new("Multitasker", Color.yellow, "Your task windows are transparent",
        "Your task windows are transparent", RoleId.Multitasker, false, true);

    public static RoleInfo lover = new("Lover", Lovers.color, "You are in love", "You are in love", RoleId.Lover, false,
        true);

    public static RoleInfo mini = new("Mini", Color.yellow, "No one will harm you until you grow up",
        "No one will harm you", RoleId.Mini, false, true);

    public static RoleInfo vip = new("VIP", Color.yellow, "You are the VIP", "Everyone is notified when you die",
        RoleId.Vip, false, true);

    public static RoleInfo indomitable = new("Indomitable", Color.yellow, "Your role cannot be guessed",
        "You are Indomitable!", RoleId.Indomitable, false, true);

    public static RoleInfo slueth = new("Sleuth", Color.yellow, "Learn the roles of bodies you report",
        "You know the roles of bodies you report", RoleId.Slueth, false, true);

    public static RoleInfo cursed = new("Fanatic", Color.yellow, "You are crewmate....for now",
        "Discover your true potential", RoleId.Cursed, false, true, true);

    public static RoleInfo invert = new("Invert", Color.yellow, "Your movement is inverted",
        "Your movement is inverted", RoleId.Invert, false, true);

    public static RoleInfo blind = new("Blind", Color.yellow, "You cannot see your report button!",
        "Was that a dead body?", RoleId.Blind, false, true);

    public static RoleInfo watcher = new("Watcher", Color.yellow, "You can see everyone's votes during meetings",
        "Pay close attention to the crew's votes", RoleId.Watcher, false, true);

    public static RoleInfo radar = new("Radar", Color.yellow, "Be on high alert", "Be on high alert", RoleId.Radar,
        false, true);

    public static RoleInfo tunneler = new("Tunneler", Color.yellow, "Complete your tasks to gain the ability to vent",
        "Finish work so you can play", RoleId.Tunneler, false, true);

    public static RoleInfo disperser = new("Disperser", Color.red, "Separate the Crew", "Separate the Crew",
        RoleId.Disperser, false, true);

    public static RoleInfo chameleon = new("Chameleon", Color.yellow, "You're hard to see when not moving",
        "You're hard to see when not moving", RoleId.Chameleon, false, true);

    public static RoleInfo shifter = new("Shifter", Color.yellow, "Shift your role", "Shift your role", RoleId.Shifter,
        false, true);

    public static RoleInfo hunter = new("Hunter", Palette.ImpostorRed,
        Helpers.cs(Palette.ImpostorRed, "Seek and kill everyone"), "Seek and kill everyone", RoleId.Impostor);

    public static RoleInfo hunted = new("Hunted", Color.white, "Hide", "Hide", RoleId.Crewmate);

    public static RoleInfo prop = new("Prop", Color.white, "Disguise As An Object and Survive", "Disguise As An Object",
        RoleId.Crewmate);


    public static List<RoleInfo> allRoleInfos =
    [
        impostor,
        assassin,
        godfather,
        mafioso,
        janitor,
        morphling,
        bomber2,
        mimic,
        camouflager,
        miner,
        eraser,
        vampire,
        undertaker,
        escapist,
        warlock,
        trickster,
        bountyHunter,
        cultist,
        cleaner,
        bomber,
        blackmailer,
        witch,
        ninja,

        //Neutral
        amnisiac,
        jester,
        vulture,
        lawyer,
        prosecutor,
        pursuer,
        jackal,
        sidekick,
        arsonist,
        werewolf,
        thief,
        swooper,

        //Crewmate
        crewmate,
        goodGuesser,
        mayor,
        portalmaker,
        privateInvestigator,
        sheriff,
        jumper,
        timeMaster,
        veteren,
        medic,
        swapper,
        seer,
        tracker,
        snitch,
        spy,
        securityGuard,
        medium,
        trapper,

        //Modifier
        disperser,
        poucher,
        bloody,
        antiTeleport,
        tiebreaker,
        bait,
        sunglasses,
        torch,
        flash,
        multitasker,
        lover,
        mini,
        vip,
        indomitable,
        slueth,
        cursed,
        invert,
        blind,
        watcher,
        radar,
        tunneler,
        chameleon,
        shifter
    ];

    private static string ReadmePage = "";
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