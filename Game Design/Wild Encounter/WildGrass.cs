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

    public void Update()
    {
        if(GameManager.Instance.PlayerState.Equals(PlayerState.MOVING))
            UpdateGrassAnimation();
    }

    public void UpdateGrassAnimation()
    {
        if(InGrass() && GameManager.Instance.PlayerState.Equals(PlayerState.MOVING))
            animator.Play("grass");
        else
            animator.Play("none");
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
