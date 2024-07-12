namespace NextPatcher;

public class NextScriptManager
{
    public string FindDir { get; set; }

    public NextScriptManager SetFindDir(string Dir)
    {
        FindDir = Dir;
        return this;
    }


    public void BuildAll()
    {
    }
}