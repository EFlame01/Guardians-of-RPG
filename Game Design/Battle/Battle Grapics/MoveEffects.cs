using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

/// <summary>
/// MoveEffects is a class that is responsible
/// for displaying the animations of the actions
/// in the game.
/// </summary>
public class MoveEffects : MonoBehaviour
{
    //serialized variables
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Shader FlashShader;
    [SerializeField] private Animator ActionAnimator;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Camera MainCamera; 

    /// <summary>
    /// This coroutine starts the animation for both
    /// the <c>actionAnimator</c> as well as the 
    /// <c>cameraAnimator</c> based on the 
    /// <paramref name="nameOfAnimation"/> and the 
    /// <paramref name="offset"/>.
    /// </summary>
    /// <param name="nameOfAnimation">name of the animation</param>
    /// <param name="offset">type of camera offset the camera should use</param>
    /// <returns></returns>
    public IEnumerator ActionAnimationRoutine(string nameOfAnimation, string offset)
    {
        string animationName = nameOfAnimation.ToLower().Replace(" ", "_");
        int oldSize = (int)MainCamera.orthographicSize;
        int newSize = (int)((float)oldSize * 0.667f);
        float t = 0f;
        bool animationPerformed = false;
        
        if(ActionAnimator.HasState(0, Animator.StringToHash(animationName)) && cameraAnimator.HasState(0, Animator.StringToHash(animationName + "_" + offset)))
        {
            ActionAnimator.Play(animationName);
            cameraAnimator.Play(animationName + "_" + offset);
            animationPerformed  = true;
        }
        else
        {
            Debug.LogWarning("Animation could not be played...");
            switch(MoveMaker.Instance.GetMoveBasedOnName(nameOfAnimation).Type)
            {
                case MoveType.KNOCK_OUT:
                case MoveType.PRIORITY:
                case MoveType.REGULAR:
                    ActionAnimator.Play("base_attack");
                    cameraAnimator.Play("base_attack_" + offset);
                    animationPerformed  = true;
                    break;
                default:
                    animationPerformed  = false;
                    break;
            }
        }

        if(animationPerformed)
        {
            while(t < ActionAnimator.GetCurrentAnimatorStateInfo(0).length)
            {
                t += Time.fixedDeltaTime;
                yield return null;
            }
            
            // actionAnimator.Play("none");

            while(t < cameraAnimator.GetCurrentAnimatorStateInfo(0).length)
            {
                t += Time.fixedDeltaTime;
                yield return null;
            }

            // cameraAnimator.Play("none");
        }
    }

    /// <summary>
    /// This coroutine takes the <c>flashShader</c> and
    /// performs a series of flashes over the
    /// <c>spriteRenderer</c>.
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashRoutine()
    {
        Shader originalShader = SpriteRenderer.material.shader;
        SpriteRenderer.material.shader = FlashShader;
        
        SpriteRenderer.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(0.05f);
        SpriteRenderer.material.SetInt("_Flash", 0);
        yield return new WaitForSeconds(0.1f);

        SpriteRenderer.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(0.05f);
        SpriteRenderer.material.SetInt("_Flash", 0);
        yield return new WaitForSeconds(0.05f);

        SpriteRenderer.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(0.05f);
        SpriteRenderer.material.SetInt("_Flash", 0);
        yield return new WaitForSeconds(0.05f);

        SpriteRenderer.material.shader = originalShader;
    }

    public IEnumerator HealRoutine()
    {
        yield return null;
    }

    public IEnumerator StatRoutine()
    {
        yield return null;
    }

    public IEnumerator StatusConditionRoutine()
    {
        yield return null;
    }

    /// <summary>
    /// Dead Method. Zooms Camera in an out.
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns></returns>
    private IEnumerator CameraZoomRoutine(int newSize)
    {
        float duration = 1f;
        float time = 0f;
        while(time < duration)
        {
            float t = time/duration;
            MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, newSize, t);
            time += Time.fixedDeltaTime;
            yield return null;
        }
    }

}
