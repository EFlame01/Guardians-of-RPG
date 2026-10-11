using UnityEngine;

/// <summary>
/// InitiativeStatus is a class that determines the 
/// <c>Character</c>'s ability to roll for Initiative
/// (the ability to move first for a round in battle).
/// </summary>
public class InitiativeStatus
{
    //public variables
    public bool RollInitiative { get; private set; }
    public int InitiativeCount { get; private set; }
    public TurnStatus InitiativeLevel { get; private set; }
    public int InitiativeValue { get; private set; }
    public bool CheckRollValue { get; private set; }

    //private const variables
    private const int MAX_INITIATIVE_COUNT = 3;
    private const int MAX_ROLL_VALUE = 20;
    private const int MIN_ROLL_VALUE = 1;
    private const int DEFAULT_ROLL_VALUE = 0;

    //Constructor
    public InitiativeStatus()
    {
        RollInitiative = false;
        InitiativeCount = MAX_INITIATIVE_COUNT;
        InitiativeLevel = TurnStatus.NOTHING;
        InitiativeValue = DEFAULT_ROLL_VALUE;
    }

    //Setter Methods
    public void SetRollInitiative(bool value)
    {
        if (value && !CanRollInitiative())
            return;

        RollInitiative = value;
    }
    public void SetInitiativeCount(int value)
    {
        InitiativeCount = Mathf.Clamp(value, 0, MAX_INITIATIVE_COUNT);
    }
    public void SetInitiativeLevel(TurnStatus value)
    {
        InitiativeLevel = value switch
        {
            TurnStatus.MOVE_FIRST or TurnStatus.PRIORITY_1 or TurnStatus.MOVE_LAST => value,
            _ => TurnStatus.NOTHING,
        };
    }
    public void SetInitiativeRollValue(int value)
    {
        InitiativeValue = Mathf.Clamp(value, MIN_ROLL_VALUE, MAX_ROLL_VALUE);
        SetCheckRollValue(true);
    }
    public void SetCheckRollValue(bool value)
    {
        CheckRollValue = value;
    }

    /// <summary>
    /// Checks if you are still able
    /// to roll for initiative.
    /// </summary>
    /// <returns><c>TRUE</c> if you are able to roll for initiative. <c>FALSE</c> otherwise.</returns>
    public bool CanRollInitiative()
    {
        return InitiativeCount > 0;
    }
}