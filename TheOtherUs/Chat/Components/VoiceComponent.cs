using System;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace TheOtherUs.Chat.Components;

[MonoRegisterAndDontDestroy]
public class VoiceComponent : MonoBehaviour
{
    private readonly VoiceManager _manager = VoiceManager.Instance;
    
    public VoiceComponent()
    {
        if (_manager.Current != null)
            _manager.Current.Destroy();
        
        _manager.Current = this;
    }

    internal bool Started;
    public void Start()
    {
        foreach (var client in _manager.ActiveClients)
        {
            client.Register(this, _manager);
            client.OnStart();
        }

        Started = true;
    }

    public void LateUpdate()
    {
        foreach (var client in _manager.ActiveClients)
        {
            client.OnUpdate();
        }
    }
}