using System.Collections.Generic;

///<summary>
/// Deafen is a class that extends the StatusCondition class. 
/// Deafen will lower the character's EVA stat by 75% no matter
/// the stage.
///</summary>
public class Deafen : StatusCondition
{
    //Constructor
    public Deafen()
    {
        Name = "DEAFEN";
        AfflictionText = "deafened";
        WhenToImplement = "'NOW'";
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", true},
            {"BURN", true},
            {"CHARM", false},
            {"CONFUSE", true},
            {"DEAFEN", false},
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
        character.BaseStats.SetEva((int)(character.BaseStats.Eva * Units.STAGE_NEG_6));
    }
}