using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHUD is a class that extends the 
/// <c>CharacterHUD</c> class. This class
/// is responsible for displaying the battle
/// information for the <c>Player</c> during
/// battle.
/// </summary>
public class PlayerHUD : CharacterHUD
{
    //serialized variable
    [SerializeField] public SliderBar XpBar;

    public override void InitializeCharacterHUD(Character character)
    {
        base.InitializeCharacterHUD(Player.Instance());
        XpBar.SetValue(Player.Instance().CurrXP, Player.Instance().LimXP);
    }

    /// <summary>
    /// Updates the XPBar of the <c>PlayerHUD</c>
    /// based on the <c>Player</c>'s information.
    /// </summary>
    public void UpdateXPBar()
    {
        CharacterLevelText.text = Player.Instance().Level.ToString();
        StartCoroutine(XpBar.ChangeValue(Player.Instance().CurrXP, Player.Instance().LimXP, true));
    }
}
