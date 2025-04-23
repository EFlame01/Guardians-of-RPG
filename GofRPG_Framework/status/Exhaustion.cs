using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exhaustion is a class that extends the StatusCondition class.
/// Exhaustion will have several effects depending on the
/// Exhaustion Level:
/// 1: Disadvantage on rolls
/// 2: Speed will be halfed
/// 3: Attack will be halfed
/// 4: Hit Point max will be halfed
/// 5: Speed reduced to 0
/// 6: Automatic Knock Out
/// </summary>
public class Exhaustion : StatusCondition
{
    public Exhaustion()
    {
        Name = "EXHAUSTION";
        AfflictionText = "exhausted";
        WhenToImplement = "'NOW'";
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
            {"STUN", true},
        };
    }

    public override void ImplementStatusCondition(Character character)
    {
        character.BattleStatus.SetExhaustionLevel();

        switch(character.BattleStatus.ExhaustionLevel)
        {
            case Units.EXHAUSTION_LEVEL_1:
                character.BattleStatus.SetRollAdvantage(-2);
                break;
            case Units.EXHAUSTION_LEVEL_2:
                character.BaseStats.SetSpd((int)(character.BaseStats.Spd * Units.STAGE_NEG_2));
                break;
            case Units.EXHAUSTION_LEVEL_3:
                character.BaseStats.SetAtk((int)(character.BaseStats.Atk * Units.STAGE_NEG_2));
                break;
            case Units.EXHAUSTION_LEVEL_4:
                character.BaseStats.SetFullHp((int)(character.BaseStats.FullHp * Units.STAGE_NEG_2));
                break;
            case Units.EXHAUSTION_LEVEL_5:
                character.BaseStats.SetSpd(0);
                break;
            case Units.EXHAUSTION_LEVEL_6:
                character.BaseStats.SetHp(0);
                break;
        }
    }
}
