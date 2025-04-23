using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollInitiativeState : MonoBehaviour
{
    Animator rollAnimator;

    public IEnumerator RollInitiative()
    {
        int number = GetInitiativeRollNumber();
        rollAnimator.Play(number.ToString());
        yield return null;
    }

    private int GetInitiativeRollNumber()
    {
        return 0;   
    }
}
