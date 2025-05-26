using UnityEngine;

///<summary>
/// The WeaponSpecialist Class is a class that extends
/// the Archetype Class. WeaponSpecialist are in the
/// SPECIALIST class, which are known for their 
/// EVASION.
///</summary>
public class WeaponSpecialist : Archetype
{
    //Constructor
    public WeaponSpecialist()
    {
        ClassName = "SPECIALIST";
        ArchetypeName = "WEAPON SPECIALIST";
        BaseStats = new BaseStats(6, 4, 5, 11, 4);
        StatBoost1 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
        StatBoost2 = new int[6]{2,1,1,2,2,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
        StatBoost3 = new int[6]{1,1,3,1,1,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,2,2,1,1,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
        StatBoost5 = new int[6]{1,1,2,2,1,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
    }
}