using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSprite : MonoBehaviour
{
    [SerializeField] public string CharacterName;
    [SerializeField] public SpriteRenderer image;
    [SerializeField] public Sprite maleSprite;
    [SerializeField] public Sprite femaleSprite;
    [SerializeField] public Animator Animator;

    public void Start()
    {
        if(CharacterName.Equals("player"))
            CharacterName = Player.Instance().MaleOrFemale().Equals("MALE") ? "adam" : "eve";
        if(gameObject.tag.Equals("Player"))
        {
            PerformIdleAnimation(PlayerDirection.UP);
            image.sprite = Player.Instance().MaleOrFemale().Equals("MALE") ? maleSprite : femaleSprite;
        }
    }

    public void PerformWalkAnimation(string walkDirection)
    {
        if(Animator.runtimeAnimatorController == null)
            return;
        
        if(walkDirection.Equals("none"))
            return;
        
        Animator.Play(CharacterName + "_" + walkDirection);
    }

    public void PerformIdleAnimation(PlayerDirection playerDirection)
    {
        if(Animator.runtimeAnimatorController == null)
            return;
        
        switch(playerDirection)
        {
            case PlayerDirection.UP:
                Animator.Play(CharacterName + "_idle_up");
                break;
            case PlayerDirection.DOWN:
                Animator.Play(CharacterName + "_idle_down");
                break;
            case PlayerDirection.LEFT:
                Animator.Play(CharacterName + "_idle_left");
                break;
            case PlayerDirection.RIGHT:
                Animator.Play(CharacterName + "_idle_right");
                break;
            default:
                Animator.Play(CharacterName + "_idle_down");
                break;
        }
    }
}
