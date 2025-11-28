using UnityEngine;

public class FireSprite : ObjectSprite
{
    public string fireAnimation;
    public override void Start()
    {
        FireAnimation(fireAnimation);
    }
}