using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TheOtherUs.Modules;

public class AssetLoader : ILoader
{
    public void LoadFormStream(Stream stream)
    {
    }

    public void LoadFormResource(ResourceGet get)
    {
    }
}

public interface ILoader
{
    public void LoadFormStream(Stream stream);
    public void LoadFormResource(ResourceGet get);
};

#nullable enable
public sealed class ResourceGet(Assembly assembly) : IDisposable
{
    public Stream? stream { get; set; }
    public Assembly _assembly { get; } = assembly;

    public Stream? LoadFormPath(string path)
    {
        stream = _assembly.GetManifestResourceStream(path);
        return stream;
    }
    
    public Stream? LoadFormName(string name)
    {
        var names = _assembly.GetManifestResourceNames();
        var ResName = names.FirstOrDefault(n => n.EndsWith(name));
        if (ResName != null)
            stream = _assembly.GetManifestResourceStream(name);
        return stream;
    }

    public void Dispose()
    {
        stream?.Dispose();
        stream = null;
    }
}