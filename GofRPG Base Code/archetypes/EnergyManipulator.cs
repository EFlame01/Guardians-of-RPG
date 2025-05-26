using UnityEngine;

///<summary>
/// The EnergyManipulator Class is a class that extends
/// the Archetype Class. EnergyManipulators are in the
/// ESOIC class, which are known for their 
/// HEALTH.
///</summary>
public class EnergyManipulator : Archetype
{
    //Constructor
    public EnergyManipulator()
    {
        ClassName = "ESOIC";
        ArchetypeName = "ENERGY MANIPULATOR";
        BaseStats = new BaseStats(6, 6, 2, 13, 3);
        StatBoost1 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
        StatBoost2 = new int[6]{2,1,1,2,1,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
        StatBoost3 = new int[6]{1,1,2,1,2,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,2,2,1,1,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
        StatBoost5 = new int[6]{2,1,1,1,2,(int)(Random.Range(1, 5)*Units.ESOIC_ELIXIR_RATE)};
    }
}