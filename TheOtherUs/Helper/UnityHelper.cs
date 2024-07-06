using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hazel;
using Reactor.Utilities.Extensions;
using TheOtherUs.Helper.RPC;
using TheOtherUs.Modules.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using SStream = System.IO.Stream;

namespace TheOtherUs.Helper;

public static class UnityHelper
{
    public static IRegionInfo CurrentServer => FastDestroyableSingleton<ServerManager>.Instance.CurrentRegion;
    public static bool IsCustomServer => CurrentServer.TranslateName 
        is StringNames.NoTranslation || (CurrentServer.TranslateName != StringNames.ServerAS && CurrentServer.TranslateName != StringNames.ServerEU && CurrentServer.TranslateName != StringNames.ServerNA);
    
    public static readonly List<Sprite> CacheSprite = [];

    public static void SendSprite(this Sprite sprite, string spriteName, RPCSendMode mode = RPCSendMode.SendToAll,
        int TargetId = -1)
    {
        var writer = FastRpcWriter.StartNewRpcWriter(CustomRPC.SendFile, SendOption.Reliable, mode, TargetId);
        var bytes = sprite.texture.GetRawTextureData();
        writer
            .WritePacked(0)
            .Write(sprite.texture.width)
            .Write(sprite.texture.height)
            .Write((byte)sprite.texture.format)
            .Write(bytes.Length)
            .Write(bytes)
            .Write(spriteName)
            .Write(sprite.rect)
            .Write(sprite.pivot)
            .Write(sprite.pixelsPerUnit);
    }

    [RPCListener(CustomRPC.SendFile)]
    public static void ReadSprite(MessageReader reader)
    {
        if (reader.ReadPackedInt32() != 0)
            return;

        var width = reader.ReadInt32();
        var height = reader.ReadInt32();
        var format = (TextureFormat)reader.ReadByte();
        var bytesLength = reader.ReadInt32();
        var RawTextureData = reader.ReadBytes(bytesLength);
        var SpriteName = reader.ReadString();
        var rect = reader.ReadRect();
        var pivot = reader.ReadVector2();
        var pixel = reader.ReadSingle();

        var texture = new Texture2D(width, height, format, true);
        texture.LoadRawTextureData(RawTextureData);

        var sprite = Sprite.Create(texture, rect, pivot, pixel);
        sprite.name = sprite.name;
        sprite.Dont();
        CacheSprite.Add(sprite);
    }

    public static T Dont<T>(this T obj) where T : Object
    {
        obj.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
        return obj;
    }
    
    public static Sprite loadSpriteFromResources(string path, float pixelsPerUnit, bool cache = true)
    {
        try
        {
            var fileName = Path.GetFileName(path) + $"_{pixelsPerUnit}";
            if (cache && CacheSprite.Exists(n => n.name == fileName)) 
                return CacheSprite.FirstOrDefault(n => n.name == fileName);
            
            var texture = loadTextureFromResources(path);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f),
                pixelsPerUnit);
            sprite.name = fileName;
            switch (cache)
            {
                case true:
                    sprite.Dont();
                    break;
                case false:
                    return sprite;
            }

            CacheSprite.Add(sprite);
            return sprite;
        }
        catch
        {
            Error("loading sprite from path: " + path);
        }

        return null;
    }

    public static unsafe Texture2D loadTextureFromResources(string path)
    {
        try
        {
            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(path);
            var length = stream!.Length;
            var byteTexture = new Il2CppStructArray<byte>(length);
            _ = stream.Read(new Span<byte>(IntPtr.Add(byteTexture.Pointer, IntPtr.Size * 4).ToPointer(), (int)length));
            ImageConversion.LoadImage(texture, byteTexture, false);
            return texture;
        }
        catch
        {
            Error("loading texture from resources: " + path);
        }

        return null;
    }

    public static Texture2D loadTextureFromDisk(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                var texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                var byteTexture = File.ReadAllBytes(path);
                ImageConversion.LoadImage(texture, byteTexture, false);
                return texture;
            }
        }
        catch
        {
            Error("Error loading texture from disk: " + path);
        }

        return null;
    }
    
    public static Sprite LoadSprite(this SStream stream, bool DontUnload , Vector2 pivot, float pixelsPerUnit)
    {
        var texture = LoadTexture(stream, DontUnload);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelsPerUnit);
        if (DontUnload)
            sprite.Dont();
        return sprite;
    }

    public static Texture2D LoadTexture(this SStream stream, bool DontUnload)
    {
        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
        var byteTexture = stream.ReadFully();
        ImageConversion.LoadImage(texture, byteTexture, false);
        if (DontUnload)
            texture.Dont();
        return texture;
    }

    public static Sprite LoadHatSpriteFormDisk(this SStream stream, string Name)
    {
        var texture = LoadTexture(stream, true);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.53f, 0.575f), texture.width * 0.375f);
        sprite.name = Name;
        sprite.Dont();
        return sprite;
    }
    
    public static AudioClip loadAudioClipFromResources(string path, string clipName = "UNNAMED_TOR_AUDIO_CLIP")
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(path);
            var byteAudio = stream!.ReadFully();
            var samples = new float[byteAudio.Length / 4]; // 4 bytes per sample
            for (var i = 0; i < samples.Length; i++)
            {
                var offset = i * 4;
                samples[i] = (float)BitConverter.ToInt32(byteAudio, offset) / int.MaxValue;
            }

            const int channels = 2;
            const int sampleRate = 48000;
            var audioClip = AudioClip.Create(clipName, samples.Length / 2, channels, sampleRate, false);
            audioClip.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            audioClip.SetData(samples, 0);
            return audioClip;
        }
        catch
        {
            Error("loading AudioClip from resources: " + path);
        }
        return null;
    }


    public static TranslateText addTranslate(this GameObject obj, string id, bool Update = false)
    {
        TranslateText translateText;
        if (obj.TryGetComponent(out TextMeshPro text))
        {
            translateText = obj.AddComponent<TranslateText>();
            
            translateText.text = text.text;
            translateText.color = text.color;
            translateText.font = text.font;
            translateText.alignment = text.alignment;
            translateText.name = text.name;
            translateText.alpha = text.alpha;
            text.Destroy();
        }
        else
        {
            translateText = obj.AddComponent<TranslateText>();
        }
        
        translateText.Id = id;
        translateText.Update = Update;

        return translateText;
    }

    public static void AddListener(this UnityEvent @event, Action action)
    {
        @event.AddListener(action);
    }
    
    public static void AddListener<T>(this UnityEvent<T> @event, Action<T> action)
    {
        @event.AddListener(action);
    }

    public static GameObject DestroyAllChildren<T>(this GameObject obj) where T : MonoBehaviour
    {
        var list = obj.GetComponentsInChildren<T>();
        list.Do(Object.Destroy);
        return obj;
    }

    public static IRegionInfo CreateHttpRegion(string name, string ip, ushort port)
    {
        return new StaticHttpRegionInfo(name,
                StringNames.NoTranslation,
                ip,
                new Il2CppReferenceArray<ServerInfo>(
                [
                    new ServerInfo(name, ip, port, false)
                ])
            )
            .CastFast<IRegionInfo>();
    }
}