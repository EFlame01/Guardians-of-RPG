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
    public bool IsInitialized { get; private set; }

    /// <summary>
    /// Takes the <paramref name="character"/> and 
    /// sets up the <c>CharacterHUD</c> UI based on 
    /// their information.
    /// </summary>
    /// <param name="character">the character in question</param>
    public virtual void InitializeCharacterHUD(Character character)
    {
        if (character == null)
        {
            Debug.LogWarning("WARNING: character being initialized is NULL.");
            IsInitialized = false;
            return;
        }
        CharacterNameText.text = character.Name;
        CharacterLevelText.text = character.Level.ToString();
        HpBar.SetValue(character.BaseStats.Hp, character.BaseStats.FullHp);
        EpBar.SetValue(character.BaseStats.Elx, character.BaseStats.GetFullElx());
        IsInitialized = true;
    }

    /// <summary>
    /// Takes the <paramref name="character"/> and 
    /// updates the <c>CharacterHUD</c> UI based on 
    /// their information. 
    /// </summary>
    /// <param name="character">the character in question</param>
    public virtual void UpdateHUD(Character character)
    {
        if (character == null)
        {
            Debug.LogWarning("WARNING: character being updated is NULL.");
            return;
        }
        StartCoroutine(HpBar.ChangeValue(character.BaseStats.Hp, character.BaseStats.FullHp, true));
        StartCoroutine(EpBar.ChangeValue(character.BaseStats.Elx, character.BaseStats.GetFullElx(), true));
    }

    /// <summary>
    /// AddStatussymbol adds a status symbol to the 
    /// <c>CharacterHUD</c> based on the status condition
    /// that the character has. It will not add a 
    /// status symbol if it is already present in the
    /// CharacterHUD
    /// </summary>
    /// <param name="symbol">The symbol that will be added to the <c>CharacterHUD</c></param>
    public void AddStatusSymbol(GameObject symbol)
    {
        if (!IsInitialized)
        {
            Debug.LogWarning("WARNING: the CharacterHUD you wish to add a status symbol to has not been initialized.");
            return;
        }
        if (symbol == null)
        {
            Debug.LogWarning("WARNING: the status symbol you wish to add to the character is NULL.");
            return;
        }
        foreach (Transform child in StatusConditionLayout)
        {
            if (child.gameObject.name.Contains(symbol.name))
                return;
        }
        Instantiate(symbol, StatusConditionLayout);
    }
}