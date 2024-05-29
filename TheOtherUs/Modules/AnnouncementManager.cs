using System;
using System.Collections.Generic;
using System.Net.Http;
using Assets.InnerNet;
using TheOtherUs.Patches;
using UnityEngine;

namespace TheOtherUs.Modules;

public class AnnouncementManager : ManagerBase<AnnouncementManager>
{
    public List<Announcement> ModAnnouncements = [];
    public readonly List<Motd> Motds = [];
    public string ReadmePage = "";
    
    public static string AnnouncementUrl => $"{DownloadHelper.RepoRawURL}/Announcements.json";
    public void DownloadAnnouncements()
    {
    }

    public static string READMEUrl => $"{DownloadHelper.RepoRawURL}/README.md";
    
    public async void DownLoadREADME()
    {
        if (ReadmePage != "") return;
        using var client = new HttpClient();
        var response =
            await client.GetAsync(READMEUrl);
        response.EnsureSuccessStatusCode();
        var http = await response.Content.ReadAsStringAsync();
        ReadmePage = http;
    }
    
    public static string MotdUrl => $"{DownloadHelper.RepoRawURL}/Motd.txt";
    public async void DownloadMOTDs()
    {
        using var client = new HttpClient();
        var response =
            await client.GetAsync(MotdUrl);
        response.EnsureSuccessStatusCode();
        var motds = await response.Content.ReadAsStringAsync();
        foreach (var line in motds.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            Motds.Add(new Motd());
    }
    
    public class Motd
    {
        public string Content { get; set; }

        public static implicit operator string(Motd motd) => motd.Content;
    }

    private float timer;
    private readonly float maxTimer = 5f;
    private int currentIndex;

    public void UpdateMotd()
    {
        if (Motds.Count == 0)
        {
            timer = maxTimer;
            return;
        }

        if (Motds.Count <= currentIndex || CredentialsPatch.LogoPatch.motdText == null)
            return;
        CredentialsPatch.LogoPatch.motdText.SetText(Motds[currentIndex]);

        // fade in and out:
        var alpha = Mathf.Clamp01(Mathf.Min([timer, maxTimer - timer]));
        if (Motds.Count == 1) alpha = 1;
        CredentialsPatch.LogoPatch.motdText.color = CredentialsPatch.LogoPatch.motdText.color.SetAlpha(alpha);
        timer -= Time.deltaTime;
        if (!(timer <= 0)) return;
        timer = maxTimer;
        currentIndex = (currentIndex + 1) % Motds.Count;
    }
}