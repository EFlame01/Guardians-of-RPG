using UnityEngine;

///<summary>
/// The HeavyShielder Class is a class that extends
/// the Archetype Class. HeavyShielder are in the
/// DEFENDER class, which are known for their 
/// DEFENSE.
///</summary>
public class HeavyShielder : Archetype
{
    //Constructor
    public HeavyShielder()
    {
        ClassName = "DEFENDER";
        ArchetypeName = "HEAVY SHIELDER";
        BaseStats = new BaseStats(4, 9, 3, 10, 4);
        StatBoost1 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
        StatBoost2 = new int[6]{1,2,1,1,2,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
        StatBoost3 = new int[6]{2,1,2,1,1,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,1,2,2,1,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
        StatBoost5 = new int[6]{2,1,1,1,2,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
    }
}