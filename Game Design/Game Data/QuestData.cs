using System;

/// <summary>
/// QuestData is the class that
/// compresses the meta data
/// for <c>Quest</c> based
/// on the id, category, type,
/// name, description, and 
/// whether or not it has been
/// completed.
/// </summary>
[Serializable]
public class QuestData
{
    //public variables
    public string Id;
    public string Category;
    public string Type;
    public string Name;
    public string Description;
    public bool Completed;

    //Constructor
    public QuestData(Quest quest)
    {
        Id = quest.Id;
        Category = quest.Category;
        Type = quest.Type;
        Name = quest.Name;
        Description = quest.Description;
        Completed = quest.Completed;
    }
}