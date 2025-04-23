using System.Collections.Generic;

/// <summary>
/// RecoilEffect is a class that extends
/// the <c>Effect</c> class. RecoilEffects
/// decrement <c>Character</c> objects health
/// a certain amount once effect is activated.
/// </summary>
public class RecoilEffect : Effect
{
    public double _recoilDamage {get; private set;}

    //Constructor
    public RecoilEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy, double recoilDamage)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;
        
        _recoilDamage = recoilDamage;
    }

    /// <summary>
    /// Inflicts the <paramref name="target"/> with recoil damage. 
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        List<string> resultList = new List<string>();
        int recoilDamage = (int)(target.BaseStats.FullHp * _recoilDamage);

        target.BaseStats.SetHp(target.BaseStats.Hp - recoilDamage);

        resultList.Add(target.Name + " suffered from recoil!");

        return resultList.ToArray();
    }
}