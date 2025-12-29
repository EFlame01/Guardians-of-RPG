using UnityEngine;

public class FireSprite : ObjectSprite
{
    public string fireAnimation;
    public override void Start()
    {
        _animator = GetComponent<Animator>();
        FireAnimation(fireAnimation);
    }
}