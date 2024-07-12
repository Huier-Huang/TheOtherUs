using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NextPatcher;

public class DirectoryCreator(string Root, params string[] Dirs)
{
    public readonly HashSet<string> Directors = [];
    private readonly List<string> Creates = Dirs.ToList();

    public DirectoryCreator Add(string dir)
    {
        Creates.Add(dir);
        return this;
    }
    
    public void Create()
    {
        foreach (var dir in Creates)
        {
            try
            {
                var current = Root;
                foreach (var d in dir.Replace('\\', '/').Split('/'))
                {
                    current = Path.Combine(current, d);
                    if (Directory.Exists(current)) continue;
                    Directory.CreateDirectory(current);
                    Directors.Add(current);
                }
            }
            catch (Exception e)
            {
                NextPatcher.LogSource.LogError(e);
            }

            Directors.Add(Path.Combine(Root, dir));
        }
    }

    public string Get(string Name)
    {
        return Directors.FirstOrDefault(n => n.EndsWith(Name) || n.EndsWith($"{Name}/")) ?? string.Empty;
    }
}