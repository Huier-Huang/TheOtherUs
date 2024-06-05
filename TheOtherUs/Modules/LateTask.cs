using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Unity.IL2CPP.Utils;
using UnityEngine;

namespace TheOtherUs.Modules;

#nullable enable
public class LateTask(float time, Action? action = null, IEnumerator? enumerator = null, Action? onEnd = null)
{
    public float Time { get; set; } = time;
    public Action? Action { get; set; }
    public IEnumerator? Enumerator { get; set; } = enumerator;

    public Action? OnEnd { get; set; } = onEnd;

    public static readonly List<LateTask> _lateTasks = [];
    
    public Coroutine? coroutine { get; private set; }

    private void Update(LateTaskUpdate lateTaskUpdate)
    {
        try
        {
            action?.Invoke();
        }
        catch (Exception e)
        {
            Exception(e);
        }

        if (Enumerator == null) return;
        try
        {
            coroutine = lateTaskUpdate.StartCoroutine(Enumerator);
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }

    public void Register()
    {
        _lateTasks.Add(this);
    }

    private void UnRegister()
    {
        _lateTasks.Remove(this);
        OnEnd?.Invoke();
    }
    
    [MonoRegisterAndDontDestroy]
    internal sealed class LateTaskUpdate : MonoBehaviour
    {
        public void LateUpdate()
        {
            foreach (var task in _lateTasks)
            {
                task.Time -= 0.1f;

                if (!(task.Time <= 0)) continue;
                task.Update(this);
                task.UnRegister();
            }
        }
    } 
}