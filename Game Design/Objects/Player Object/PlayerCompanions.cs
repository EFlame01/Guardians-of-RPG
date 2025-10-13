using UnityEngine;

/// <summary>
/// PlayerCompanions is a class that 
/// extends the <c>PersistentSingleton</c> class.
/// PlayerCompanion allows the game to update
/// the Companions that the player has to battle
/// with them in the form of a <c>BattleCharacterData</c> 
/// array.
/// </summary>
public class PlayerCompanions : PersistentSingleton<PlayerCompanions>
{
    //public methods
    public BattleCharacterData[] CompanionData { get; private set; }

    public void Start()
    {
        CompanionData = new BattleCharacterData[2];
    }

    /// <summary>
    /// Adds BattleCharacterData to the list of companions.
    /// If the list is full, then it will not add the data
    /// as a companion.
    /// </summary>
    /// <param name="data">BattleCharacterData for the companion</param>
    public void AddCompanion(BattleCharacterData data)
    {
        bool added = false;
        for (int i = 0; i < CompanionData.Length; i++)
        {
            if (CompanionData[i] == null)
            {
                CompanionData[i] = data;
                added = true;
                break;
            }
        }
        if (!added)
            Debug.LogWarning("WARNING: Could not add battle character data. Placement is full.");
    }

    /// <summary>
    /// Adds multiple BattleCharacterData to the list of 
    /// companions in the form of an array. If the list is full
    /// or the data was null, then it will not add the data
    /// as a companion.
    /// </summary>
    /// <param name="data">An array of BattleCharacterData for the companions</param>
    public void AddCompanions(BattleCharacterData[] data)
    {
        if (data.Length == CompanionData.Length)
            CompanionData = data;
        else if (data == null)
            Debug.LogWarning("WARNING: could not add battle character data because data was null");
        else if (CompanionData.Length != data.Length)
        {
            for (int i = 0; i < data.Length; i++)
                AddCompanion(data[i]);
        }
        else
        {
            for (int i = 0; i < CompanionData.Length; i++)
                CompanionData[i] = data[i];
        }
    }

    /// <summary>
    /// Removes BattleCharacterData from the list of companions
    /// based on the characterData. If the characterData is not
    /// present, it will not remove anything.
    /// </summary>
    /// <param name="characterData">characterID for companion</param>
    public void RemoveCompanion(string characterData)
    {
        for (int i = 0; i < CompanionData.Length; i++)
        {
            if (CompanionData[i] == null)
                continue;
            else if (CompanionData[i].CharacterData.Equals(characterData))
                CompanionData[i] = null;
        }
    }

    /// <summary>
    /// Removes a list of BattleCharacterData from the list of companions
    /// based on their characterData. If the characterData is not
    /// present, it will not remove anything.
    /// </summary>
    /// <param name="characterData">characterID for companion</param>
    public void RemoveCompanions(string[] characterData)
    {
        for (int i = 0; i < characterData.Length; i++)
        {
            for (int j = 0; j < CompanionData.Length; j++)
            {
                if (CompanionData[j] == null)
                    continue;
                else if (CompanionData[j].CharacterData.Equals(characterData[i]))
                    CompanionData[j] = null;
            }
        }
    }

    /// <summary>
    /// Clears the array of BattleCharacterData.
    /// </summary>
    public void ClearCompanionList()
    {
        CompanionData = new BattleCharacterData[2];
    }
}