using System.Collections.Generic;

namespace TheOtherUs.Chat;

public class EnvironmentTextManager : ManagerBase<EnvironmentTextManager>
{
    private readonly EnvironmentTextList textList = [];
    public IReadOnlyList<EnvironmentText> List => textList;
    public string CurrentEnvironment { get; set; } = string.Empty;
    

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