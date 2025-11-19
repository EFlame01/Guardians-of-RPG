using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// QuestManager is a class that manages
/// the <c>Quest</c> objects that the <c>Player</c>
/// will have.
/// </summary>
public class QuestManager
{
    public static Dictionary<string, Quest> QuestDictionary = new Dictionary<string, Quest>();

    public QuestData[] QuestDatas;
    
    //Constructor
    public QuestManager()
    {
        QuestDictionary = new Dictionary<string, Quest>();
    }

    /// <summary>
    /// Adds a <paramref name="quest"/> to the 
    /// list of quests in the QuestManager.
    /// </summary>
    /// <param name="quest">the quest to be added to the list.</param>
    /// <returns><c>TRUE</c> if the quest was added successfully.
    /// <c>FALSE</c> if otherwise.</returns>
    public bool AddQuest(string id)
    {
        if(id == null)
            return false;
        if(QuestDictionary.ContainsKey(id))
            return false;
        
        Quest quest = QuestMaker.Instance.GetQuestByID(id);
        
        if(quest == null)
            return false;
        
        QuestDictionary.Add(id, quest);
        return true;
    }

    /// <summary>
    /// Marks a <paramref name="quest"/> in the list as completed.
    /// </summary>
    /// <param name="quest">the quest to be marked completed in list.</param>
    /// <returns><c>TRUE</c> if the quest was marked completed successfully.
    /// <c>FALSE</c> if otherwise.</returns>
    public bool MarkQuestCompleted(string id)
    {
        if(id == null)
            return false;
        if(!QuestDictionary.ContainsKey(id))
            return false;

        Quest completedQuest = QuestDictionary[id];
        completedQuest.SetCompleted(true);
        return true;
    }

    /// <summary>
    /// Saves all the quest data and places it inside the persistentDataPath.
    /// </summary>
    public void UpdateQuestData()
    {
        List<QuestData> quests = new List<QuestData>();
        foreach(KeyValuePair<string, Quest> questInfo in QuestDictionary)
        {
            quests.Add(new QuestData(questInfo.Value));
        }
        QuestDatas = quests.ToArray();
    }

    /// <summary>
    /// Loads all the story flag data in the persistentDataPath
    /// and adds it back to the dictionary.
    /// </summary>
    public void LoadUpdatedQuestData()
    {
        foreach(QuestData questData in QuestDatas)
            QuestDictionary.Add(questData.Id, new Quest(questData.Id, questData.Category, questData.Type, questData.Name, questData.Description, questData.Completed));
    }
}