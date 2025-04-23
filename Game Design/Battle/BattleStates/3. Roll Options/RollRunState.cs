using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollRunState : MonoBehaviour
{
    Animator rollAnimator;

    public IEnumerator RollRun()
    {
        int number = GetRunRollNumber();
        rollAnimator.Play(number.ToString());
        yield return null;
    }

    private int GetRunRollNumber()
    {
        return 0;   
    }
}
