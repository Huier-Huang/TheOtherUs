using System.Collections.Generic;
using System.Linq;
using TheOtherUs.Modules.Compatibility;
using UnityEngine;

namespace TheOtherUs.Utilities;

[Harmony]
#nullable enable
public static class MapData
{
    public static readonly List<Vector3> SkeldSpawnPosition =
    [
        new Vector3(-2.2f, 2.2f, 0.0f), //cafeteria. botton. top left.
        new Vector3(0.7f, 2.2f, 0.0f), //caffeteria. button. top right.
        new Vector3(-2.2f, -0.2f, 0.0f), //caffeteria. button. bottom left.
        new Vector3(0.7f, -0.2f, 0.0f), //caffeteria. button. bottom right.
        new Vector3(10.0f, 3.0f, 0.0f), //weapons top
        new Vector3(9.0f, 1.0f, 0.0f), //weapons bottom
        new Vector3(6.5f, -3.5f, 0.0f), //O2
        new Vector3(11.5f, -3.5f, 0.0f), //O2-nav hall
        new Vector3(17.0f, -3.5f, 0.0f), //navigation top
        new Vector3(18.2f, -5.7f, 0.0f), //navigation bottom
        new Vector3(11.5f, -6.5f, 0.0f), //nav-shields top
        new Vector3(9.5f, -8.5f, 0.0f), //nav-shields bottom
        new Vector3(9.2f, -12.2f, 0.0f), //shields top
        new Vector3(8.0f, -14.3f, 0.0f), //shields bottom
        new Vector3(2.5f, -16f, 0.0f), //coms left
        new Vector3(4.2f, -16.4f, 0.0f), //coms middle
        new Vector3(5.5f, -16f, 0.0f), //coms right
        new Vector3(-1.5f, -10.0f, 0.0f), //storage top
        new Vector3(-1.5f, -15.5f, 0.0f), //storage bottom
        new Vector3(-4.5f, -12.5f, 0.0f), //storrage left
        new Vector3(0.3f, -12.5f, 0.0f), //storrage right
        new Vector3(4.5f, -7.5f, 0.0f), //admin top
        new Vector3(4.5f, -9.5f, 0.0f), //admin bottom
        new Vector3(-9.0f, -8.0f, 0.0f), //elec top left
        new Vector3(-6.0f, -8.0f, 0.0f), //elec top right
        new Vector3(-8.0f, -11.0f, 0.0f), //elec bottom
        new Vector3(-12.0f, -13.0f, 0.0f), //elec-lower hall
        new Vector3(-17f, -10f, 0.0f), //lower engine top
        new Vector3(-17.0f, -13.0f, 0.0f), //lower engine bottom
        new Vector3(-21.5f, -3.0f, 0.0f), //reactor top
        new Vector3(-21.5f, -8.0f, 0.0f), //reactor bottom
        new Vector3(-13.0f, -3.0f, 0.0f), //security top
        new Vector3(-12.6f, -5.6f, 0.0f), // security bottom
        new Vector3(-17.0f, 2.5f, 0.0f), //upper engibe top
        new Vector3(-17.0f, -1.0f, 0.0f), //upper engine bottom
        new Vector3(-10.5f, 1.0f, 0.0f), //upper-mad hall
        new Vector3(-10.5f, -2.0f, 0.0f), //medbay top
        new Vector3(-6.5f, -4.5f, 0.0f)
    ];

    public static readonly List<Vector3> MiraSpawnPosition =
    [
        new Vector3(-4.5f, 3.5f, 0.0f), //launchpad top
        new Vector3(-4.5f, -1.4f, 0.0f), //launchpad bottom
        new Vector3(8.5f, -1f, 0.0f), //launchpad- med hall
        new Vector3(14f, -1.5f, 0.0f), //medbay
        new Vector3(16.5f, 3f, 0.0f), // comms
        new Vector3(10f, 5f, 0.0f), //lockers
        new Vector3(6f, 1.5f, 0.0f), //locker room
        new Vector3(2.5f, 13.6f, 0.0f), //reactor
        new Vector3(6f, 12f, 0.0f), //reactor middle
        new Vector3(9.5f, 13f, 0.0f), //lab
        new Vector3(15f, 9f, 0.0f), //bottom left cross
        new Vector3(17.9f, 11.5f, 0.0f), //middle cross
        new Vector3(14f, 17.3f, 0.0f), //office
        new Vector3(19.5f, 21f, 0.0f), //admin
        new Vector3(14f, 24f, 0.0f), //greenhouse left
        new Vector3(22f, 24f, 0.0f), //greenhouse right
        new Vector3(21f, 8.5f, 0.0f), //bottom right cross
        new Vector3(28f, 3f, 0.0f), //caf right
        new Vector3(22f, 3f, 0.0f), //caf left
        new Vector3(19f, 4f, 0.0f), //storage
        new Vector3(22f, -2f, 0.0f)
    ];

    public static readonly List<Vector3> PolusSpawnPosition =
    [
        new Vector3(16.6f, -1f, 0.0f), //dropship top
        new Vector3(16.6f, -5f, 0.0f), //dropship bottom
        new Vector3(20f, -9f, 0.0f), //above storrage
        new Vector3(22f, -7f, 0.0f), //right fuel
        new Vector3(25.5f, -6.9f, 0.0f), //drill
        new Vector3(29f, -9.5f, 0.0f), //lab lockers
        new Vector3(29.5f, -8f, 0.0f), //lab weather notes
        new Vector3(35f, -7.6f, 0.0f), //lab table
        new Vector3(40.4f, -8f, 0.0f), //lab scan
        new Vector3(33f, -10f, 0.0f), //lab toilet
        new Vector3(39f, -15f, 0.0f), //specimen hall top
        new Vector3(36.5f, -19.5f, 0.0f), //specimen top
        new Vector3(36.5f, -21f, 0.0f), //specimen bottom
        new Vector3(28f, -21f, 0.0f), //specimen hall bottom
        new Vector3(24f, -20.5f, 0.0f), //admin tv
        new Vector3(22f, -25f, 0.0f), //admin books
        new Vector3(16.6f, -17.5f, 0.0f), //office coffe
        new Vector3(22.5f, -16.5f, 0.0f), //office projector
        new Vector3(24f, -17f, 0.0f), //office figure
        new Vector3(27f, -16.5f, 0.0f), //office lifelines
        new Vector3(32.7f, -15.7f, 0.0f), //lavapool
        new Vector3(31.5f, -12f, 0.0f), //snowmad below lab
        new Vector3(10f, -14f, 0.0f), //below storrage
        new Vector3(21.5f, -12.5f, 0.0f), //storrage vent
        new Vector3(19f, -11f, 0.0f), //storrage toolrack
        new Vector3(12f, -7f, 0.0f), //left fuel
        new Vector3(5f, -7.5f, 0.0f), //above elec
        new Vector3(10f, -12f, 0.0f), //elec fence
        new Vector3(9f, -9f, 0.0f), //elec lockers
        new Vector3(5f, -9f, 0.0f), //elec window
        new Vector3(4f, -11.2f, 0.0f), //elec tapes
        new Vector3(5.5f, -16f, 0.0f), //elec-O2 hall
        new Vector3(1f, -17.5f, 0.0f), //O2 tree hayball
        new Vector3(3f, -21f, 0.0f), //O2 middle
        new Vector3(2f, -19f, 0.0f), //O2 gas
        new Vector3(1f, -24f, 0.0f), //O2 water
        new Vector3(7f, -24f, 0.0f), //under O2
        new Vector3(9f, -20f, 0.0f), //right outside of O2
        new Vector3(7f, -15.8f, 0.0f), //snowman under elec
        new Vector3(11f, -17f, 0.0f), //comms table
        new Vector3(12.7f, -15.5f, 0.0f), //coms antenna pult
        new Vector3(13f, -24.5f, 0.0f), //weapons window
        new Vector3(15f, -17f, 0.0f), //between coms-office
        new Vector3(17.5f, -25.7f, 0.0f)
    ];

    public static readonly List<Vector3> DleksSpawnPosition = // No Dleks
    [
        new Vector3(2.2f, 2.2f, 0.0f), //cafeteria. botton. top left.
        new Vector3(-0.7f, 2.2f, 0.0f), //caffeteria. button. top right.
        new Vector3(2.2f, -0.2f, 0.0f), //caffeteria. button. bottom left.
        new Vector3(-0.7f, -0.2f, 0.0f), //caffeteria. button. bottom right.
        new Vector3(-10.0f, 3.0f, 0.0f), //weapons top
        new Vector3(-9.0f, 1.0f, 0.0f), //weapons bottom
        new Vector3(-6.5f, -3.5f, 0.0f), //O2
        new Vector3(-11.5f, -3.5f, 0.0f), //O2-nav hall
        new Vector3(-17.0f, -3.5f, 0.0f), //navigation top
        new Vector3(-18.2f, -5.7f, 0.0f), //navigation bottom
        new Vector3(-11.5f, -6.5f, 0.0f), //nav-shields top
        new Vector3(-9.5f, -8.5f, 0.0f), //nav-shields bottom
        new Vector3(-9.2f, -12.2f, 0.0f), //shields top
        new Vector3(-8.0f, -14.3f, 0.0f), //shields bottom
        new Vector3(-2.5f, -16f, 0.0f), //coms left
        new Vector3(-4.2f, -16.4f, 0.0f), //coms middle
        new Vector3(-5.5f, -16f, 0.0f), //coms right
        new Vector3(1.5f, -10.0f, 0.0f), //storage top
        new Vector3(1.5f, -15.5f, 0.0f), //storage bottom
        new Vector3(4.5f, -12.5f, 0.0f), //storrage left
        new Vector3(-0.3f, -12.5f, 0.0f), //storrage right
        new Vector3(-4.5f, -7.5f, 0.0f), //admin top
        new Vector3(-4.5f, -9.5f, 0.0f), //admin bottom
        new Vector3(9.0f, -8.0f, 0.0f), //elec top left
        new Vector3(6.0f, -8.0f, 0.0f), //elec top right
        new Vector3(8.0f, -11.0f, 0.0f), //elec bottom
        new Vector3(12.0f, -13.0f, 0.0f), //elec-lower hall
        new Vector3(17f, -10f, 0.0f), //lower engine top
        new Vector3(17.0f, -13.0f, 0.0f), //lower engine bottom
        new Vector3(21.5f, -3.0f, 0.0f), //reactor top
        new Vector3(21.5f, -8.0f, 0.0f), //reactor bottom
        new Vector3(13.0f, -3.0f, 0.0f), //security top
        new Vector3(12.6f, -5.6f, 0.0f), // security bottom
        new Vector3(17.0f, 2.5f, 0.0f), //upper engibe top
        new Vector3(17.0f, -1.0f, 0.0f), //upper engine bottom
        new Vector3(10.5f, 1.0f, 0.0f), //upper-mad hall
        new Vector3(10.5f, -2.0f, 0.0f), //medbay top
        new Vector3(6.5f, -4.5f, 0.0f)
    ];

    public static readonly List<Vector3> AirshipSpawnPosition =
    [
        // No Spawn Position for airships
    ];

    public static readonly List<Vector3> FungleSpawnPosition =
    [
        new Vector3(-10.0842f, 13.0026f, 0.013f),
        new Vector3(0.9815f, 6.7968f, 0.0068f),
        new Vector3(22.5621f, 3.2779f, 0.0033f),
        new Vector3(-1.8699f, -1.3406f, -0.0013f),
        new Vector3(12.0036f, 2.6763f, 0.0027f),
        new Vector3(21.705f, -7.8691f, -0.0079f),
        new Vector3(1.4485f, -1.6105f, -0.0016f),
        new Vector3(-4.0766f, -8.7178f, -0.0087f),
        new Vector3(2.9486f, 1.1347f, 0.0011f),
        new Vector3(-4.2181f, -8.6795f, -0.0087f),
        new Vector3(19.5553f, -12.5014f, -0.0125f),
        new Vector3(15.2497f, -16.5009f, -0.0165f),
        new Vector3(-22.7174f, -7.0523f, 0.0071f),
        new Vector3(-16.5819f, -2.1575f, 0.0022f),
        new Vector3(9.399f, -9.7127f, -0.0097f),
        new Vector3(7.3723f, 1.7373f, 0.0017f),
        new Vector3(22.0777f, -7.9315f, -0.0079f),
        new Vector3(-15.3916f, -9.3659f, -0.0094f),
        new Vector3(-16.1207f, -0.1746f, -0.0002f),
        new Vector3(-23.1353f, -7.2472f, -0.0072f),
        new Vector3(-20.0692f, -2.6245f, -0.0026f),
        new Vector3(-4.2181f, -8.6795f, -0.0087f),
        new Vector3(-9.9285f, 12.9848f, 0.013f),
        new Vector3(-8.3475f, 1.6215f, 0.0016f),
        new Vector3(-17.7614f, 6.9115f, 0.0069f),
        new Vector3(-0.5743f, -4.7235f, -0.0047f),
        new Vector3(-20.8897f, 2.7606f, 0.002f)
    ];


    private static List<Vector3>? VentSpawnPositions;
    private static byte mapId;
    public static readonly Dictionary<PlayerControl, Vent> PlayerVentDic = new();

    public static List<Vector3> FindVentPositions()
    {
        if (VentSpawnPositions != null && mapId == GameOptionsManager.Instance.currentNormalGameOptions.MapId)
            return VentSpawnPositions;

        var poss = (from vent in DestroyableSingleton<ShipStatus>.Instance.AllVents
            select vent.transform
            into Transform
            select Transform.position
            into position
            select new Vector3(position.x, position.y, position.z)).ToList();
        mapId = GameOptionsManager.Instance.currentNormalGameOptions.MapId;
        VentSpawnPositions = poss;
        return poss;
    }

    public static void RandomSpawnAllPlayers()
    {
        RandomSpawnPlayers(AllPlayers.Select(n => n.Control));
    }

    public static void RandomSpawnPlayers(IEnumerable<PlayerControl> spawnPlayers)
    {
        if (CustomOptionHolder.randomGameStartToVents)
            RandomSpawnToVent(spawnPlayers);
        else
            RandomSpawnToMap(spawnPlayers);
    }
    
    public static void RandomSpawnAllPlayersToVent() => RandomSpawnToVent(AllPlayers.Select(n => n.Control));
    
    public static void RandomSpawnAllPlayersToMap() => RandomSpawnToMap(AllPlayers.Select(n => n.Control));


    public static void RandomSpawnToMap(IEnumerable<PlayerControl> spawnPlayer)
    {
        var newPositions = GameOptionsManager.Instance.currentNormalGameOptions.MapId switch
        {
            0 => SkeldSpawnPosition,
            1 => MiraSpawnPosition,
            2 => PolusSpawnPosition,
            3 => DleksSpawnPosition,
            4 => AirshipSpawnPosition,
            5 => FungleSpawnPosition,
            _ => []
        };

        foreach (var player in spawnPlayer)
        {
            var defPos = player.transform.position;
            var newPos = newPositions.Any() ? newPositions.Random() : defPos;
            player.NetTransform.RpcSnapTo(newPos);
        }
    }

    public static void RandomSpawnToVent(IEnumerable<PlayerControl> spawnPlayer)
    {
        var newPositions = FindVentPositions();

        foreach (var player in spawnPlayer)
        {
            var defPos = player.transform.position;
            var newPos = newPositions.Any() ? newPositions.Random() - (Vector3)player.Collider.offset : defPos;
            player.NetTransform.RpcSnapTo(newPos);
        }
    }

    public static Il2CppReferenceArray<Vent> AllVent => ShipStatus.Instance.AllVents;
    public static int GetAvailableVentId()
    {
        var id = 0;
        while (true)
        {
            if (AllVent.All(v => v.Id != id)) 
                return id;
            id++;
        }
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.EnterVent))]
    [HarmonyPostfix]
    public static void OnEnterVent(PlayerControl pc, Vent __instance)
    {
        PlayerVentDic[pc] = __instance;
    }

    [HarmonyPatch(typeof(Vent._ExitVent_d__40), nameof(Vent._ExitVent_d__40.MoveNext))]
    [HarmonyPostfix]
    public static void OnExitVent(Vent._ExitVent_d__40 __instance)
    {
        if (PlayerVentDic.ContainsKey(__instance.pc)) PlayerVentDic.Remove(__instance.pc);
    }

    public static void AllPlayerExitVent()
    {
        foreach (var (player, vent) in PlayerVentDic) player.MyPhysics.RpcExitVent(vent.Id);
    }
    // Scales
    public const float DvdScreenNewScale = 0.75f;

    // Positions
    public static readonly Vector3 DvdScreenNewPos = new(26.635f, -15.92f, 1f);
    public static readonly Vector3 VitalsNewPos = new(31.275f, -6.45f, 1f);

    public static readonly Vector3 WifiNewPos = new(15.975f, 0.084f, 1f);
    public static readonly Vector3 NavNewPos = new(11.07f, -15.298f, -0.015f);

    public static readonly Vector3 TempColdNewPos = new(25.4f, -6.4f, 1f);
    public static readonly Vector3 TempColdNewPosDV = new(7.772f, -17.103f, -0.017f);

    // Checks
    public static bool IsAdjustmentsDone;
    public static bool IsObjectsFetched;
    public static bool IsRoomsFetched;
    public static bool IsVentsFetched;

    // Tasks Tweak
    public static Console? WifiConsole;
    public static Console? NavConsole;

    // Vitals Tweak
    public static SystemConsole? Vitals;
    public static GameObject? DvdScreenOffice;

    // Vents Tweak
    public static Vent? ElectricBuildingVent;
    public static Vent? ElectricalVent;
    public static Vent? ScienceBuildingVent;
    public static Vent? StorageVent;

    // TempCold Tweak
    public static Console? TempCold;

    // Rooms
    public static GameObject? Comms;
    public static GameObject? DropShip;
    public static GameObject? Outside;
    public static GameObject? Science;

    private static void ApplyChanges(ShipStatus instance)
    {
        if (instance.Type != ShipStatus.MapType.Pb) return;
        FindPolusObjects();
        AdjustPolus();
    }

    public static void FindPolusObjects()
    {
        FindVents();
        FindRooms();
        FindObjects();
    }

    public static void AdjustPolus()
    {
        if (!CustomOptionHolder.enableBetterPolus) return;
        if (IsObjectsFetched && IsRoomsFetched)
        {
            if (CustomOptionHolder.movePolusVitals) MoveVitals();
            if (CustomOptionHolder.swapNavWifi) SwitchNavWifi();
            if (CustomOptionHolder.movePolusVitals && !CustomOptionHolder.moveColdTemp)
                MoveTempCold();
            if (CustomOptionHolder.moveColdTemp) MoveTempColdDV();
        }
        else
        {
            Error("Couldn't move elements as not all of them have been fetched.");
        }

        if (CustomOptionHolder.movePolusVents) AdjustVents(); // Programed

        IsAdjustmentsDone = true;
    }

    // --------------------
    // - Objects Fetching -
    // --------------------

    public static void FindVents()
    {
        var ventsList = Object.FindObjectsOfType<Vent>().ToList();

        if (ElectricBuildingVent == null)
            ElectricBuildingVent = ventsList.Find(vent => vent.gameObject.name == "ElectricBuildingVent")!;

        if (ElectricalVent == null) ElectricalVent = ventsList.Find(vent => vent.gameObject.name == "ElectricalVent")!;

        if (ScienceBuildingVent == null)
            ScienceBuildingVent = ventsList.Find(vent => vent.gameObject.name == "ScienceBuildingVent")!;

        if (StorageVent == null) StorageVent = ventsList.Find(vent => vent.gameObject.name == "StorageVent");

        IsVentsFetched = ElectricBuildingVent != null && ElectricalVent != null && ScienceBuildingVent != null &&
                         StorageVent != null;
    }

    public static void FindRooms()
    {
        if (Comms == null) Comms = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Comms");

        if (DropShip == null)
            DropShip = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Dropship");

        if (Outside == null) Outside = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Outside");

        if (Science == null) Science = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Science");

        IsRoomsFetched = Comms != null && DropShip != null && Outside != null && Science != null;
    }

    public static void FindObjects()
    {
        if (WifiConsole == null)
            WifiConsole = Object.FindObjectsOfType<Console>().ToList()
                .Find(console => console.name == "panel_wifi");

        if (NavConsole == null)
            NavConsole = Object.FindObjectsOfType<Console>().ToList()
                .Find(console => console.name == "panel_nav");

        if (Vitals == null)
            Vitals = Object.FindObjectsOfType<SystemConsole>().ToList()
                .Find(console => console.name == "panel_vitals");

        if (DvdScreenOffice == null)
        {
            var DvdScreenAdmin = Object.FindObjectsOfType<GameObject>().ToList()
                .Find(o => o.name == "dvdscreen");

            if (DvdScreenAdmin != null) DvdScreenOffice = Object.Instantiate(DvdScreenAdmin);
        }

        if (TempCold == null)
            TempCold = Object.FindObjectsOfType<Console>().ToList()
                .Find(console => console.name == "panel_tempcold");

        IsObjectsFetched = WifiConsole != null && NavConsole != null && Vitals != null &&
                           DvdScreenOffice != null && TempCold != null;
    }

    // -------------------
    // - Map Adjustments -
    // -------------------

    public static void AdjustVents()
    {
        if (IsVentsFetched)
        {
            if (ElectricBuildingVent != null)
                ElectricBuildingVent.Left = ElectricalVent;
            
            if (ElectricalVent != null)
                ElectricalVent.Center = ElectricBuildingVent;

            if (ScienceBuildingVent != null)
                ScienceBuildingVent.Left = StorageVent;
            if (StorageVent != null)
                StorageVent.Center = ScienceBuildingVent;
        }
        else
        {
            Error("Couldn't adjust Vents as not all objects have been fetched.");
        }
    }

    public static void MoveTempCold()
    {
        if (TempCold == null) return;
        if (TempCold.transform.position == TempColdNewPos) return;
        var tempColdTransform = TempCold.transform;
        if (Outside != null)
            tempColdTransform.parent = Outside.transform;
        
        tempColdTransform.position = TempColdNewPos;

        var collider = TempCold.GetComponent<BoxCollider2D>();
        collider.isTrigger = false;
        collider.size += new Vector2(0f, -0.3f);
    }

    public static void MoveTempColdDV()
    {
        if (TempCold == null) return;
        if (TempCold.transform.position == TempColdNewPos) return;
        var tempColdTransform = TempCold.transform;
        if (Outside != null)
            tempColdTransform.parent = Outside.transform;

        // Fixes collider being too high
        var collider = TempCold.GetComponent<BoxCollider2D>();
        collider.isTrigger = false;
        collider.size += new Vector2(0f, -0.3f);
    }
    
    public static bool zoomOutStatus;
    public static void ToggleZoom(bool reset = false)
    {
        var orthographicSize = reset || zoomOutStatus ? 3f : 12f;

        zoomOutStatus = !zoomOutStatus && !reset;
        if (Camera.main == null) return;
        Camera.main.orthographicSize = orthographicSize;
        foreach (var cam in Camera.allCameras)
            if (cam != null && cam.gameObject.name == "UI Camera")
                cam.orthographicSize =
                    orthographicSize; // The UI is scaled too, else we cant click the buttons. Downside: map is super small.

        /*if (HudManagerStartPatch.zoomOutButton != null)
        {
            HudManagerStartPatch.zoomOutButton.Sprite = zoomOutStatus
                ? UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.PlusButton.png", 75f)
                : UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.MinusButton.png", 150f);
            HudManagerStartPatch.zoomOutButton.PositionOffset =
                zoomOutStatus ? new Vector3(0f, 3f, 0) : new Vector3(0.4f, 2.8f, 0);
        }*/

        ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height,
            Screen.fullScreen); // This will move button positions to the correct position.
    }

    public static void SwitchNavWifi()
    {
        if (WifiConsole == null) return;
        if (WifiConsole.transform.position != WifiNewPos)
        {
            var wifiTransform = WifiConsole.transform;
            if (DropShip != null) 
                wifiTransform.parent = DropShip.transform;
            wifiTransform.position = WifiNewPos;
        }

        if (NavConsole == null) return;
        if (NavConsole.transform.position == NavNewPos) return;
        var navTransform = NavConsole.transform;
        if (Comms != null)
            navTransform.parent = Comms.transform;
        navTransform.position = NavNewPos;

        // Prevents crewmate being able to do the task from outside
        NavConsole.checkWalls = true;
    }

    public static void MoveVitals()
    {
        if (Vitals == null) return;
        if (Vitals.transform.position != VitalsNewPos)
        {
            // Vitals
            var vitalsTransform = Vitals.gameObject.transform;
            if (Science != null) 
                vitalsTransform.parent = Science.transform;
            vitalsTransform.position = VitalsNewPos;
        }

        if (DvdScreenOffice == null) return;
        if (DvdScreenOffice.transform.position == DvdScreenNewPos) return;
        // DvdScreen
        var dvdScreenTransform = DvdScreenOffice.transform;
        dvdScreenTransform.position = DvdScreenNewPos;

        var localScale = dvdScreenTransform.localScale;
        localScale =
            new Vector3(DvdScreenNewScale, localScale.y,
                localScale.z);
        dvdScreenTransform.localScale = localScale;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ShipStatus))]
    [HarmonyPatch(nameof(ShipStatus.Begin))]
    [HarmonyPatch(nameof(ShipStatus.Awake))]
    [HarmonyPatch(nameof(ShipStatus.FixedUpdate))]
    private static void ApplyChangePatch(ShipStatus  __instance)
    {
        /*if (!IsObjectsFetched || !IsAdjustmentsDone)*/ 
            ApplyChanges(__instance);
    }

    public static byte MapId => GameOptionsManager.Instance.CurrentGameOptions.MapId;
    public static Maps CurrentMap => (Maps)mapId;

    public static bool MapIs(Maps map)
    {
        if (map == Maps.Submerged)
            return CompatibilityManager.Instance.GetCompatibility<SubmergedCompatibility>().IsSubmerged;
        
        return CurrentMap == map;
    }
}

public enum Maps : byte
{
    Skeld = 0,
    Mira = 1,
    Polus = 2,
    Fungle = 3,
    Airship = 4,
    Submerged = 10
}