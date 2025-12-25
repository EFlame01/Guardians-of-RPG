using System.Collections.Generic;

/// <summary>
/// AbilityManager is a class that holds all of the
/// <c>Ability</c> objects that the <c>Player</c> has.
/// </summary>
public class AbilityManager
{
    public static Dictionary<string, Ability> AbilityDictionary { get; private set; }

    //Constructor
    public AbilityManager()
    {
        AbilityDictionary = new Dictionary<string, Ability>();
    }

    /// <summary>
    /// Adds <paramref name="ability"/> to list of
    /// abilities the player can use.
    /// </summary>
    /// <param name="ability">ability that will be added to list.</param>
    /// <returns><c>TRUE</c> if the ability was added successfully.
    /// <c>FALSE</c> if otherwise.</returns>
    public bool AddAbilityToList(Ability ability)
    {
        if (AbilityDictionary.ContainsKey(ability.Name))
            return false;

        AbilityDictionary.Add(ability.Name, ability);
        return true;
    }

    public void AddAbilitiesToList(string[] abilities)
    {
        foreach (string abilityName in abilities)
        {
            Ability ability = AbilityMaker.Instance.GetAbilityBasedOnName(abilityName);
            AddAbilityToList(ability);
        }
    }

    /// <summary>
    /// Equips the <c>Player</c> with the <paramref name="ability"/>.
    /// </summary>
    /// <param name="ability">The ability that the player will have.</param>
    /// <returns><c>TRUE</c> if the ability was equipped successfully.
    /// <c>FALSE</c> if otherwise.</returns>
    public bool EquipAbilityToPlayer(Ability ability)
    {
        if (ability == null)
            return false;

        if (!AbilityDictionary.ContainsKey(ability.Name))
            return false;

        Player.Instance().SetAbility(ability);
        return true;
    }
}