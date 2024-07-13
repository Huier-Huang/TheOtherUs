using System.Reflection;
using System.Runtime.Versioning;

namespace NextPatcher;

public static class AssemblyExtension
{
    public static TargetFrameworkAttribute GetFramework(this Assembly assembly)
    {
        return assembly.GetCustomAttribute<TargetFrameworkAttribute>()!;
    }

    public static string GetFrameworkVersion(this TargetFrameworkAttribute attribute)
    {
        return attribute.FrameworkDisplayName?.Split(" ")[1] ?? string.Empty;
    }

    public static string GetFrameworkName(this TargetFrameworkAttribute attribute)
    {
        return attribute.FrameworkDisplayName?.Split(" ")[0] ?? string.Empty;
    }

    public static string GetNugetName(this TargetFrameworkAttribute attribute)
    {
        var version = attribute.GetFrameworkVersion();
        var name = attribute.GetFrameworkName().ToLowerInvariant().Replace(".", string.Empty);
        return name + version;
    }
}