#nullable enable
using System;
using System.Collections.Generic;

namespace TheOtherUs.Modules;

public interface IManagerBase;

public static class MangerBases
{
    private static readonly List<IManagerBase> _managers = [];
    public static IReadOnlyList<IManagerBase> AllManager => _managers;
    public static void Add(IManagerBase @base) => _managers.Add(@base);
}
public abstract class ManagerBase<T> : IDisposable, IManagerBase where T : ManagerBase<T> , new()
{
    private static T? _instance;

    public static T Instance => _instance ??= new T();
    
    public ManagerBase()
    {
        MangerBases.Add(this);
        _instance = (T)this;
    }

    public virtual void Dispose()
    {
    }
}

public class ListManager<T, TList> : ManagerBase<T>, IDisposable where T : ListManager<T, TList> , new()
{
    protected List<TList> List = [];
    public IReadOnlyList<TList> ReadOnlyList => List;
}