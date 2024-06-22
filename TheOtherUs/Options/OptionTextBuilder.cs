using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TheOtherUs.Options;

public class OptionTextBuilder(ICollection options, BuildRule rule)
{
    public OptionTextBuilder(ICollection options) : this(options, BuildRule.DefRule) { }

    private int StartPage = -1;
    private int EndPage = -1;
    private readonly List<string> PageTexts = [];
    private int PageCount = 0;
    private int page = options.Count / 50;

    private BuildRule Rule = rule;

    private string ParentText = string.Empty;

    private StringBuilder _stringBuilder = new();

    public OptionTextBuilder SetParentText(string text)
    {
        ParentText = text;
        return this;
    }
    
    public OptionTextBuilder SetBuildPage(int pageCount, int start, int end)
    {
        PageCount = pageCount;
        StartPage = start;
        EndPage = end;
        return this;
    }

    public OptionTextBuilder SetOptions(ICollection option)
    {
        options = option;
        return this;
    }
    
    public OptionTextBuilder BuildAll()
    {
        PageTexts.Clear();
        return this;
    }

    public OptionTextBuilder Build(CustomOptionTypes type)
    {
        PageTexts.Clear();
        return this;
    }

    public string GetPageText(int pageIndex)
    {
        return PageTexts[pageIndex] ?? string.Empty;
    }
    
    public string GetAllText()
    {
        return string.Empty;
    }

    public static implicit operator string(OptionTextBuilder builder) => builder.GetAllText();

    public string this[int index] => GetPageText(index);
}

public class BuildRule
{
    public static BuildRule DefRule;
}