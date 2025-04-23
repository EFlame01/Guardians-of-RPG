
/// <summary>
/// AbilityMaker is a class that parses through
/// the data to create <c>Ability</c> objects for
/// the <c>Character</c> class.
/// </summary>
public class AbilityMaker : Singleton<AbilityMaker>
{
    private readonly string _abilityDataPath = "/database/abilities.csv";

    /// <summary>
    /// Gets and returns an ability based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the ability</param>
    /// <returns>the <c>Ability</c> object or <c>null</c> if an ability could not be found.</returns>
    public Ability GetAbilityBasedOnName(string name)
    {
        if(string.IsNullOrEmpty(name)) 
            return null;
        
        string[] mainAttributes;

        DataEncoder.Instance.DecodeFile(_abilityDataPath);
        mainAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');
        DataEncoder.ClearData();
        
        return new Ability
        (
            mainAttributes[0],
            mainAttributes[1],
            EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0]),
            mainAttributes[2].Split('~')
        );
    }
}