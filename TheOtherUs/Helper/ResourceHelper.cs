using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TheOtherUs.Helper;

public static class ResourceHelper
{
    public static string RootPath => nameof(TheOtherUs);
        
    public static string ResourcePath => RootPath.AddSplit("Resources");

    public const string SplitChar = ".";

    public static string AddSplit(this string path, string Name) => path + SplitChar + Name;

    private static Assembly _assembly => typeof(ResourceHelper).Assembly;

    public static readonly HashSet<string> Exs = [];

    public static List<string> ResPaths =>
        _assembly.GetManifestResourceNames().Where(N => N.StartsWith(ResourcePath)).ToList();

    public static string GetResPath(string Name)
    {
        return ResPaths.FirstOrDefault(n => n.GetResFileName() == Name);
    }

    public static string GetResFileName(this string resPath)
    {
        var strings = resPath.Split(SplitChar);
        return !Exs.Contains(strings[^1]) ? strings[^1] : strings[^2];
    }

    #nullable enable
    public static Stream? GetResStream(this string Path)
    {
        var stream = _assembly.GetManifestResourceStream(Path);
        return stream;
    }
}