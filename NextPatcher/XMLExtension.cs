using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NextPatcher;

public static class XMLExtension
{
    public static List<XmlNode> getNodes(this XmlNodeList nodeList)
    {
        return nodeList.Cast<XmlNode>().ToList();
    }

    public static IEnumerable<XmlNode> FindXML(this List<XmlNode> list, string Name)
    {
        return list.Where(n => n.Name == Name);
    }
    
    public static IEnumerable<XmlNode> FindXML(this XmlNodeList list, string Name)
    {
        return list.getNodes().FindXML(Name);
    }
    
    public static IEnumerable<XmlNode> FindXML(this XmlNode node, string Name)
    {
        return node.ChildNodes.FindXML(Name);
    }
    
    public static XmlNode FindOneXML(this XmlNode node, string Name)
    {
        return node.ChildNodes.FindXML(Name).First();
    }
}