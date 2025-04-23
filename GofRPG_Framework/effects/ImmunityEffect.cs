using System.Collections.Generic;

/// <summary>
/// ImmunityEffect is a class that extends
/// the <c>Effect</c> class. ImmunityEffect
/// allows <c>Character</c> objects immunity to 
/// certain moves/effects in battle.
/// </summary>
public class ImmunityEffect : Effect
{
    public string _immunityTowards {get; private set;}

    //Constructor
    public ImmunityEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy, string immunityTowards)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;

        _immunityTowards = immunityTowards;
    }

    /// <summary>
    /// Grants the <paramref name="target"/> immunity
    /// based on the <c>_immunityTowards</c> variable.
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        List<string> resultList = new List<string>();
        
        target.BattleStatus.Immunities[_immunityTowards] = true;

        return resultList.ToArray();
    }
}