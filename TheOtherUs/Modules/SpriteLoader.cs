using System.Collections.Generic;
using System.IO;
using System.Linq;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace TheOtherUs.Modules;

public class SpriteLoader : ILoader
{
    private readonly DirectoryInfo _directory;

    public SpriteLoader(string path) : this(new DirectoryInfo(path)) { }

    public SpriteLoader()
    {
        
    }
    
    public SpriteLoader(DirectoryInfo directory)
    {
        _directory = directory;
    }

    public List<Sprite> LoadAll(string ex, bool DontUnload, Vector2 pivot, float pixelsPerUnit)
    {
        return _directory.GetFiles(ex)
            .Select(file => LoadSprite(file.OpenRead(),  DontUnload, pivot, pixelsPerUnit)).ToList();
    }
    
    public List<Sprite> LoadAllHatSprite(string ex)
    {
        var list = new List<Sprite>();
        foreach (var file in _directory.GetFiles(ex))
        {
            var texture = LoadTexture(file.OpenRead(), true);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.53f, 0.575f), texture.width * 0.375f);
            sprite.name = $"{_directory.Name}/{file.Name.Replace(".png", string.Empty)}";
            sprite.DontDestroyOnLoad();
            list.Add(sprite);
        }

        return list;
    }
    
    public static Sprite LoadSprite(Stream stream, bool DontUnload , Vector2 pivot, float pixelsPerUnit)
    {
        var texture = LoadTexture(stream, DontUnload);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelsPerUnit);
        if (DontUnload)
            sprite.DontDestroyOnLoad();
        return sprite;
    }

    public static Texture2D LoadTexture(Stream stream, bool DontUnload)
    {
        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
        var byteTexture = stream.ReadFully();
        ImageConversion.LoadImage(texture, byteTexture, false);
        if (DontUnload)
            texture.DontDestroyOnLoad();
        return texture;
    }

    public static Sprite LoadHatSpriteFormDisk(Stream stream, string Name)
    {
        var texture = LoadTexture(stream, true);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.53f, 0.575f), texture.width * 0.375f);
        sprite.name = Name;
        sprite.DontDestroyOnLoad();
        return sprite;
    }

    public void LoadFormStream(Stream stream)
    {
    }

    public void LoadFormResource(ResourceGet get)
    {
    }
}