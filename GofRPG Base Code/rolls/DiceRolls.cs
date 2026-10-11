using UnityEngine;

/// <summary>
/// DiceRolls is a class that facilitates
/// the rolls that a <c>Character</c> will have
/// in the game. Dice rolls will be determined 
/// by the following roles:
///     <list type="bullet">
///         <item>D6: 6-sided die</item>
///         <item>D8: 8-sided die</item>
///         <item>D20: 20-sided die</item>
///     </list>
/// </summary>
public class DiceRolls
{
    //public constant variables
    public const int D6 = 6;
    public const int D8 = 8;
    public const int D20 = 20;

    //private static variable
    private static DiceRolls diceRolls;

    /// <summary>
    /// This method gets the single instance
    /// of the <c>DiceRolls</c> class to be used
    /// for future dice rolls method.
    /// </summary>
    /// <returns>The instance of the DiceRoll</returns>
    public static DiceRolls GetInstance()
    {
        if (diceRolls != null)
            return diceRolls;

        diceRolls = new DiceRolls();
        return diceRolls;
    }

    /// <summary>
    /// Rolls a number between 1 and 6. The number is
    /// influenced by the <paramref name="character"/>'s
    /// Advantage Stat.
    /// </summary>
    /// <param name="character">The person who is rolling the D6</param>
    /// <returns>a number between 1 and 6</returns>
    public int RollD6(Character character)
    {
        return (int)((Random.Range(1, D6) + 1) * character.BaseStats.Adv);
    }

    /// <summary>
    /// Rolls a number between 1 and 8. The number is
    /// influenced by the <paramref name="character"/>'s
    /// Advantage Stat.
    /// </summary>
    /// <param name="character">The person who is rolling the D8</param>
    /// <returns>a number between 1 and 8</returns>
    public int RollD8(Character character)
    {
        return (int)((Random.Range(1, D8) + 1) * character.BaseStats.Adv);
    }

    /// <summary>
    /// Rolls a number between 1 and 20. The number is
    /// influenced by the <paramref name="character"/>'s
    /// Advantage Stat.
    /// </summary>
    /// <param name="character">The person who is rolling the D20</param>
    /// <returns>a number between 1 and 20</returns>
    public int RollD20(Character character)
    {
        return (int)((Random.Range(1, D20) + 1) * character.BaseStats.Adv);
    }

    /// <summary>
    /// Rolls two D20s. Depending on the 
    /// <paramref name="character"/>'s Advantage Stat, 
    /// this method will choose the die with the lowest, or
    /// highest number.
    /// </summary>
    /// <param name="character">The person who is rolling the D8</param>
    /// <returns>a number between 1 and 20</returns>
    public int RollSkillCheck(Character character)
    {
        int value1 = Random.Range(1, D20) + 1;
        int value2 = Random.Range(1, D20) + 1;

        if (character.BaseStats.Adv < Units.STAGE_0)
            return value1 < value2 ? value1 : value2;
        else if (character.BaseStats.Adv > Units.STAGE_0)
            return value1 > value2 ? value1 : value2;
        else
            return value1;
    }
}