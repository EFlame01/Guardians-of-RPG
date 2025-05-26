using UnityEngine;

///<summary>
/// The Berserker Class is a class that extends
/// the Archetype Class. Berserkers are in the
/// BRAWLER class, which are known for their 
/// SPEED.
///</summary>
public class Berserker : Archetype
{
    //Constructor
    public Berserker()
    {
        ClassName = "BRAWLER";
        ArchetypeName = "BERSERKER";
        BaseStats = new BaseStats(7, 5, 2, 8, 8);
        StatBoost1 = new int[6]{2,1,1,1,2,(int)(Random.Range(1, 5)*Units.BRAWLER_ELIXIR_RATE)};
        StatBoost2 = new int[6]{1,2,1,1,2,(int)(Random.Range(1, 5)*Units.BRAWLER_ELIXIR_RATE)};
        StatBoost3 = new int[6]{1,1,2,2,1,(int)(Random.Range(1, 5)*Units.BRAWLER_ELIXIR_RATE)};
        StatBoost4 = new int[6]{1,1,2,2,1,(int)(Random.Range(1, 5)*Units.BRAWLER_ELIXIR_RATE)};
        StatBoost5 = new int[6]{1,2,1,2,1,(int)(Random.Range(1, 5)*Units.BRAWLER_ELIXIR_RATE)};
    } 
}