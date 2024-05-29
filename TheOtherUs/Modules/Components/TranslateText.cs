using Reactor.Utilities.Attributes;
using TMPro;

namespace TheOtherUs.Modules.Components;

[RegisterInIl2Cpp]
public sealed class TranslateText : TextMeshPro
{
    public string DefText { get; set; }

    public string Id { get; set; } = string.Empty;
    
    public bool Update { get; set; } = false;
    
    public override void Awake()
    {
        base.Awake();
        DefText = text;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (Id == string.Empty)
        {
            text = DefText;
            return;
        }
        text = Id.Translate();
    }

    public void LateUpdate()
    {
        if (Id == string.Empty)
        {
            text = DefText;
            return;
        }
        if (Update) text = Id.Translate();
    }
}