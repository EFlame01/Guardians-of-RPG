using System;

[Serializable]
public class QuestData
{
    public string Id;
    public string Category;
    public string Type;
    public string Name;
    public string Description;
    public bool Completed;

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