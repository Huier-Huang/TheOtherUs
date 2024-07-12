using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Cpp2IL.Core.Extensions;

namespace NextPatcher;

public class NugetZipGet(NuGetDownloader downloader)
{
    public readonly ZipArchive ZipArchive = new(downloader.Download().Result);

    public Stream GetAssemblyStream(string Framework)
    {
        return ZipArchive.Entries
            .Where(n => n.FullName.EndsWith(".dll"))
            .First(n => n.FullName.Contains(Framework))
            .Open();
    }

    public Assembly GetAssembly(string Framework)
    {
        return Assembly.Load(GetAssemblyStream(Framework).ReadBytes());
    }
}