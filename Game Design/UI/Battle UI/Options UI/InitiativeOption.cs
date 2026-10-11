using TMPro;
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
    public TextMeshProUGUI rollsLeftText;

    //private variable
    private int _rollInitiative;

    public void OnEnable()
    {
        _rollInitiative = 0;
        rollsLeftText.text = "(" + Player.Instance().BattleStatus.InitiativeStatus.InitiativeCount + " left)";
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
                Player.Instance().BattleStatus.InitiativeStatus.SetRollInitiative(true);
                Player.Instance().BattleStatus.SetTurnStatus(TurnStatus.INITIATIVE);
                break;
            case 2:
                Player.Instance().BattleStatus.InitiativeStatus.SetRollInitiative(false);
                break;
            default:
                _rollInitiative = 0;
                break;
        }
    }
}
