using UnityEngine;

///<summary>
/// The RegularSwordsman Class is a class that extends
/// the Archetype Class. RegularSwordsman are in the
/// SWORDSMAN class, which are known for their 
/// ATTACK.
///</summary>
public class RegularSwordsman : Archetype
{
    //Constructor
    public RegularSwordsman()
    {
        ClassName = "SWORDSMAN";
        ArchetypeName = "REGULAR SWORDSMAN";
        BaseStats = new BaseStats(8,3,3,10,6);
        StatBoost1 = new int[6]{2,1,1,1,2,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
        StatBoost2 = new int[6]{2,1,1,2,1,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
        StatBoost3 = new int[6]{1,2,2,1,1,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,1,2,1,2,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
        StatBoost5 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.SWORDSMAN_ELIXIR_RATE)};
    }
}