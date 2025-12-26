
/// <summary>
/// AbilityMaker is a class that parses through
/// the data to create <c>Ability</c> objects for
/// the <c>Character</c> class.
/// </summary>
public class AbilityMaker : Singleton<AbilityMaker>
{
    private const int ABILITY_INDEX = 0;

    /// <summary>
    /// Gets and returns an ability based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the ability</param>
    /// <returns>the <c>Ability</c> object or <c>null</c> if an ability could not be found.</returns>
    public Ability GetAbilityBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        string[] abilityAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[ABILITY_INDEX], name).Split(',');

        if (abilityAttributes == null)
            return null;

        return new Ability
        (
            abilityAttributes[0],
            abilityAttributes[1].Replace('~', ','),
            EffectMaker.Instance.GetEffectsBasedOnName(abilityAttributes[0]),
            abilityAttributes[2].Split('~')
        );
    }
}