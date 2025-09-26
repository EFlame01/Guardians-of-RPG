using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class WildGrass : MonoBehaviour
{
    public Transform playerTransform;
    public Animator animator;
    public LayerMask grassLayer;
    private bool resetAnimation;

    public void Update()
    {
        UpdateGrassAnimation();
    }

    public void UpdateGrassAnimation()
    {
        if (InGrass())
        {
            resetAnimation = true;
            if (InGrass() && GameManager.Instance.PlayerState.Equals(PlayerState.MOVING))
                animator.Play("grass");
            else if (InGrass() && !GameManager.Instance.PlayerState.Equals(PlayerState.MOVING))
                animator.Play("none");
        }
        else
        {
            if (resetAnimation)
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
            if (playerTransform != null)
                return Physics2D.OverlapCircle(playerTransform.position, 0.2f, grassLayer) != null;
            return false;
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARNING: " + e.Message);
            return false;
        }
    }
}
