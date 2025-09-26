using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// CharacterHUD is a class that is responsible
/// for displaying the battle information for
/// each <c>Character</c> during battle.
/// </summary>
public class CharacterHUD : MonoBehaviour
{
    //serialized variables
    public TextMeshProUGUI CharacterNameText;
    public TextMeshProUGUI CharacterLevelText;
    public SliderBar HpBar;
    public SliderBar EpBar;
    public Transform StatusConditionLayout;

    /// <summary>
    /// Takes the <paramref name="character"/> and 
    /// sets up the <c>CharacterHUD</c> UI based on 
    /// their information.
    /// </summary>
    /// <param name="character">the character in question</param>
    public virtual void InitializeCharacterHUD(Character character)
    {
        CharacterNameText.text = character.Name;
        CharacterLevelText.text = character.Level.ToString();
        HpBar.SetValue(character.BaseStats.Hp, character.BaseStats.FullHp);
        EpBar.SetValue(character.BaseStats.Elx, character.BaseStats.GetFullElx());
    }

    /// <summary>
    /// Takes the <paramref name="character"/> and 
    /// updates the <c>CharacterHUD</c> UI based on 
    /// their information. 
    /// </summary>
    /// <param name="character">the character in question</param>
    public virtual void UpdateHUD(Character character)
    {
        StartCoroutine(HpBar.ChangeValue(character.BaseStats.Hp, character.BaseStats.FullHp, true));
        StartCoroutine(EpBar.ChangeValue(character.BaseStats.Elx, character.BaseStats.GetFullElx(), true));
    }

    public void AddStatusSymbol(GameObject symbol)
    {
        GameObject.Instantiate(symbol, StatusConditionLayout);
    }
}