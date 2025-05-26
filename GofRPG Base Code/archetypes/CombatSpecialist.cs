using UnityEngine;

///<summary>
/// The CombatSpecialist Class is a class that extends
/// the Archetype Class. CombatSpecialist are in the
/// SPECIALIST class, which are known for their 
/// EVASION.
///</summary>
public class CombatSpecialist : Archetype
{
    //Constructor
    public CombatSpecialist()
    {
        ClassName = "SPECIALIST";
        ArchetypeName = "COMBAT SPECIALIST";
        BaseStats = new BaseStats(4, 6, 5, 9, 6);
        StatBoost1 = new int[6]{2,1,1,1,2,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
        StatBoost2 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
        StatBoost3 = new int[6]{1,1,3,1,1,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,1,2,2,1,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
        StatBoost5 = new int[6]{1,2,2,1,1,(int)(Random.Range(1, 5)*Units.SPECIALIST_ELIXIR_RATE)};
    }
}