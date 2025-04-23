using System.Collections.Generic;

/// <summary>
/// HealthBoostEffect is a class that extends
/// the <c>Effect</c> class. HealthBoostEffect
/// allows <c>Character</c> objects to experience a health
/// boost when the effect is activated.
/// </summary>
public class HealthBoostEffect : Effect
{
    private double _healthBoost;

    //Constructor
    public HealthBoostEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy, double healthBoost)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;
        
        _healthBoost = healthBoost;
    }

    /// <summary>
    /// Gives health boost to <paramref name="target"/>.
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        List<string> resultList = new List<string>();
        int healthBoost = (int)(target.BaseStats.FullHp * _healthBoost);
        
        target.BaseStats.SetHp(target.BaseStats.Hp + healthBoost);
        resultList.Add(target.Name + " health was restored!");

        return resultList.ToArray();
    }
}