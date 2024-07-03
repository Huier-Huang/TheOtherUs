using AmongUs.GameOptions;
using InnerNet;

namespace TheOtherUs.Utilities;

public static class GameStates
{ 
    public static bool InGame = false;
        
    public static bool AlreadyDied = false;

    /**********Check Game Status***********/
    public static bool HasGameStart => GameManager.Instance.GameHasStarted;
    public static bool IsHost => AmongUsClient.Instance.HostId == AmongUsClient.Instance.ClientId;
        
    public static bool IsNormalGame =>
        GameOptionsManager.Instance.CurrentGameOptions.GameMode is GameModes.Normal or GameModes.NormalFools;
        
    public static bool IsHideNSeek =>
        GameOptionsManager.Instance.CurrentGameOptions.GameMode is GameModes.HideNSeek or GameModes.SeekFools;
    public static bool IsLobby => AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Joined; 
    public static bool IsInGame => InGame; 
    public static bool IsEnded => AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Ended; 
    public static bool IsNotJoined => AmongUsClient.Instance.GameState == InnerNetClient.GameStates.NotJoined; 
    public static bool IsOnlineGame => AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame; 
    public static bool IsLocalGame => AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame; 
    public static bool IsFreePlay => AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay; 
    public static bool IsInTask => InGame && !MeetingHud.Instance; 
    public static bool IsMeeting => InGame && MeetingHud.Instance;
        
    public static bool IsVoting => IsMeeting &&
                                   MeetingHud.Instance.state is MeetingHud.VoteStates.Voted
                                       or MeetingHud.VoteStates.NotVoted;
        
    public static bool IsProceeding => IsMeeting && MeetingHud.Instance.state is MeetingHud.VoteStates.Proceeding;

    public static bool IsExilling => ExileController.Instance != null;
        
    public static bool IsCountDown => GameStartManager.InstanceExists &&
                                      GameStartManager.Instance.startState == GameStartManager.StartingStates.Countdown;

    /**********TOP ZOOM.cs***********/ 
    public static bool IsShip => ShipStatus.Instance != null; 
}