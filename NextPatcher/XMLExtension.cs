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

    public static IEnumerable<XmlNode> FindXMLAttribute(this XmlNode node, string Name, string Attribute)
    {
        var nodes = node.ChildNodes.FindXML(Name);
        var list = new List<XmlNode>();
        foreach (var n in nodes)
        {
            if (n.Attributes == null) continue;
            if (n.Attributes.Cast<XmlAttribute>().Any(attribute => attribute.Name == Attribute))
            {
                list.Add(n);
            }
        }

        return list;
    }
    
    public static XmlNode? FindXMLAttribute(this XmlNode node, string Name, string Attribute, string Value)
    {
        var nodes = node.FindXMLAttribute(Name, Attribute).ToList();
        return !nodes.Any() ? null :
            (from n in nodes from XmlAttribute attribute in n.Attributes! where attribute.Name == Attribute && attribute.Value == Value select n)
            .FirstOrDefault();
    }

    public static string GetAttributeValue(this XmlNode node, string Name)
    {
        if (node.Attributes == null) return string.Empty;
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if (attribute.Name == Name)
                return attribute.Value;
        }

        return string.Empty;
    }
}