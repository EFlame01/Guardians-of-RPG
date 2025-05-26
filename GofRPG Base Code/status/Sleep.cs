using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Sleep is a class that extends the StatusCondition
/// class. Sleep is a status condition that puts a
/// character to sleep for up to 3 rounds.
///</summary>
public class Sleep : StatusCondition
{
    private int _roundsLeft;

    //Constructor
    ///<param name="rounds"> 
    /// the amount of rounds a character will be asleep when under this status condition. 
    ///</param>
    public Sleep(int rounds)
    {
        Name = "SLEEP";
        AfflictionText = "asleep";
        WhenToImplement = "DURING ROUND";
        _roundsLeft = Mathf.Clamp(rounds, 1, 3);
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", false},
            {"BURN", true},
            {"CHARM", false},
            {"CONFUSE", false},
            {"DEAFEN", true},
            {"EXHAUSTION", false},
            {"FLINCH", false},
            {"FRIGHTEN", false},
            {"FROZEN", true},
            {"PETRIFIED", true},
            {"POISON", true},
            {"RESTRAIN", true},
            {"SLEEP", false},
            {"STUN", true},
        };
    }

    ///<summary>
    /// Determines if the <paramref name="character"> should still be asleep due
    /// to their status condition, or if the <paramref name="character"> should
    /// wake up based on the _roundsLeft value.
    ///<summary>
    ///<param name="character"> the character being slept... lol </param>
    public override void ImplementStatusCondition(Character character)
    {
        if(_roundsLeft > 0)
        {
            _roundsLeft--;
            character.BaseStats.SetEva(0);
            character.BattleStatus.SetTurnStatus(TurnStatus.CANNOT_MOVE);
            character.BattleStatus.SetTurnStatusTag(character.Name + " is asleep!");
        }
        else
        {
            character.BaseStats.ChangeStat("EVA", 0);
            RemoveStatusCondition(character, Name);
        }
    }
}