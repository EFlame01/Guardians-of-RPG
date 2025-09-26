using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InitiativeOption is a class that handles
/// the UI responsible for determining if the
/// <c>Player</c> should roll for initiative.
/// </summary>
public class InitiativeOption : MonoBehaviour
{
    //serialized variable
    public Button NextButton;

    //private variable
    private int _rollInitiative;

    public void OnEnable()
    {
        _rollInitiative = 0;
    }

    public void Update()
    {
        NextButton.interactable = _rollInitiative > 0;
    }

    /// <summary>
    /// Checks the <paramref name="value"/> passed through
    /// to determine if the player will roll initiative or not.
    /// </summary>
    /// <param name="value">the number to determine roll iniative. 1 is yes while 2 is no.</param>
    public void OnRollInitiative(int value)
    {
        _rollInitiative = value;

        switch (value)
        {
            case 1:
                Player.Instance().BattleStatus.SetRollInitiativeTrue();
                break;
            case 2:
                Player.Instance().BattleStatus.SetRollInitiativeFalse();
                break;
            default:
                _rollInitiative = 0;
                break;
        }
    }
}
