using UnityEngine;

/// <summary>
/// BlockStatus is a class that determines the 
/// <c>Character</c>'s ability to roll for blocking
/// (the ability to withstand a physical attack for a round).
/// </summary>
public class BlockStatus
{
    //public variables
    public bool RollBlock { get; private set; }
    public int BlockCount { get; set; }
    public int BlockValue { get; set; }

    //private const variables
    private const int MAX_BLOCK_COUNT = 10;
    private const int MAX_ROLL_VALUE = 20;
    private const int MIN_ROLL_VALUE = 1;
    private const int DEFAULT_ROLL_VALUE = 0;

    //Constructor
    public BlockStatus()
    {
        RollBlock = false;
        BlockCount = MAX_BLOCK_COUNT;
        BlockValue = DEFAULT_ROLL_VALUE;
    }

    //Setter Methods
    public void SetRollBlock(bool value)
    {
        if (value && !CanRollBlock())
            return;

        RollBlock = value;
    }
    public void SetBlockCount(int value)
    {
        BlockCount = Mathf.Clamp(value, DEFAULT_ROLL_VALUE, MAX_BLOCK_COUNT);
    }
    public void SetBlockValue(int value)
    {
        BlockValue = Mathf.Clamp(value, MIN_ROLL_VALUE, MAX_ROLL_VALUE);
    }

    /// <summary>
    /// Determines if you are still able
    /// to roll for block.
    /// </summary>
    /// <returns><c>TRUE</c> if you are able to roll for block. <c>FALSE</c> otherwise.</returns>
    public bool CanRollBlock()
    {
        return BlockCount > 0;
    }
}