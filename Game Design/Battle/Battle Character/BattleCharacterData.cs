using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BattleCharacterData
{
    public bool IsPlayer;
    public string CharacterData;
    public Sprite CharacterSprite;
    public Sprite CharacterSprite2;
    public int NPCLevel;
    public string CharacterAnimationPosition;
    public RuntimeAnimatorController CharacterAnimator;

    public string GetPlayerAnimationPosition()
    {
        CharacterAnimationPosition = Player.Instance().MaleOrFemale().Equals("MALE") ? "adam_idle_right" : "eve_idle_right";
        return CharacterAnimationPosition;
    }

    public Sprite GetPlayerSprite()
    {
        return Player.Instance().MaleOrFemale().Equals("MALE") ? CharacterSprite : CharacterSprite2;
    }
}