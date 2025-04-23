using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flinch : StatusCondition
{
    public Flinch()
    {
        Name = "FLINCH";
        AfflictionText = "flinched!";
        WhenToImplement = "'DURING ROUND'";
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", true},
            {"BURN", true},
            {"CHARM", true},
            {"CONFUSE", true},
            {"DEAFEN", true},
            {"EXHAUSTION", true},
            {"FLINCH", false},
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
        character.BattleStatus.SetTurnStatus(TurnStatus.CANNOT_MOVE);
        character.BattleStatus.SetTurnStatusTag(character.Name + " flinched!");
        RemoveStatusCondition(character, Name);
    }
}
