using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class WildGrass : MonoBehaviour
{
    [SerializeField] public Transform playerTransform;
    [SerializeField] public Animator animator;
    [SerializeField] public LayerMask grassLayer;
    private bool resetAnimation;

    public void Update()
    {
        UpdateGrassAnimation();
    }

    public void UpdateGrassAnimation()
    {
        if(InGrass())
        {
            resetAnimation = true;
            if(InGrass() && GameManager.Instance.PlayerState.Equals(PlayerState.MOVING))
            animator.Play("grass");
            else if(InGrass() && !GameManager.Instance.PlayerState.Equals(PlayerState.MOVING))
                animator.Play("none");
        }
        else
        {
            if(resetAnimation)
            {
                resetAnimation = false;
                animator.Play("none");
            }
        }
    }

    private bool InGrass()
    {
        try
        {
            return Physics2D.OverlapCircle(playerTransform.position, 0.2f, grassLayer) != null;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
