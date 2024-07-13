namespace NextPatcher;

public class NextScriptManager(string findDir)
{
    public string FindDir { get; set; } = findDir;

    public NextScriptManager SetFindDir(string Dir)
    {
        FindDir = Dir;
        return this;
    }


    public NextScriptManager BuildAll()
    {
        return this;
    }

    public NextScriptManager Run(INextScriptInfo info)
    {
        return this;
    }
}