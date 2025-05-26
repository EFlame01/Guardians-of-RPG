using UnityEngine;

///<summary>
/// The NatureManipulator Class is a class that extends
/// the Archetype Class. NatureManipulator are in the
/// ESOIC class, which are known for their 
/// HEALTH.
///</summary>
public class NatureManipulator : Archetype
{
    //Constructor
    public NatureManipulator()
    {
        ClassName = "ESOIC";
        ArchetypeName = "NATURE MANIPULATOR";
        BaseStats = new BaseStats(6, 5, 2, 15, 2);
        StatBoost1 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
        StatBoost2 = new int[6]{2,1,1,2,1,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
        StatBoost3 = new int[6]{1,1,2,1,2,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,2,2,1,1,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
        StatBoost5 = new int[6]{2,1,1,1,2,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
    }
}