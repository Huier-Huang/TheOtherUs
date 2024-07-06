#nullable enable
using System;
using System.Linq;
using Hazel;
using UnityEngine;

namespace TheOtherUs.Helper.RPC;

public static class FastRPCExtension
{
    public static Vector2 ReadVector2(this MessageReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        return new Vector2(x, y);
    }

    public static Vector3 ReadVector3(this MessageReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        return new Vector3(x, y, z);
    }

    public static Rect ReadRect(this MessageReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var width = reader.ReadSingle();
        var height = reader.ReadSingle();
        return new Rect(x, y, width, height);
    }

    public static PlayerControl ReadPlayer(this MessageReader reader)
    {
        var id = reader.ReadByte();
        return AllPlayers.FirstOrDefault(n => n.PlayerId == id);
    }

    public static Il2CppStructArray<byte> ReadBytesFormLength(this MessageReader reader)
    {
        var length = reader.ReadPackedInt32();
        return reader.ReadBytes(length);
    }

    public static MessageReader ReadReader(this MessageReader reader)
    {
        return MessageReader.Get(reader.ReadBytesFormLength());
    }

    public static Version ReadVersion(this MessageReader reader)
    {
        var major = reader.ReadInt32();
        var minor = reader.ReadInt32();
        var build = reader.ReadInt32();
        var revision = reader.ReadInt32();
        return revision == -1 ? new Version(major, minor, build) : new Version(major, minor, build, revision);
    }
}