using UnityEngine;
using System;

public class FireSprite : ObjectSprite
{
    public string fireAnimation;
    public override void Start()
    {
        base.Start();
        FireAnimation();
    }

    /// <summary>
    /// This is used to play animations that ARE ON FIRE.
    /// This includes fire of any color.
    /// </summary>
    public void FireAnimation()
    {
        if (_animator == null)
            return;
        if (string.IsNullOrEmpty(fireAnimation))
            return;

        try
        {
            _animator.Play(_objectID + fireAnimation);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"WARNING: {e.Message} \n {_objectID}{fireAnimation} not found...");
        }
    }
}