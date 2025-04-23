using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Frighten is a class that extends the StatusCondition class.
/// Frighten will impact the Roll Advantage of the frightened
/// character, preventing them from escaping or blocking moves
/// effectively.
/// </summary>
public class Frighten : StatusCondition
{
    private int _rollAdvantage;
    
    public Frighten(int rollAdvantage)
    {
        Name = "FRIGHTEN";
        AfflictionText = "frightened";
        WhenToImplement = "'NOW'";
        _rollAdvantage = Mathf.Clamp(rollAdvantage, -6, -1);
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", true},
            {"BURN", true},
            {"CHARM", true},
            {"CONFUSE", true},
            {"DEAFEN", true},
            {"EXHAUSTION", true},
            {"FLINCH", true},
            {"FRIGHTEN", false},
            {"FROZEN", true},
            {"PETRIFIED", true},
            {"POISON", true},
            {"RESTRAIN", true},
            {"SLEEP", true},
            {"STUN", true},
        };
    }

    public override void ImplementStatusCondition(Character character)
    {
        character.BattleStatus.SetRollAdvantage(_rollAdvantage);
        RemoveStatusCondition(character, Name);
    }
}
