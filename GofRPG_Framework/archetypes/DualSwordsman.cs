using UnityEngine;

///<summary>
/// The DualSwordsman Class is a class that extends
/// the Archetype Class. DualSwordsman are in the
/// SWORDSMAN class, which are known for their 
/// ATTACK.
///</summary>
public class DualSwordsman : Archetype
{
    //Constructor
    public DualSwordsman()
    {
        ClassName = "SWORDSMAN";
        ArchetypeName = "DUAL SWORDSMAN";
        BaseStats = new BaseStats(10,3,3,9,5);
        StatBoost1 = new int[6]{2,1,1,1,2,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
        StatBoost2 = new int[6]{2,1,1,2,1,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
        StatBoost3 = new int[6]{1,2,2,1,1,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,1,2,1,2,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
        StatBoost5 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
    }
}