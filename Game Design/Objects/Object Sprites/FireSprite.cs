using UnityEngine;

public class FireSprite : ObjectSprite
{
    public override void Start()
    {
        FireAnimation(_objectID);
    }
}