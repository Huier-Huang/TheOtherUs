using System.IO;
using System.Linq;
using Innersloth.IO;
using UnityEngine;

namespace TheOtherUs.Configs;

public static class VersionIsolationPatch
{
    [HarmonyPatch(typeof(FileIO), nameof(FileIO.GetDataPathTo)), HarmonyPrefix]
    private static bool FileIoGetDataPathToPatch(string[] directories, ref string __result)
    {
        if (!TheOtherUsConfig.EnableVersionIsolation) return true;

        var dirPath = Path.Combine(FileIO.GetRootDataPath(), Application.version);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        var path = directories.Where(p => !p.IsNullOrWhiteSpace()).Aggregate(dirPath, Path.Combine);
         __result = path;
        return false;
    }
}