using System.Collections.Generic;

///<summary>
/// Burn is a class that extends the StatusCondition class. 
/// Burn will inflict a set percentage of the character's
/// base hp when implemented.
///</summary>
public class Burn : StatusCondition
{
    //Constructor
    public Burn()
    {
        Name = "BURN";
        AfflictionText = "burned";
        WhenToImplement = "'AFTER ROUND'";
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", true},
            {"BURN", false},
            {"CHARM", true},
            {"CONFUSE", true},
            {"DEAFEN", true},
            {"EXHAUSTION", true},
            {"FLINCH", true},
            {"FRIGHTEN", true},
            {"FROZEN", false},
            {"PETRIFIED", false},
            {"POISON", true},
            {"RESTRAIN", true},
            {"SLEEP", true},
            {"STUN", true},
        };
    }

    ///<summary>
    /// Takes the <paramref name="character"/> and decrements
    /// their base hp by the BURN_DMG value.
    /// See <see cref="Unit.BURN_DMG"/>
    ///</summary>
    ///<param name="character"> the character being burned. </param>
    public override void ImplementStatusCondition(Character character)
    {
        int oldHp = character.BaseStats.Hp;
        int newHp = oldHp - (int)(oldHp * Units.BURN_DMG);
        if(oldHp == newHp)
            newHp--;
        character.BaseStats.SetHp(newHp);
    }
}