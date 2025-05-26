using System.Collections.Generic;

/// <summary>
/// RechargeEffect is a class that extends
/// the <c>Effect</c> class. RechargeEffects
/// require <c>Character</c> objects to recharge
/// for a certain amount of rounds once effect is 
/// activated.
/// </summary>
public class RechargeEffect : Effect
{
    public int _rechargeTime;

    //Constructor
    public RechargeEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy, int rechargeTime)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;

        _rechargeTime = rechargeTime;
    }

    /// <summary>
    /// Puts the r<paramref name="target"/> on a cooldown
    /// timer for a certain amount of rounds.
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        List<string> resultList = new List<string>();
        
        target.BattleStatus.SetRechargeTime(_rechargeTime);
        
        return resultList.ToArray();
    }
}