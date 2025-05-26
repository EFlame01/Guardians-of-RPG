using System.Collections.Generic;

/// <summary>
/// NegationEffect is a class that extends
/// the <c>Effect</c> class. NegationEffect
/// allows <c>Character</c> objects to negate
/// certain abilities or protect moves.
/// </summary>
public class NegationEffect : Effect
{
    public string _negationType {get; private set;}

    //Constructor
    public NegationEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy, string negationType)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;

        _negationType = negationType;
    }

    /// <summary>
    /// Negates certain properties of the <paramref name="target" />.
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        List<string> resultList = new List<string>();

        switch(_negationType)
        {
            case "ABILITY":
                //TODO: add ability flag to false
                target.BattleStatus.SetAbilityUse(false);
                resultList.Add(target + "'s ability was negated!");
                break;
            case "PROTECT_MOVE":
                //TODO: set protect_breaker flag to true
                target.BattleStatus.SetProtectBreaker(true);
                break;
            default:
                break;
        }

        return resultList.ToArray();
    }
}