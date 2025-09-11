using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMechanic : MonoBehaviour
{
    public Animator RollAnimator;
    public int RollNumber {get; private set;}

    public IEnumerator RollRun()
    {
        RollNumber = GetRunRollNumber();
        yield return PlayRollAnimation();
    }

    public IEnumerator RollInitiative()
    {
        RollNumber = GetInitiativeRollNumber();
        yield return PlayRollAnimation();
    }

    public IEnumerator RollBlock()
    {
        RollNumber = GetBlockRollNumber();
        yield return PlayRollAnimation();
    }

    public IEnumerator PlayRollAnimation()
    {
        RollAnimator.Play(RollNumber.ToString());
        yield return new WaitForSeconds(1.25f);
    }

    private int GetRunRollNumber()
    {
        //TODO: configure a better roll mechanic 
        return Random.Range(0, 20) + 1;   
    }

    private int GetInitiativeRollNumber()
    {
    //TODO: configure a better roll mechanic 
        return Random.Range(0, 20) + 1;
    }

    public int GetBlockRollNumber()
    {
        //TODO: configure a better roll mechanic 
        return Random.Range(0, 20) + 1;
    }
}