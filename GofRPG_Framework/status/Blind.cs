using System.Collections.Generic;

///<summary>
/// Blind is a class that extends the StatusCondition class. 
/// Blind will half both the accuracy and the evasion of the character
/// no matter the stage.
///</summary>
public class Blind : StatusCondition
{
    //Constructor
    public Blind()
    {
        Name = "BLIND";
        AfflictionText = "blinded";
        WhenToImplement = "'NOW'";
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", false},
            {"BURN", true},
            {"CHARM", false},
            {"CONFUSE", true},
            {"DEAFEN", true},
            {"EXHAUSTION", true},
            {"FLINCH", true},
            {"FRIGHTEN", true},
            {"FROZEN", true},
            {"PETRIFIED", false},
            {"POISON", true},
            {"RESTRAIN", true},
            {"SLEEP", true},
            {"STUN", true},
        };
    }

    public override void ImplementStatusCondition(Character character)
    {
        character.BaseStats.SetAcc((int)(character.BaseStats.Acc * Units.STAGE_NEG_2));
        character.BaseStats.SetEva((int)(character.BaseStats.Eva * Units.STAGE_NEG_2));
    }
}