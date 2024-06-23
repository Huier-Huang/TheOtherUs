using System.Collections.Generic;
using System.Text;


namespace TheOtherUs.Chat;

public class EnvironmentTextManager : ManagerBase<EnvironmentTextManager>
{
    private readonly EnvironmentTextList textList = [];
    public IReadOnlyList<EnvironmentText> List => textList;
    public string CurrentEnvironment { get; set; } = string.Empty;
    public char CurrentRegexChar { get; set; }
    
    public EnvironmentTextManager Add(EnvironmentText environmentText)
    {
        textList.Add(environmentText);
        return this;
    }

    public EnvironmentTextManager Remove(string Org, string target)
    {
        textList.RemoveAll(n => n.Org == Org && n.Target == target);
        return this;
    }

    public string StartReplace(string text, bool isEnvironment = false)
    {
        return text;
    }
    
    public string StartReplaceRegex(string text, bool isEnvironment = false)
    {
        var builder = new StringBuilder();
        var state = false;
        var regexText = string.Empty;
        foreach (var c in text)
        {
            if (c == CurrentRegexChar)
            {
                if (!state)
                {
                    builder.Append(textList[regexText].Target);
                    regexText = string.Empty;
                }
                state = !state;
                continue;
            }

            if (!state)
                builder.Append(c);
            else
                regexText += c;
        }
        return builder.ToString();
    }
}

public sealed class TextEnvironment
{
    
}

public sealed class EnvironmentTextList : List<EnvironmentText>
{
    public EnvironmentText GetEnvironment(string Environment)
    {
        return FindLast(n => n.Environment == Environment);
    }
    
    public EnvironmentText[] GetEnvironments(string Environment)
    {
        return  FindAll(n => n.Environment == Environment).ToArray();
    }
    
    public EnvironmentText Get(string text)
    {
        return FindLast(n => n.Org == text);
    }

    public EnvironmentText[] Gets(string org)
    {
        return FindAll(n => n.Org == org).ToArray();
    }

    public EnvironmentText this[string org] => Get(org);
}

public record EnvironmentText(string Org, string Target, string Environment = "");