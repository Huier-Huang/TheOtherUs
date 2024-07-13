using System.IO;
using System.Linq;
using Innersloth.IO;
using UnityEngine;

namespace TheOtherUs.Configs;

[Harmony]
public static class VersionIsolationPatch
{
    [HarmonyPatch(typeof(PlatformPaths), nameof(PlatformPaths.persistentDataPath), MethodType.Getter), HarmonyPrefix]
    private static bool FileIoGetRootDataPathPatch(ref string __result)
    {
        var dirPath = Path.Combine(Application.persistentDataPath, Application.version);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        __result = dirPath;
        return false;
    }
}