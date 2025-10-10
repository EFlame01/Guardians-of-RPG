using UnityEngine;

public class PlayerCompanions : PersistentSingleton<PlayerCompanions>
{
    public BattleCharacterData[] CompanionData { get; private set; }

    public void Start()
    {
        CompanionData = new BattleCharacterData[2];
    }

    public void AddCompanion(BattleCharacterData data)
    {
        bool added = false;
        for (int i = 0; i < CompanionData.Length; i++)
        {
            if (CompanionData[i] == null)
            {
                CompanionData[i] = data;
                added = true;
            }
        }
        if (!added)
            Debug.LogWarning("WARNING: Could not add battle character data. Placement is full.");
    }

    public void AddCompanions(BattleCharacterData[] data)
    {
        if (data.Length == CompanionData.Length)
            CompanionData = data;
        else if (data == null)
            Debug.LogWarning("WARNING: could not add battle character data because data was null");
        else
        {
            for (int i = 0; i < CompanionData.Length; i++)
                CompanionData[i] = data[i];
        }
    }

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

    public void ClearCompanionList()
    {
        CompanionData = new BattleCharacterData[2];
    }
}