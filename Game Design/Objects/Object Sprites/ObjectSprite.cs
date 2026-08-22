using System;
using UnityEngine;

/// <summary>
/// ObjectSprite is a class that controls the
/// animation of certain objects in the game.
/// </summary>
[RequireComponent(typeof(Animator))]
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

        if (!string.IsNullOrEmpty(_startAnimation))
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
}