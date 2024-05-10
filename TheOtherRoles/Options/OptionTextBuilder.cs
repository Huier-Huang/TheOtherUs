using System.Collections;

namespace TheOtherRoles.Options;

public class OptionTextBuilder(ICollection options, BuildRule rule)
{
    public OptionTextBuilder(ICollection options) : this(options, BuildRule.DefRule) { }
    
    private int page = options.Count / 50;

    private BuildRule Rule = rule;
    
    public OptionTextBuilder SetBuildPage(int index)
    {
        page = index;
        return this;
    }
    
    public string BuildAll()
    {
        return string.Empty;
    }

    public string Build(OptionTypes type)
    {
        return string.Empty;
    }
}

public class BuildRule
{
    public static BuildRule DefRule;
}