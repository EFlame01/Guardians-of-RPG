using UnityEngine;

///<summary>
/// RegularMove is a class that extends the
/// Move class. Unless they have secondary
/// effects, RegularMoves act like regular
/// moves: the user hits their target to lower
/// their hp with no special gimmick.
///</summary>
public class RegularMove : Move
{
    //Constructor
    public RegularMove(string name, string description, double power, double accuracy, string archetypeName, int level, MoveTarget target, MoveType type, double elixirPoints, Effect[] secondaryEffects)
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
    /// Sets the <paramref name="target"/>'s base hp
    /// and subtracks it by the total damage the move will do.
    ///</summary>
    ///<param name="user"> the user of the move. </param>
    ///<param name="target"> the target for the move. </param>
    ///<param name="epMultiplyer"> the elixir point multiplyer. </param>
    public override void UseMove(Character user, Character target, double epMultiplyer)
    {
        base.UseMove(user, target, epMultiplyer);
        int dmg = CalculateDamage(user, target, epMultiplyer);
        target.BaseStats.SetHp(target.BaseStats.Hp - dmg);
    }
}