using System;
using TheOtherUs.Chat.Components;

namespace TheOtherUs.Chat;

public class VoiceClient : IDisposable
{
    public byte PlayerId { get; set; }
    public bool active { get; set; }

    private VoiceComponent _component;
    private VoiceManager _voiceManager;

    public void Register(VoiceComponent component, VoiceManager voiceManager)
    {
        this._component = component;
        this._voiceManager = voiceManager;
    }

    public void OnUpdate()
    {
        
    }

    public void OnStart()
    {
        
    }

    public void Dispose()
    {
        VoiceManager.Instance.Remove(this);
    }
}