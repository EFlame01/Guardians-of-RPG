using System.Collections.Generic;

///<summary>
/// Poison is a class that extends the StatusCondition
/// class. Like <see cref="Burn"/>, Poison also deals damage
/// incrementally. However, there is more than one rate
/// to poison a character.
///</summary>
public class Poison : StatusCondition
{
    private double _poisonDmg;

    //Constructor
    ///<param name="stage">
    /// the poison rate from a scale of 1-3.
    ///</param>
    public Poison(int stage)
    {
        Name = "POISON";
        AfflictionText = "poisoned";
        WhenToImplement = "'AFTER ROUND'";
        _poisonDmg = stage switch
        {
            1 => Units.POISON_DMG_STG_1,
            2 => Units.POISON_DMG_STG_2,
            3 => Units.POISON_DMG_STG_3,
            _ => Units.POISON_DMG_STG_1,
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
            {"POISON", false},
            {"RESTRAIN", true},
            {"SLEEP", true},
            {"STUN", true},
        };
    }

    ///<summary>
    /// Takes the <paramref name="character"/> and decrements
    /// their base hp by the _poisonDmg value.
    ///</summary>
    ///<param name="character"> the character being poisoned. </param>
    public override void ImplementStatusCondition(Character character)
    {
        int oldHp = character.BaseStats.Hp;
        int newHp = oldHp - (int)(oldHp * _poisonDmg);
        if(oldHp == newHp)
            newHp--;
        character.BaseStats.SetHp(newHp);
    }
}