using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InnerNet;
using TheOtherRoles.CustomGameMode;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.Patches;

[HarmonyPatch]
public static class CredentialsPatch
{
    public static string fullCredentialsVersion =
        $@"<size=130%><color=#ff351f>TheOtherUs</color></size> v{TheOtherRolesPlugin.Version}";

    public static string fullCredentials =
        @"<size=60%>Modified by <color=#FCCE03FF>Spex</color>
Based on TheOtherRoles";

    public static string mainMenuCredentials =
        @"Modified by <color=#FCCE03FF>Spex</color>, based on TheOtherRoles by <color=#FCCE03FF>Eisbison</color>, <color=#FCCE03FF>Thunderstorm584</color>, 
<color=#FCCE03FF>EndOfFile</color>, <color=#FCCE03FF>Mallöris</color> & <color=#FCCE03FF>Gendelo</color>
Design by <color=#FCCE03FF>Bavari</color>";

    public static string contributorsCredentials =
        @"<size=60%> <color=#FCCE03FF>Special thanks to Smeggy, Scoom, Xer, Mr_Fluuff, Fangkuai, and mxyx-club</color></size>";

    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    internal static class PingTrackerPatch
    {
        public static GameObject modStamp;

        private static void Postfix(PingTracker __instance)
        {
            __instance.text.alignment = TextAlignmentOptions.TopRight;
            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
            {
                var gameModeText = "";
                if (HideNSeek.isHideNSeekGM) gameModeText = "Hide 'N Seek";
                else if (HandleGuesser.isGuesserGm) gameModeText = "Guesser";
                else if (PropHunt.isPropHuntGM) gameModeText = "Prop Hunt";
                if (gameModeText != "") gameModeText = Helpers.cs(Color.yellow, gameModeText) + "\n";
                __instance.text.text =
                    $"<size=130%><color=#ff351f>TheOtherUs</color></size> v{TheOtherRolesPlugin.Version + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}\n{gameModeText}" +
                    __instance.text.text;
                if (CachedPlayer.LocalPlayer.Data.IsDead || (!(CachedPlayer.LocalPlayer.Control == null) &&
                                                             (CachedPlayer.LocalPlayer.Control == Lovers.lover1 ||
                                                              CachedPlayer.LocalPlayer.Control == Lovers.lover2)))
                {
                    var transform = __instance.transform;
                    var localPosition = transform.localPosition;
                    localPosition = new Vector3(3.45f, localPosition.y, localPosition.z);
                    transform.localPosition = localPosition;
                }
                else
                {
                    var transform = __instance.transform;
                    var localPosition = transform.localPosition;
                    localPosition = new Vector3(4.2f, localPosition.y, localPosition.z);
                    transform.localPosition = localPosition;
                }
            }
            else
            {
                var gameModeText = MapOptions.gameMode switch
                {
                    CustomGameModes.HideNSeek => "Hide 'N Seek",
                    CustomGameModes.Guesser => "Guesser",
                    CustomGameModes.PropHunt => "Prop Hunt",
                    _ => string.Empty
                };
                if (gameModeText != "") gameModeText = Helpers.cs(Color.yellow, gameModeText) + "\n";

                __instance.text.text =
                    $"{fullCredentialsVersion}\n  {gameModeText + fullCredentials}\n {__instance.text.text}";
                var transform = __instance.transform;
                var localPosition = transform.localPosition;
                localPosition = new Vector3(3.5f, localPosition.y, localPosition.z);
                transform.localPosition = localPosition;
            }
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class LogoPatch
    {
        public static SpriteRenderer renderer;
        public static Sprite bannerSprite;
        public static Sprite horseBannerSprite;
        public static Sprite banner2Sprite;
        private static PingTracker instance;

        public static GameObject motdObject;
        public static TextMeshPro motdText;

        private static void Postfix(PingTracker __instance)
        {
            var torLogo = new GameObject("bannerLogo_TOR");
            torLogo.transform.SetParent(GameObject.Find("RightPanel").transform, false);
            torLogo.transform.localPosition = new Vector3(-0.4f, 1f, 5f);

            renderer = torLogo.AddComponent<SpriteRenderer>();
            loadSprites();
            renderer.sprite = UnityHelper.loadSpriteFromResources("TheOtherRoles.Resources.Banner.png", 300f);

            instance = __instance;
            loadSprites();
            renderer.sprite = bannerSprite;
            var credentialObject = new GameObject("credentialsTOR");
            var credentials = credentialObject.AddComponent<TextMeshPro>();
            credentials.SetText(
                $"v{Main.Version}\n<size=30f%>\n</size>{mainMenuCredentials}\n<size=30%>\n</size>{contributorsCredentials}");
            credentials.alignment = TextAlignmentOptions.Center;
            credentials.fontSize *= 0.05f;

            credentials.transform.SetParent(torLogo.transform);
            credentials.transform.localPosition = Vector3.down * 1.25f;
            motdObject = new GameObject("torMOTD");
            motdText = motdObject.AddComponent<TextMeshPro>();
            motdText.alignment = TextAlignmentOptions.Center;
            motdText.fontSize *= 0.04f;

            motdText.transform.SetParent(torLogo.transform);
            motdText.enableWordWrapping = true;
            var rect = motdText.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(5.2f, 0.25f);

            motdText.transform.localPosition = Vector3.down * 2.25f;
            motdText.color = new Color(1, 53f / 255, 31f / 255);
            var mat = motdText.fontSharedMaterial;
            mat.shaderKeywords = new[] { "OUTLINE_ON" };
            motdText.SetOutlineColor(Color.white);
            motdText.SetOutlineThickness(0.025f);
        }

        public static void loadSprites()
        {
            if (bannerSprite == null)
                bannerSprite = UnityHelper.loadSpriteFromResources("TheOtherRoles.Resources.Banner.png", 300f);
            if (banner2Sprite == null)
                banner2Sprite = UnityHelper.loadSpriteFromResources("TheOtherRoles.Resources.Banner2.png", 300f);
            if (horseBannerSprite == null)
                horseBannerSprite =/**/
                    UnityHelper.loadSpriteFromResources("TheOtherRoles.Resources.bannerTheHorseRoles.png", 300f);
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
    public static class MOTD
    {
        public static List<string> motds = [];
        private static float timer;
        private static readonly float maxTimer = 5f;
        private static int currentIndex;

        public static void Postfix()
        {
            if (motds.Count == 0)
            {
                timer = maxTimer;
                return;
            }

            if (motds.Count > currentIndex && LogoPatch.motdText != null)
                LogoPatch.motdText.SetText(motds[currentIndex]);
            else return;

            // fade in and out:
            var alpha = Mathf.Clamp01(Mathf.Min([timer, maxTimer - timer]));
            if (motds.Count == 1) alpha = 1;
            LogoPatch.motdText.color = LogoPatch.motdText.color.SetAlpha(alpha);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = maxTimer;
                currentIndex = (currentIndex + 1) % motds.Count;
            }
        }

        public static async Task loadMOTDs()
        {
            var client = new HttpClient();
            var response =
                await client.GetAsync("https://raw.githubusercontent.com/TheOtherRolesAU/MOTD/main/motd.txt");
            response.EnsureSuccessStatusCode();
            var motds = await response.Content.ReadAsStringAsync();
            foreach (var line in motds.Split("\n", StringSplitOptions.RemoveEmptyEntries)) MOTD.motds.Add(line);
        }
    }
}