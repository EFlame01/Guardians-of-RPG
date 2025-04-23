using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Petrified is a class that extends the StatusCondition
/// class. Petrified is a status condition that freezes the
/// opponent in place. Petrification lasts until the character
/// is unpetrified, either by an item, by chance, or until
/// the battle is over.
///</summary>
public class Petrified : StatusCondition
{
    private int _roundsLeft;
    //Constructor
    public Petrified()
    {
        Name = "PETRIFIED";
        AfflictionText = "petrified";
        WhenToImplement = "'DURING ROUND'";
        _roundsLeft = Units.PETRIFIED_ROUNDS;
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", false},
            {"BURN", false},
            {"CHARM", false},
            {"CONFUSE", false},
            {"DEAFEN", false},
            {"EXHAUSTION", false},
            {"FLINCH", false},
            {"FRIGHTEN", false},
            {"FROZEN", false},
            {"PETRIFIED", false},
            {"POISON", false},
            {"RESTRAIN", true},
            {"SLEEP", false},
            {"STUN", false},
        };
    }

    ///<summary>
    /// Determines if the <paramref name="character"/> should
    /// become/remain petrified based on the PETRIFIED_RATE.
    /// See <see cref="Unit.PETRIFIED_RATE"/>
    ///</summary>
    ///<param name="character"> the character being petrified. </param>
    public override void ImplementStatusCondition(Character character)
    {
        int percent = Random.Range(0, 100) + 1;
        if(Units.PETRIFIED_RATE >= percent || _roundsLeft > 0)
        {
            character.BattleStatus.SetTurnStatus(TurnStatus.CANNOT_MOVE);
            character.BattleStatus.SetTurnStatusTag(character.Name + " is petrified!");
            _roundsLeft--;
        }
        else
            RemoveStatusCondition(character, Name);
    }
}