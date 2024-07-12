using System;
using System.Collections.Generic;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace TheOtherUs;

/*[MonoRegisterAndDontDestroy]*/
[RegisterInIl2Cpp]
public class MainManager : DestroyableSingleton<MainManager>
{
    private readonly ResourceSprite cursorSprite = new("Cursor.png");
    public readonly Dictionary<MainActionsType, Action<MainManager>> MainActions = [];

    public void RegisterAction(MainActionsType type, Action<MainManager> action)
    {
        if (MainActions.ContainsKey(type))
            MainActions[type] = manager => { };

        MainActions[type] += action;
    }

    public void OnEnable()
    {
        Cursor.SetCursor(cursorSprite.GetSprite().texture, Vector2.zero, CursorMode.Auto);
        if (MainActions.TryGetValue(MainActionsType.OnEnable, out var action))
            action.Invoke(this);
    }

    public void LateUpdate()
    {
        if (MainActions.TryGetValue(MainActionsType.LateUpdate, out var action))
            action.Invoke(this);
    }
    
    public enum MainActionsType
    {
        LateUpdate,
        OnEnable
    }
}