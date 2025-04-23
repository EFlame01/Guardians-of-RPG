using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSprite : MonoBehaviour
{
    [SerializeField] private string _objectID;
    [SerializeField] private string _startAnimation;
    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
        if(_startAnimation != null)
            _animator.Play(_objectID + "_" + _startAnimation);
    }

    public void OpenAnimation()
    {
        if(_animator == null)
            return;

        _animator.Play(_objectID + "_open");
    }

    public void CloseAnimation()
    {
        if(_animator == null)
            return;

        _animator.Play(_objectID + "_close");
    }
}