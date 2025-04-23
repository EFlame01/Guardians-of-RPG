using UnityEngine;

///<summary>
/// StatChangingMove is a class that extends
/// the Move class. StatChangingMoves change
/// the stats of the target by the amount of
/// stages provided.
///</summary>
public class StatChangingMove : Move
{
    public string[] _stats;
    public int[] _stages;

    //Constructor
    public StatChangingMove(string name, string description, double power, double accuracy, string archetypeName, int level, MoveTarget target, MoveType type, double elixirPoints, Effect[] secondaryEffects, string[] stats, int[] stages)
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

        _stats = stats;
        _stages = stages;
    }

    //TODO: update method to let user know if stats can go any higher or lower,
    ///<summary>
    /// Changes the stats of the <paramref name="target"/> based off of
    /// the Stats and the Stages array.
    ///</summary>
    ///<param name="user"> the user of the move. </param>
    ///<param name="target"> the target for the move. </param>
    public override void UseMove(Character user, Character target)
    {
        base.UseMove(user, target);
        for(int i = 0; i < _stats.Length; i++)
            target.BaseStats.ChangeStat(_stats[i], _stages[i]);
    }

    public override void UseMove(Character user, Character target, double epMultiplyer)
    {
        UseMove(user, target);
    }
}