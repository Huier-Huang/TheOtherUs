#nullable enable
using System;
using System.Collections.Generic;

namespace TheOtherUs.Modules;

public class ManagerBase<T> : IDisposable where T : ManagerBase<T> , new()
{
    private static T? _instance;

    public static T Instance => _instance ??= new T();
    private static readonly List<ManagerBase<T>> _managers = [];
    public static IReadOnlyList<ManagerBase<T>> AllManager => _managers;
    
    public ManagerBase()
    {
        _managers.Add(this);
        _instance = (T)this;
    }

    public virtual void Dispose()
    {
    }
}