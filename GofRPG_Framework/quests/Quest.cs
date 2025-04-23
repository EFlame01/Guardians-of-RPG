
using UnityEngine;

/// <summary>
/// Quest is a class that holds tasks that the
/// <c>Player</c> is supposed to perform in the 
/// game.
/// </summary>
public class Quest
{
    //quest_id	quest_category	quest_type	quest_name	quest_description	quest_completed
    public string Id {get; private set;}
    public string Category  {get; private set;}
    public string Type {get; private set;}
    public string Name {get; private set;}
    public string Description {get; private set;}
    public bool Completed {get; private set;}

    //Constructor
    public Quest(string id, string category, string type, string name, string description, bool completed)
    {
        Id = id;
        Category = category;
        Type = type;
        Name = name;
        Description = description;
        Completed = completed;
    }

    //Setters and Getters
    public void SetCompleted(bool completed)
    {
        Completed = completed;
    }
}