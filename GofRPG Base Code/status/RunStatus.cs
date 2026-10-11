using UnityEngine;

/// <summary>
/// InitiativeStatus is a class that determines the 
/// <c>Character</c>'s ability to roll to run
/// (the ability to escape battle).
/// </summary>
public class RunStatus
{
    //public variables
    public bool CanEscape { get; private set; }
    public bool RollRun { get; private set; }
    public int RollValue { get; private set; }

    //private const variables
    private const int MIN_ROLL_VALUE = 20;
    private const int MAX_ROLL_VALUE = 20;

    //Constructor
    public RunStatus()
    {
        CanEscape = true;
        RollRun = false;
        RollValue = 0;
    }

    //Setter Methods
    public void SetCanEscape(bool value)
    {
        CanEscape = value;
    }
    public void SetRollRun(bool value)
    {
        RollRun = value;
    }
    public void SetRollValue(int value)
    {
        RollValue = Mathf.Clamp(value, MIN_ROLL_VALUE, MAX_ROLL_VALUE);
    }
}