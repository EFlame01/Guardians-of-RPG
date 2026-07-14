using System.Collections;
using UnityEngine;

public class RollMechanic : MonoBehaviour
{
    //public methods
    [SerializeField] public Animator RollAnimator;
    public int RollNumber { get; private set; }

    //private  methods
    private static readonly WaitForSeconds _waitForSeconds1_25 = new(1.25f);

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
        if (RollAnimator.HasState(0, Animator.StringToHash(RollNumber.ToString())))
            RollAnimator.Play(RollNumber.ToString());
        else
            Debug.LogWarning("WARNING: Animation " + RollNumber.ToString() + " could not be played...");
        yield return _waitForSeconds1_25;
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