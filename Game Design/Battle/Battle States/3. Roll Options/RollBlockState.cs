using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// RollBlockState is a class that extends
/// the <c>BattleState</c> class. This state asks
/// if you want to roll to block. Blocking
/// will give the opportunity to decrease
/// the amount of damage to physical attacks
/// inflicted on the <c>Player</c>
/// </summary>
public class RollBlockState : MonoBehaviour
{
    TextMeshProUGUI QuestionText;
    Animator rollAnimator;

    public IEnumerator AskForBlock()
    {

        yield return null;
    }

    public IEnumerator RollBlock()
    {
        int number = GetBlockRollNumber();
        rollAnimator.Play(number.ToString());
        yield return null;
    }

    private int GetBlockRollNumber()
    {
        return 0;   
    }

    public void OnBlockButtonPressed(int val)
    {
        switch(val)
        {
            case 0:
                //not blocking
                break;
            case 1:
                //blocking
                break;
            default:
                //not blocking
                break;
        }
    }

}
