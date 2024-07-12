using UnityEngine;

namespace TheOtherUs.Objects;

public class Arrow
{
    private static readonly ResourceSprite ArrowSprite = new ("Arrow.png", 200f);
    public GameObject arrow;
    private readonly ArrowBehaviour arrowBehaviour;
    public SpriteRenderer image;
    private Vector3 oldTarget;
    public float perc = 0.925f;


    public Arrow(Color color)
    {
        arrow = new GameObject("Arrow")
        {
            layer = 5
        };
        image = arrow.AddComponent<SpriteRenderer>();
        image.sprite = ArrowSprite;
        image.color = color;
        arrowBehaviour = arrow.AddComponent<ArrowBehaviour>();
        arrowBehaviour.image = image;
    }
    

    public void Update()
    {
        var target = oldTarget;
        Update(target);
    }

    public void Update(Vector3 target, Color? color = null)
    {
        if (arrow == null) return;
        oldTarget = target;

        if (color.HasValue) image.color = color.Value;

        arrowBehaviour.target = target;
        arrowBehaviour.Update();
    }
}