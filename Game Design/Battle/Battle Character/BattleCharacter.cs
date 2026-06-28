using System;
using UnityEngine;

/// <summary>
/// BattleCharacter is a class that portrays the <c>Character</c>
/// in the <c>BattleSimulator</c>. This includes the sprite, platform, 
/// and the <c>CharacterHUD</c>.
/// </summary>
public class BattleCharacter : MonoBehaviour
{
    //Serialized variables
    public Character Character;

    [Header("Sprite and Animation Details")]
    public RuntimeAnimatorController RuntimeAnimatorController;
    public string AnimationPosition;
    public MoveEffects MoveEffects;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected SpriteRenderer CharacterSprite;
    [SerializeField] protected SpriteRenderer PlatformSprite;
    public CharacterHUD CharacterHUD;

    [Header("Move Details")]
    public string MoveHitWith;

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
        if (CharacterHUD != null)
            CharacterHUD.gameObject.SetActive(enable);
    }

    /// <summary>
    /// Sets the <c>RuntimeAnimatorController</c> of 
    /// the <c>BattleCharacter</c> and plays the right
    /// animation based on their position.
    /// </summary>
    private void SetAnimation()
    {
        try
        {
            Animator.runtimeAnimatorController = RuntimeAnimatorController;
            Animator.Play(AnimationPosition);
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARNING in playing animation " + e.Message);
        }
    }
}