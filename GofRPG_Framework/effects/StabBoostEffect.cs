using System.Collections.Generic;

/// <summary>
/// StabBoostEffect is a class that extends
/// the <c>Effect</c> class. StabBoostEffects
/// allows <c>Character</c> objects to include
/// STAB damage to their moves, even if the move
/// is of a different type.
/// </summary>
public class StabBoostEffect : Effect
{
    //Constructor
    public StabBoostEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;
    }

    /// <summary>
    /// Grants the <paramref name="target"/> Same Type Attack Bonus
    /// or STAB for all moves.
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        target.BattleStatus.SetStab(true);
        List<string> resultList = new List<string>();
        return resultList.ToArray();
    }
}