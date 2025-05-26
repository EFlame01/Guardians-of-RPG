using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Restrain is a class that extends the StatusCondition class.
/// Restrain will set the character's speed to 0, and prevent them 
/// from being able to escape battle until they are no longer
/// restrained. Being restrained last up to 2 rounds.
/// </summary>
public class Restrain : StatusCondition
{
    private int _roundsLeft;

    public Restrain(int restrainDuration)
    {
        Name = "RESTRAIN";
        AfflictionText = "restrained";
        WhenToImplement = "'DURING ROUND'";
        _roundsLeft = Mathf.Clamp(restrainDuration, 1, 3);
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", true},
            {"BURN", true},
            {"CHARM", true},
            {"CONFUSE", true},
            {"DEAFEN", true},
            {"EXHAUSTION", true},
            {"FLINCH", true},
            {"FRIGHTEN", true},
            {"FROZEN", true},
            {"PETRIFIED", true},
            {"POISON", true},
            {"RESTRAIN", false},
            {"SLEEP", true},
            {"STUN", true},
        };
    }

    public override void ImplementStatusCondition(Character character)
    {
        if(_roundsLeft <= 0)
        {
            character.BaseStats.ChangeStat("SPD", 0);
            character.BattleStatus.SetCanEscape(true);
            RemoveStatusCondition(character, Name);
        }
        else
        {
            character.BaseStats.SetSpd(0);
            character.BattleStatus.SetCanEscape(false);
            _roundsLeft--;
        }
    }
}
