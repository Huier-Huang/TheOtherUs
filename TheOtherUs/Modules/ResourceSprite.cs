using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TheOtherUs.Modules;

#nullable enable
public class ResourceSprite(
    string pathName = "",
    float pixel = 115f,
    bool cache = true,
    Action<ResourceSprite>? onGetSprite = null)
{
    private const string ResourcePath = "TheOtherUs.Resources.";

    private static readonly Assembly assembly = Assembly.GetExecutingAssembly();

    public bool _cache = cache;

    public string _pathName = pathName;

    public float _pixel = pixel;
    private Sprite? _sprite;

    public Sprite? ReturnSprite = null;

    public string Path => GetPath();

    public object? Instance { get; set; }

    public static implicit operator Sprite(ResourceSprite rs)
    {
        return rs.GetSprite();
    }

    public event Action<ResourceSprite>? OnGetSprite = onGetSprite;

    public Sprite GetSprite()
    {
        OnGetSprite?.Invoke(this);

        if (ReturnSprite != null)
            return ReturnSprite;

        if (_sprite != null && _sprite.pixelsPerUnit == _pixel)
            return _sprite;

        _sprite = UnityHelper.loadSpriteFromResources(GetPath(), _pixel, _cache);
        return _sprite;
    }

    private string GetPath()
    {
        if (assembly.GetManifestResourceNames().Contains(ResourcePath + _pathName)) return ResourcePath + _pathName;

        return _pathName;
    }
}