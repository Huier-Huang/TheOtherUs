using UnityEngine;

namespace TheOtherUs;

[MonoRegisterAndDontDestroy]
public class MainManager : MonoBehaviour
{
    public MainManager Instance => MonoRegisterAndDontDestroy.GetRegister<MainManager>();
    private readonly ResourceSprite cursorSprite = new("Cursor.png");

    public void OnEnable()
    {
        Cursor.SetCursor(cursorSprite.GetSprite().texture, Vector2.zero, CursorMode.Auto);
    }
}