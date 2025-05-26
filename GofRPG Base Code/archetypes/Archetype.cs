using UnityEngine;

///<summary>
/// Archetype is a class that is assigned to a 
/// character to determine what moves they will learn,
/// what abilities they will develop, how their stats will
/// increase as they level, and how effective certain moves
/// will be.
///</summary>
public class Archetype
{
    public string ClassName {get; protected set;}
    public string ArchetypeName {get; protected set;}
    public BaseStats BaseStats  {get; protected set;}
    protected int[] StatBoost1 {private get; set;} //50% chance of getting boost stat
    protected int[] StatBoost2 {private get; set;} //25% chance of getting boost stat
    protected int[] StatBoost3 {private get; set;} //12% chance of getting boost stat
    protected int[] StatBoost4 {private get; set;} //7% chance of getting boost stat
    protected int[] StatBoost5 {private get; set;} //6% chance of getting boost stat

    ///<summary>
    /// Picks a number between 0 and 100 and selects 1 of 5
    /// stat boosting options based off of the result.
    /// <list> - statBoost1 = 50% likely </list>
    /// <list> - statBoost2 = 25% likely </list>
    /// <list> - statBoost3 = 12% likely </list>
    /// <list> - statBoost4 =  7% likely </list>
    /// <list> - statBoost6 =  6% likely </list>
    ///</summary>
    public int[] ChooseStatBoostRandomly()
    {
        int percent = Random.Range(0, 100) + 1;

        if(percent < 50)
            return StatBoost1;
        else if(percent < 75)
            return StatBoost2;
        else if(percent < 87)
            return StatBoost3;
        else if(percent < 94)
            return StatBoost4;
        else
            return StatBoost5;
    }

    ///<summary>
    /// Returns an instance of a new Archetype based off of the
    /// name of the <paramref name="archetype"/>. If the
    /// <paramref name="archetype"/> name does not match,
    /// then it will return null.
    ///</summary>
    ///<param name="archetype">the archetype name in all caps.</param>
    ///<returns> Any of the 10 archetypes that extend the Archetype class, or null </returns>
    public static Archetype GetArchetype(string archetype)
    {
        return archetype switch
        {
            "REGULAR SWORDSMAN" => new RegularSwordsman(),
            "DUAL SWORDSMAN" => new DualSwordsman(),
            "KNIGHT" => new Knight(),
            "HEAVY SHIELDER" => new HeavyShielder(),
            "ENERGY MANIPULATOR" => new EnergyManipulator(),
            "NATURE MANIPULATOR" => new NatureManipulator(),
            "MIXED MARTIAL ARTIST" => new MixedMartialArtist(),
            "BERSERKER" => new Berserker(),
            "WEAPON SPECIALIST" => new WeaponSpecialist(),
            "COMBAT SPECIALIST" => new CombatSpecialist(),
            _ => null,
        };

    }
}