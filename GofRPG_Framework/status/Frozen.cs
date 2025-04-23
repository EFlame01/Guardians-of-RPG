using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Frozen is a class that extends the StatusCondition
/// class. Frozen is a status condition that freezes 
/// a character, and they have a set chance of thawing
/// out after each round.
///</summary>
public class Frozen : StatusCondition
{
    private int _freezeChance;
    private int _roundsLeft;

    //Constructor
    ///<param name="freezeChance">
    /// the number that determines the percentage they will freeze between 1-3.
    ///</param>
    public Frozen(int freezeChance)
    {
        Name = "FROZEN";
        AfflictionText = "frozened";
        WhenToImplement = "'DURING ROUND'";
        _roundsLeft = Units.FREEZE_DURATION;
        _freezeChance = freezeChance switch{
            1 => Units.FREEZE_CHANCE_1,
            2 => Units.FREEZE_CHANCE_2,
            3 => Units.FREEZE_CHANCE_3,
            _ => Units.FREEZE_CHANCE_1
        };
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
            {"PETRIFIED", true},
            {"POISON", false},
            {"RESTRAIN", true},
            {"SLEEP", false},
            {"STUN", false},
        };
    }

    ///<summary>
    /// Determines if the <paramref name="character"/> should
    /// become/remain frozen based on the _freezeChance.
    ///</summary>
    ///<param name="character"> the character being frozen.</param>
    public override void ImplementStatusCondition(Character character)
    {
        int percent = Random.Range(0, 100);
        if(_freezeChance >= percent || _roundsLeft > 0)
        {
            character.BattleStatus.SetTurnStatus(TurnStatus.CANNOT_MOVE);
            character.BattleStatus.SetTurnStatusTag(character.Name + " is frozen!");
            _roundsLeft--;
        }
        else
            RemoveStatusCondition(character, Name);
    }
}