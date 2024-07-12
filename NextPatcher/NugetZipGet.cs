using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Xml;
using Cpp2IL.Core.Extensions;


namespace NextPatcher;

public class NugetZipGet(NuGetDownloader downloader)
{
    public readonly ZipArchive ZipArchive = new(downloader.Download().Result);

    public string[] GetFrameworks()
    {
        var document = new XmlDocument();
        document.LoadXml(GetDocumentText());
        var groups = document
            .DocumentElement?
            .FirstChild?
            .FindOneXML("dependencies")
            .FindXML("group");
        
        return groups == null
                ? []
                : (from XmlElement @group in groups select @group.GetAttribute("targetFramework"))
                .ToArray();
        
    }
    
    public string GetDocumentText()
    {
        return new StreamReader(ZipArchive.Entries
            .First(n => n.FullName.EndsWith(".nuspec"))
            .Open()).ReadToEnd();
    }
    
    public Stream GetAssemblyStream(string Framework)
    {
        NextPatcher.LogSource.LogInfo($"GetStream {Framework}");
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