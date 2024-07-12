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
    public XmlDocument? NugetDocument { get; set; }

    public XmlDocument GetOrLoadDocument()
    {
        if (NugetDocument != null)
            return NugetDocument;
        NugetDocument = new XmlDocument();
        NugetDocument.LoadXml(GetDocumentText());
        return NugetDocument;
    }

    public string[] GetFrameworks()
    {
        var groups = GetOrLoadDocument()
            .DocumentElement?
            .FirstChild?
            .FindOneXML("dependencies")
            .FindXML("group");
        
        return groups == null
                ? []
                : (from XmlElement @group in groups select @group.GetAttribute("targetFramework"))
                .ToArray();
        
    }

    public List<DependencyInfo> GetDependency(string Framework)
    {
        var list = new List<DependencyInfo>();
        var group = GetOrLoadDocument()
            .DocumentElement?
            .FirstChild?
            .FindOneXML("dependencies")
            .FindXMLAttribute("group", "targetFramework", Framework);
        if (group != null)
        {
            list.AddRange(group.FindXML("dependency").Select(dependency => new DependencyInfo() { Id = dependency.GetAttributeValue("id"), Version = dependency.GetAttributeValue("version") }));
        }
        return list;
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