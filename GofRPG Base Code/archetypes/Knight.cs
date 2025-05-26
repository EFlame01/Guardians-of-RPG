using UnityEngine;

///<summary>
/// The Knight Class is a class that extends
/// the Archetype Class. Knight are in the
/// DEFENDER class, which are known for their 
/// DEFENSE.
///</summary>
public class Knight : Archetype
{
    //Constructor
    public Knight()
    {
        ClassName = "DEFENDER";
        ArchetypeName = "KNIGHT";
        BaseStats = new BaseStats(4, 8, 3, 11, 4);
        StatBoost1 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
        StatBoost2 = new int[6]{1,2,1,1,2,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
        StatBoost3 = new int[6]{2,1,2,1,1,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,1,2,2,1,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
        StatBoost5 = new int[6]{2,1,1,1,2,(int)(Random.Range(1, 5)*Units.DEFENDER_ELIXIR_RATE)};
    }
}