using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InitiativeOption is a class that handles
/// the UI responsible for determining if the
/// <c>Player</c> should roll for initiative.
/// </summary>
public class BlockOption : MonoBehaviour
{
    //serialized variable
    public Button NextButton;
    public TextMeshProUGUI rollsLeftText;

    //private variable
    private int _rollBlock;

    public void OnEnable()
    {
        _rollBlock = 0;
        rollsLeftText.text = "(" + Player.Instance().BattleStatus.BlockStatus.BlockCount + " left)";
    }

    public void Update()
    {
        NextButton.interactable = _rollBlock > 0;
    }

    /// <summary>
    /// Checks the <paramref name="value"/> passed through
    /// to determine if the player will roll initiative or not.
    /// </summary>
    /// <param name="value">the number to determine roll iniative. 1 is yes while 2 is no.</param>
    public void OnRollBlock(int value)
    {
        _rollBlock = value;

        switch (value)
        {
            case 1:
                Player.Instance().BattleStatus.BlockStatus.SetRollBlock(true);
                break;
            case 2:
                Player.Instance().BattleStatus.BlockStatus.SetRollBlock(false);
                break;
            default:
                _rollBlock = 0;
                break;
        }
    }
}