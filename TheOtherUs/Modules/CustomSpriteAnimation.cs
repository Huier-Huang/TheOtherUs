using System.Collections.Generic;
using System.Reflection;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace TheOtherUs.Modules;

[RegisterInIl2Cpp]
public class CustomSpriteAnimation : MonoBehaviour
{
    public string ResDirPath { get; set; }
    public string ResRootName { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }

    private bool Starting;
    
    public SpriteRenderer renderer;
    public List<Sprite> _sprites;

    public void Read()
    {
        _sprites.Clear();
        var currentIndex = 0;
        var current = StartIndex;
        while (current < EndIndex)
        {
            var sp = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{ResDirPath}.{ResRootName}_{current}");
            if (sp != null)
            {
                _sprites[currentIndex] = sp.LoadSprite(true, Vector2.zero, 150f);
                currentIndex++;
            }
            current++;
        }
    }

    public void Create(string resDirPath , string resRootName, int startIndex , int endIndex)
    {
        renderer ??= gameObject.AddComponent<SpriteRenderer>();
        _sprites = [];
        ResDirPath = resDirPath;
        ResRootName = resRootName;
        StartIndex = startIndex;
        EndIndex = endIndex;
        Read();
    }
    
    
    public void Play()
    {
        Starting = true;
        Show();
    }


    public int Current { get; private set; }
    public int Max => _sprites.Count - 1;
    public void Update()
    {
        if (!Starting) return;
        if (Current < 0)
            Current = 0;
        
        renderer.sprite = _sprites[Current];
        if (Current + 1 > Max)
            Current = 0;
        else
            Current++;
        
    }

    public void RePlay()
    {
        Play();
        Current = 0;
    }

    public void Stop()
    {
        Starting = false;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        renderer.enabled = true;
        enabled = true;
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        renderer.enabled = false;
        enabled = false;
    }
}