using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleCharacter is a class that portrays the <c>Character</c>
/// in the <c>BattleSimulator</c>. This includes the sprite, platform, 
/// and the <c>CharacterHUD</c>.
/// </summary>
public class BattleCharacter : MonoBehaviour
{
    //serialized variables
    public Character Character;
    public RuntimeAnimatorController RuntimeAnimatorController;
    public string AnimationPosition;
    public MoveEffects MoveEffects;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected SpriteRenderer CharacterSprite;
    [SerializeField] protected SpriteRenderer PlatformSprite;
    public CharacterHUD CharacterHUD;

    /// <summary>
    /// Sets up the <c>CharacterHUD</c> and the position
    /// of the <c>Character</c> based on the information inside
    /// the <c>BattleCharacter</c> instance.
    /// </summary>
    public void InitializeBattleCharacter()
    {
        CharacterHUD.InitializeCharacterHUD(Character);
        SetAnimation();
    }

    /// <summary>
    /// Updates the <c>CharacterHUD</c> based on the
    /// <c>Character</c>'s <c>BattleStatus</c> inside the
    /// <c>BattleCharacter</c> instance.
    /// </summary>
    public void UpdateHUD()
    {
        CharacterHUD.UpdateHUD(Character);
    }

    /// <summary>
    /// Enables or disables the <c>CharacterHUD</c> based
    /// on the <paramref name="enable"/> variable.
    /// </summary>
    /// <param name="enable">enable or disable <c>CharacterHUD</c></param>
    public void EnableHUD(bool enable)
    {
        CharacterHUD?.gameObject.SetActive(enable);
    }

    private void SetAnimation()
    {
        try
        {
            Animator.runtimeAnimatorController = RuntimeAnimatorController;
            Animator.Play(AnimationPosition);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error in playing animation " + e.Message);
        }
    }
}