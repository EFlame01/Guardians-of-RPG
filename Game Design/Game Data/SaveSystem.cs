using System.IO;
using UnityEngine;

/// <summary>
/// SaveSystem is a class that is responsible
/// for saving and loading the game.
/// </summary>
public class SaveSystem : Singleton<SaveSystem>
{

    /// <summary>
    /// Deletes all player data in the persistentDataPaths.
    /// </summary>
    public static void DeleteSavedData()
    {
        foreach(string relativePath in Units.SAVE_DATA_PATHS)
        {
            if(File.Exists(Application.persistentDataPath + relativePath))
                File.Delete(Application.persistentDataPath + relativePath);
        }
    }

    /// <summary>
    /// Saves all player data to the persistentDataPaths.
    /// </summary>
    public static void SavePlayerData()
    {
        PlayerData data = new PlayerData();
        string json = JsonUtility.ToJson(data);
        
        if(File.Exists(Application.persistentDataPath + Units.PLAYER_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.PLAYER_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.PLAYER_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.PLAYER_DATA_PATH);
    }

    /// <summary>
    /// Loads all player data from the persistentDataPaths.
    /// </summary>
    /// <returns>the player data</returns>
    public static PlayerData LoadPlayerData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.PLAYER_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.PLAYER_DATA_PATH);
        string json = DataEncoder.GetData();
        DataEncoder.ClearData();
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        data.LoadPlayerDataIntoGame();

        return data;
    }

    /// <summary>
    /// Saves all item data to the persistentDataPaths.
    /// </summary>
    public static void SaveItemData()
    {
        ItemDataContainer itemDataList = new ItemDataContainer();
        string json = JsonUtility.ToJson(itemDataList);
        
        if(File.Exists(Application.persistentDataPath + Units.ITEM_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.ITEM_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.ITEM_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.ITEM_DATA_PATH);
    }

    /// <summary>
    /// Loads all item data from the persistentDataPaths.
    /// </summary>
    /// <returns>the item data</returns>
    public static ItemDataContainer LoadItemData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.ITEM_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.ITEM_DATA_PATH);
        string json = DataEncoder.GetData();
        DataEncoder.ClearData();
        
        ItemDataContainer data = JsonUtility.FromJson<ItemDataContainer>(json);
        data.LoadItemDataIntoGame();
        return data;
    }

    /// <summary>
    /// Saves all quest data to the persistentDataPaths.
    /// </summary>
    public static void SaveQuestData()
    {
        QuestManager qm = Player.Instance().QuestManager;
        qm.UpdateQuestData();
        string json = JsonUtility.ToJson(qm);

        if(File.Exists(Application.persistentDataPath + Units.QUEST_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.QUEST_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.QUEST_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.QUEST_DATA_PATH);
    }

    /// <summary>
    /// Loads all quest data from the persistentDataPaths.
    /// </summary>
    /// <returns>the quest data</returns>
    public static QuestManager LoadQuestData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.QUEST_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.QUEST_DATA_PATH);
        string json = DataEncoder.GetData();
        DataEncoder.ClearData();
        QuestManager data = JsonUtility.FromJson<QuestManager>(json);
        
        data.LoadUpdatedQuestData();
        Player.Instance().SetQuestManager(data);

        return data;
    }

    /// <summary>
    /// Saves all player inventory data to the persistentDataPaths.
    /// </summary>
    public static void SaveInventoryData()
    {
        Inventory inventory = Player.Instance().Inventory;
        inventory.UpdateInventoryData();
        string json = JsonUtility.ToJson(inventory);

        if(File.Exists(Application.persistentDataPath + Units.INVENTORY_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.INVENTORY_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.INVENTORY_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.INVENTORY_DATA_PATH);
    }

    /// <summary>
    /// Loads all player inventory data from the persistentDataPaths.
    /// </summary>
    /// <returns>the player's inventory data</returns>
    public static Inventory LoadInventoryData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.INVENTORY_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.INVENTORY_DATA_PATH);
        string json = DataEncoder.GetData();
        DataEncoder.ClearData();
        Inventory data = JsonUtility.FromJson<Inventory>(json);
        
        data.LoadUpdatedInventoryData();
        Player.Instance().SetInventory(data);
        return data;
    }

    /// <summary>
    /// Saves all story flag data to the persistentDataPaths.
    /// </summary>
    public static void SaveStoryFlagData()
    {
        StoryFlagManager storyFlagManager = Player.Instance().StoryFlagManager;
        storyFlagManager.UpdateFlagData();
        string json = JsonUtility.ToJson(storyFlagManager);

        if(File.Exists(Application.persistentDataPath + Units.STORY_FLAG_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.STORY_FLAG_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.STORY_FLAG_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.STORY_FLAG_DATA_PATH);
    }

    /// <summary>
    /// Loads all story flag data from the persistentDataPaths.
    /// </summary>
    /// <returns>the story flag data</returns>
    public static StoryFlagManager LoadStoryFlagData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.STORY_FLAG_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.STORY_FLAG_DATA_PATH);
        string json = DataEncoder.GetData();
        StoryFlagManager data = JsonUtility.FromJson<StoryFlagManager>(json);
        
        data.LoadUpdatedFlagData();
        Player.Instance().SetStoryFlagManager(data);
        return data;
    }

    /// <summary>
    /// Saves all npc data to the persistentDataPaths.
    /// </summary>
    public static void SaveNpcData()
    {
        NpcDataContainer npcDataList = new NpcDataContainer();
        string json = JsonUtility.ToJson(npcDataList);
        
        if(File.Exists(Application.persistentDataPath + Units.NPC_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.NPC_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.NPC_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.NPC_DATA_PATH);
    }

    /// <summary>
    /// Loads all npc data from the persistentDataPaths.
    /// </summary>
    /// <returns>the npc data</returns>
    public static NpcDataContainer LoadNpcData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.NPC_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.NPC_DATA_PATH);
        string json = DataEncoder.GetData();
        
        NpcDataContainer data = JsonUtility.FromJson<NpcDataContainer>(json);
        data.LoadNpcDataIntoGame();
        
        return data;
    }

    /// <summary>
    /// Saves all well data to the persistentDataPaths.
    /// </summary>
    public static void SaveWellData()
    {
        WellDataContainer wellDataList = new WellDataContainer();
        string json = JsonUtility.ToJson(wellDataList);
        
        if(File.Exists(Application.persistentDataPath + Units.WELL_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.WELL_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.WELL_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.WELL_DATA_PATH);
    }

    /// <summary>
    /// Loads all well data from the persistentDataPaths.
    /// </summary>
    /// <returns>the npc data</returns>
    public static WellDataContainer LoadWellData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.WELL_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.WELL_DATA_PATH);
        string json = DataEncoder.GetData();
        
        WellDataContainer data = JsonUtility.FromJson<WellDataContainer>(json);
        data.LoadWellDataIntoGame();
        
        return data;
    }

    public static void SaveMedicalCenterData()
    {
        MedicalCenterDataContainer wellDataList = new MedicalCenterDataContainer();
        string json = JsonUtility.ToJson(wellDataList);
        
        if(File.Exists(Application.persistentDataPath + Units.MEDICAL_CENTER_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.MEDICAL_CENTER_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.MEDICAL_CENTER_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.MEDICAL_CENTER_DATA_PATH);
    }

    public static MedicalCenterDataContainer LoadMedicalCenterData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.MEDICAL_CENTER_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.MEDICAL_CENTER_DATA_PATH);
        string json = DataEncoder.GetData();
        
        MedicalCenterDataContainer data = JsonUtility.FromJson<MedicalCenterDataContainer>(json);
        data.LoadMedicalCenterDataIntoGame();
        
        return data;
    }

    public static void SaveSettingsData()
    {
        SettingsData data = new SettingsData();
        string json = JsonUtility.ToJson(data);
        
        if(File.Exists(Application.persistentDataPath + Units.SETTINGS_DATA_PATH))
            File.Delete(Application.persistentDataPath + Units.SETTINGS_DATA_PATH);
        
        File.WriteAllText(Application.persistentDataPath + Units.SETTINGS_DATA_PATH, json);
        DataEncoder.Instance.EncodeFile(Application.persistentDataPath, Units.SETTINGS_DATA_PATH);
    }

    public static SettingsData LoadSettingsData()
    {
        if(!File.Exists(Application.persistentDataPath + Units.SETTINGS_DATA_PATH))
            return null;

        DataEncoder.Instance.DecodeFile(Units.SETTINGS_DATA_PATH);
        string json = DataEncoder.GetData();
        DataEncoder.ClearData();
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);
        data.LoadSettingsData();

        return data;
    }
}