using UnityEngine;

///<summary>
/// HealingMove is a class that extends the Move
/// class. HealingMoves are designed to heal the 
/// target. Because of this, the move is uneffected
/// by the target's DEF stat.
///</summary>
public class HealingMove : Move
{
    //Constructor
    public HealingMove(string name, string description, double power, double accuracy, string archetypeName, int level, MoveTarget target, MoveType type, double elixirPoints, Effect[] secondaryEffects)
    {
        Name = name;
        Description = description;
        Power = power;
        Accuracy = accuracy;
        ArchetypeName = archetypeName;
        Level = level;
        Target = target;
        Type = type;
        EP = elixirPoints;
        SecondaryEffects = secondaryEffects;
    }

    public override void UseMove(Character user, Character target)
    {
        UseMove(user, target, 1);
    }

    ///<summary>
    /// After calculating the healAmount, the
    /// <paramref name="user"/> proceeds to heal the
    /// <paramref name="target"/> by said heal amount.
    /// This bypasses the <paramref name="target"/>'s
    /// defense.
    ///</summary>
    ///<param name="user"> the user of the move. </param>
    ///<param name="target"> the target for the move. </param>
    ///<param name="epMultiplier"> the elixir point multiplyer. </param>
    public override void UseMove(Character user, Character target, int epMultiplier)
    {
        base.UseMove(user, target, epMultiplier);
        int healAmount = CalculateHealingPower(user, target, epMultiplier);
        int targetHp = target.BaseStats.Hp + healAmount;
        target.BaseStats.SetHp(targetHp);
    }

    /// <summary>
    /// Calculates the heal amount based on the 
    /// <paramref name="user"/> stats, and the 
    /// <paramref name="epMultiplier"/>, and the 
    /// max amount that can be transfered to the
    /// <paramref name="target"/>
    /// </summary>
    /// <param name="user">user of the move</param>
    /// <param name="target">target of the move</param>
    /// <param name="epMultiplier">multiplyer based on extra elixir poured into move</param>
    /// <returns>the amount of HP the <paramref name="user"/> can give the
    /// <paramref name="target"/></returns>
    private int CalculateHealingPower(Character user, Character target, double epMultiplier)
    {
        int healingPower = user.BaseStats.Atk;
        int max = target.BaseStats.FullHp - target.BaseStats.Hp;

        healingPower += (int)(healingPower * Power);

        if (user.Archetype.ClassName == ArchetypeName || user.Archetype.ClassName == ArchetypeName)
            healingPower += (int)(healingPower * Units.STAB_DMG);
        if (epMultiplier != 0)
            healingPower += (int)(healingPower * epMultiplier);

        return Mathf.Clamp(healingPower, 1, max);
    }
}