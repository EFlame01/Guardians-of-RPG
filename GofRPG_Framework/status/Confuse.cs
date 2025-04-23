using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Confuse is a class that extends the StatusCondition class.
/// Confuse will have a 30% chance of making the confused character
/// hit themselves in confusion. Being confused last up 
/// to 5 rounds.
/// </summary>
public class Confuse : StatusCondition
{
    private int _roundsLeft;

    public Confuse(int confuseDuration)
    {
        Name = "CONFUSE";
        AfflictionText = "confused";
        WhenToImplement = "'DURING ROUND'";
        _roundsLeft = Mathf.Clamp(confuseDuration, 1, 5);
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", true},
            {"BURN", true},
            {"CHARM", false},
            {"CONFUSE", false},
            {"DEAFEN", true},
            {"EXHAUSTION", true},
            {"FLINCH", true},
            {"FRIGHTEN", true},
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
        if(_roundsLeft <= 0)
        {
            RemoveStatusCondition(character, Name);
            return;
        }

        if(Units.CONFUSION_RATE > Random.Range(1, 100))
        {
            int atk = (int)(character.BaseStats.FullHp * Units.CONFUSION_HIT);
            int newHp = character.BaseStats.Hp - atk;
            string pronoun = character.Sex switch{
                "MALE" => "himself",
                "FEMALE" => "herself",
                "MALEFE" => "themself",
                _ => "itself",
            };
            character.BaseStats.SetHp(newHp);
        }
        
        character.BattleStatus.SetTurnStatus(TurnStatus.CANNOT_MOVE);
        character.BattleStatus.SetTurnStatusTag(character.Name + " is confused!");
        _roundsLeft--;
    }
}
