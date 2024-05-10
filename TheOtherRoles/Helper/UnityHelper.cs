using System.Collections.Generic;
using Hazel;
using UnityEngine;

namespace TheOtherRoles.Helper;

public static class UnityHelper
{
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
        sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
        CacheSprite.Add(sprite);
    }
}