using System.Collections.Generic;

/// <summary>
/// StatusConditionEffect is a class that extends
/// the <c>Effect</c> class. StatusConditionEffects
/// inflicts <c>Character</c> objects with status
/// conditions.
/// </summary>
public class StatusConditionEffect : Effect
{
    public StatusCondition _statusCondition { get; private set; }
    public StatusConditionEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy, StatusCondition statusCondition)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;

        _statusCondition = statusCondition;
    }

    /// <summary>
    /// Gives the <paramref name="target"/> a status condition.
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        List<string> resultList = new List<string>();

        if (!StatusCondition.CanStackStatusCondition(target, _statusCondition.Name))
            resultList.Add(_statusCondition.Name + " does not stack...");
        else if (_statusCondition.Name.Equals("FLINCH"))
            _statusCondition.ImplementStatusCondition(target);
        else if (target.BattleStatus.Immunities[_statusCondition.Name] || target.BattleStatus.Immunities["STATUS_CONDITION"])
            resultList.Add("It does not effect " + target.Name);
        else
        {
            target.BattleStatus.StatusConditions.Add(_statusCondition.Name, _statusCondition);
            switch (_statusCondition.Name)
            {
                case "BLIND":
                    resultList.Add(target.Name + " is blinded!");
                    break;
                case "BURN":
                    resultList.Add(target.Name + " is burned!");
                    break;
                case "CHARM":
                    resultList.Add(target.Name + " is charmed!");
                    break;
                case "CONFUSED":
                    resultList.Add(target.Name + " is confused!");
                    break;
                case "DEAFEN":
                    resultList.Add(target.Name + " is deafened!");
                    break;
                case "EXHAUSTION":
                    resultList.Add(target.Name + " is exhausted!");
                    break;
                case "FROZEN":
                    resultList.Add(target.Name + " is frozened!");
                    break;
                case "PETRIFIED":
                    resultList.Add(target.Name + " is petrified!");
                    break;
                case "POISON":
                    resultList.Add(target.Name + " is inflicted with poison!");
                    break;
                case "RESTRAIN":
                    resultList.Add(target.Name + " is restrained!");
                    break;
                case "SLEEP":
                    resultList.Add(target.Name + " has fallen asleep!");
                    break;
                case "STUN":
                    resultList.Add(target.Name + " is stunned!");
                    break;
                default:
                    break;
            }
        }

        return resultList.ToArray();
    }
}