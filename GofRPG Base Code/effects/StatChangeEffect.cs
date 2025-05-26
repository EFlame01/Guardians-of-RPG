using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatChangeEffect is a class that extends
/// the <c>Effect</c> class. StatChangeEffects
/// allows <c>Character</c> objects to buff or
/// debuff their target's stats once the ability
/// activates.
/// </summary>
public class StatChangeEffect : Effect
{
    private string [] _statNames;
    private int [] _statStages;

    //Constructor
    public StatChangeEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy, string[] statNames, int[] statStages)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;

        _statNames = statNames;
        _statStages = statStages;
    }

    /// <summary>
    /// Buffs or debuffs the <paramref name="target"/> stats.
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        List<string> resultList = new List<string>();

        for(int i = 0; i < _statNames.Length; i++)
        {
            string effect = target.Name + "' " + _statNames[i];

            if(_statStages[i] > 0)
                effect += " increased " + Mathf.Abs(_statStages[i]) + " stages!";
            else
                effect += " decreased " + Mathf.Abs(_statStages[i]) + " stages!";

            target.BaseStats.ChangeStat(_statNames[i], _statStages[i]);
            resultList.Add(effect);
        }

        return resultList.ToArray();
    }
}