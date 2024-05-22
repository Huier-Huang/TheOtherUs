using System.Collections;

namespace TheOtherUs.Options;

public class OptionTextBuilder(ICollection options, BuildRule rule)
{
    public OptionTextBuilder(ICollection options) : this(options, BuildRule.DefRule) { }
    
    private int page = options.Count / 50;

    private BuildRule Rule = rule;

    private string ParentText = string.Empty;

    public OptionTextBuilder SetParentText(string text)
    {
        ParentText = text;
        return this;
    }
    
    public OptionTextBuilder SetBuildPage(int index)
    {
        page = index;
        return this;
    }

    public OptionTextBuilder SetOptions(ICollection option)
    {
        options = option;
        return this;
    }
    
    public OptionTextBuilder BuildAll()
    {
        return this;
    }

    public OptionTextBuilder Build(OptionTypes type)
    {
        return this;
    }

    public string GetPageText(int page)
    {
        return string.Empty;
    }
    
    public string GetAllText()
    {
        return string.Empty;
    }
}

public class BuildRule
{
    public static BuildRule DefRule;
}