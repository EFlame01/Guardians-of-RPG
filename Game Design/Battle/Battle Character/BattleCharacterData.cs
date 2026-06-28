using UnityEngine;
using System;

/// <summary>
/// BattleCharacterData is the data of the
/// BattleCharacter needed inside of the
/// BattleSimulator
/// </summary>
[Serializable]
public class BattleCharacterData
{
    //public/serialized variables
    public bool IsPlayer;
    public string CharacterData;
    public Sprite CharacterSprite;
    public Sprite CharacterSprite2;
    public int NPCLevel;
    public string CharacterAnimationPosition;
    public RuntimeAnimatorController CharacterAnimator;

    /// <summary>
    /// Determines the idle animation for the
    /// <c>Player</c> in the <c>BattleSimulator</c>.
    /// </summary>
    /// <returns>The name of the animation in the form of a string.</returns>
    public string GetPlayerAnimationPosition()
    {
        CharacterAnimationPosition = Player.Instance().MaleOrFemale().Equals("MALE") ? "adam_idle_right" : "eve_idle_right";
        return CharacterAnimationPosition;
    }

    /// <summary>
    /// Determines the sprite for the <c>Player</c>
    /// in the <c>BattleSimulator</c>.
    /// </summary>
    /// <returns>A male or female sprite</returns>
    public Sprite GetPlayerSprite()
    {
        return Player.Instance().MaleOrFemale().Equals("MALE") ? CharacterSprite : CharacterSprite2;
    }
}