using UnityEngine;

/// <summary>
/// ObjectSprite is a class that controls the
/// animation of certain objects in the game.
/// </summary>
public class ObjectSprite : MonoBehaviour
{
    //Serialized variables
    [SerializeField] protected string _objectID;
    [SerializeField] private string _startAnimation;

    //private variables
    protected Animator _animator;

    public virtual void Start()
    {
        _animator = GetComponent<Animator>();
        if (_startAnimation != null)
            _animator.Play(_objectID + "_" + _startAnimation);
    }

    /// <summary>
    /// This is used to play animations that OPENS
    /// things. This includes doors, chests, and menus.
    /// </summary>
    public void OpenAnimation()
    {
        if (_animator == null)
            return;

        _animator.Play(_objectID + "_open");
    }

    /// <summary>
    /// This is used to play animations that CLOSES 
    /// things. This includes doors, chests, and menus.
    /// </summary>
    public void CloseAnimation()
    {
        if (_animator == null)
            return;

        _animator.Play(_objectID + "_close");
    }

    /// <summary>
    /// This is used to play animations that ARE ON FIRE.
    /// This includes fire of any color.
    /// </summary>
    public void FireAnimation(string fireAnimation)
    {
        if (_animator == null)
            return;

        _animator.Play(_objectID + fireAnimation);

        // switch (fireAnimation)
        // {
        //     case "_fire":
        //         _animator.Play(_objectID + "_fire");
        //         break;
        //     case "_smoke":
        //         _animator.Play(_objectID + "_smoke");
        //         break;
        //     case "_none":
        //         _animator.Play(_objectID + "_none");
        //         break;
        //     default:
        //         _animator.Play(_objectID + "_fire");
        //         break;
        // }
    }
}