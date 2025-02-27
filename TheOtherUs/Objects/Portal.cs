using System;
using System.Collections.Generic;
using TheOtherUs.Modules.Compatibility;
using UnityEngine;

namespace TheOtherUs.Objects;

public class Portal
{
    public static Portal firstPortal;
    public static Portal secondPortal;
    public static bool bothPlacedAndEnabled;
    public static Sprite[] portalFgAnimationSprites = new Sprite[205];
    public static Sprite portalSprite;
    public static bool isTeleporting;
    public static float teleportDuration = 3.4166666667f;

    public static List<tpLogEntry> teleportedPlayers;
    private readonly SpriteRenderer animationFgRenderer;

    public GameObject portalFgAnimationGameObject;
    public GameObject portalGameObject;
    private readonly SpriteRenderer portalRenderer;
    public string room;

    public Portal(Vector2 p)
    {
        portalGameObject = new GameObject("Portal") { layer = 11 };
        //Vector3 position = new Vector3(p.x, p.y, CachedPlayer.LocalPlayer.transform.position.z + 1f);
        var position = new Vector3(p.x, p.y, (p.y / 1000f) + 0.01f);

        // Create the portal            
        portalGameObject.transform.position = position;
        portalGameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
        portalRenderer = portalGameObject.AddComponent<SpriteRenderer>();
        portalRenderer.sprite = portalSprite;

        var fgPosition = new Vector3(0, 0, -1f);
        portalFgAnimationGameObject = new GameObject("PortalAnimationFG");
        portalFgAnimationGameObject.transform.SetParent(portalGameObject.transform);
        portalFgAnimationGameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
        portalFgAnimationGameObject.transform.localPosition = fgPosition;
        animationFgRenderer = portalFgAnimationGameObject.AddComponent<SpriteRenderer>();
        animationFgRenderer.material = FastDestroyableSingleton<HatManager>.Instance.PlayerMaterial;

        // Only render the inactive portals for the Portalmaker
        var playerIsPortalmaker = LocalPlayer.Is<PortalMaker>();
        portalGameObject.SetActive(playerIsPortalmaker);
        portalFgAnimationGameObject.SetActive(true);

        if (firstPortal == null) firstPortal = this;
        else if (secondPortal == null) secondPortal = this;
        var lastRoom = FastDestroyableSingleton<HudManager>.Instance?.roomTracker.LastRoom.RoomId;
        room = lastRoom != null
            ? DestroyableSingleton<TranslationController>.Instance.GetString((SystemTypes)lastRoom)
            : "Open Field";
    }

    public static Sprite getFgAnimationSprite(int index)
    {
        if (portalFgAnimationSprites == null || portalFgAnimationSprites.Length == 0) return null;
        index = Mathf.Clamp(index, 0, portalFgAnimationSprites.Length - 1);
        if (portalFgAnimationSprites[index] == null)
            portalFgAnimationSprites[index] =
                UnityHelper.loadSpriteFromResources($"TheOtherUs.Resources.PortalAnimation.portal_{index:000}.png",
                    115f);
        return portalFgAnimationSprites[index];
    }

    public static void startTeleport(byte playerId, byte exit)
    {
        if (firstPortal == null || secondPortal == null) return;
        isTeleporting = true;

        // Generate log info
        var playerControl = playerId.GetCachePlayer();
        var flip = playerControl.cosmeticsLayer.currentBodySprite.BodySprite
            .flipX; // use the original player control here, not the morhpTarget.
        firstPortal.animationFgRenderer.flipX = flip;
        secondPortal.animationFgRenderer.flipX = flip;
        /*if (Morphling.morphling != null && Morphling.morphTimer > 0)
            playerControl = Morphling.morphTarget; // Will output info of morph-target instead
        var playerNameDisplay = PortalMaker.logOnlyHasColors
            ? "A player (" + (Helpers.isLighterColor(playerControl) ? "L" : "D") + ")"
            : playerControl.Data.PlayerName;*/

        /*var colorId = playerControl.Data.DefaultOutfit.ColorId;

        if (Camouflager.camouflageTimer > 0 || Helpers.MushroomSabotageActive())
        {
            playerNameDisplay = "A camouflaged player";
            colorId = 6;
        }*/

        if (!playerControl.IsDead)
            /*teleportedPlayers.Add(new tpLogEntry(playerId, playerNameDisplay, DateTime.UtcNow));*/

        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(teleportDuration,
            new Action<float>(p =>
            {
                if (firstPortal != null && firstPortal.animationFgRenderer != null && secondPortal != null &&
                    secondPortal.animationFgRenderer != null)
                {
                    if (exit == 0 || exit == 1)
                        firstPortal.animationFgRenderer.sprite =
                            getFgAnimationSprite((int)(p * portalFgAnimationSprites.Length));
                    if (exit == 0 || exit == 2)
                        secondPortal.animationFgRenderer.sprite =
                            getFgAnimationSprite((int)(p * portalFgAnimationSprites.Length));
                    playerControl.Control.SetPlayerMaterialColors(firstPortal.animationFgRenderer);
                    playerControl.Control.SetPlayerMaterialColors(secondPortal.animationFgRenderer);
                    if ((int)p != 1) return;
                    firstPortal.animationFgRenderer.sprite = null;
                    secondPortal.animationFgRenderer.sprite = null;
                    isTeleporting = false;
                }
            })));
    }

    public static bool locationNearEntry(Vector2 p)
    {
        if (!bothPlacedAndEnabled) return false;
        var maxDist = 0.25f;

        var dist1 = Vector2.Distance(p, firstPortal.portalGameObject.transform.position);
        var dist2 = Vector2.Distance(p, secondPortal.portalGameObject.transform.position);
        if (dist1 > maxDist && dist2 > maxDist) return false;
        return true;
    }

    public static Vector2 findExit(Vector2 p)
    {
        var dist1 = Vector2.Distance(p, firstPortal.portalGameObject.transform.position);
        var dist2 = Vector2.Distance(p, secondPortal.portalGameObject.transform.position);
        return dist1 < dist2
            ? secondPortal.portalGameObject.transform.position
            : firstPortal.portalGameObject.transform.position;
    }

    public static Vector2 findEntry(Vector2 p)
    {
        var dist1 = Vector2.Distance(p, firstPortal.portalGameObject.transform.position);
        var dist2 = Vector2.Distance(p, secondPortal.portalGameObject.transform.position);
        return dist1 > dist2
            ? secondPortal.portalGameObject.transform.position
            : firstPortal.portalGameObject.transform.position;
    }

    public static void meetingEndsUpdate()
    {
        // checkAndEnable
        if (secondPortal != null)
        {
            firstPortal.portalGameObject.SetActive(true);
            secondPortal.portalGameObject.SetActive(true);
            bothPlacedAndEnabled = true;
            /*HudManagerStartPatch.portalmakerButtonText2.text = "2. " + secondPortal.room;*/
        }

        // reset teleported players
        teleportedPlayers = [];
    }

    private static void preloadSprites()
    {
        for (var i = 0; i < portalFgAnimationSprites.Length; i++) getFgAnimationSprite(i);
        portalSprite = UnityHelper.loadSpriteFromResources("TheOtherUs.Resources.PortalAnimation.plattform.png", 115f);
    }

    public static void clearPortals()
    {
        preloadSprites(); // Force preload of sprites to avoid lag
        bothPlacedAndEnabled = false;
        firstPortal = null;
        secondPortal = null;
        isTeleporting = false;
        teleportedPlayers = [];
    }

    public struct tpLogEntry(byte playerId, string name, DateTime time)
    {
        public byte playerId = playerId;
        public string name = name;
        public DateTime time = time;
    }
}