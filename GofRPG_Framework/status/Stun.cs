using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Stun is a class that extends the StatusCondition
/// class. Stun is a status condition that randomly
/// prevents the character from moving.
///</summary>
public class Stun : StatusCondition
{
    private int _stunProbability;

    //Constructor
    ///<param name="stunProbability">
    /// the number that determines the stun probability from 1-3.
    /// See <see cref="Unit.STUN_PROB_1"/>, <see cref="Unit.STUN_PROB_2"/>, and
    /// <see cref="Unit.STUN_PROB_3"/>.
    ///</param>
    public Stun(int stunProbability)
    {
        Name = "STUN";

        _stunProbability = stunProbability switch
        {
            1 => Units.STUN_PROB_1,
            2 => Units.STUN_PROB_2,
            3 => Units.STUN_PROB_3,
            _ => Units.STUN_PROB_1,
        };

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
            {"RESTRAIN", true},
            {"SLEEP", true},
            {"STUN", false},
        };
    }

    ///<summary>
    /// Determines if the <paramref name="character"> should still be stunned due
    /// to their status condition, or if the <paramref name="character"> can
    /// move based on the _stunProbability.
    ///<summary>
    ///<param name="character"> the character being stunned </param>
    public override void ImplementStatusCondition(Character character)
    {
        int percent = Random.Range(0, 100);
        if(_stunProbability >= percent)
        {
            character.BattleStatus.SetTurnStatus(TurnStatus.CANNOT_MOVE);
            character.BattleStatus.SetTurnStatusTag(character.Name + " is stunned!");
        }
    }
}