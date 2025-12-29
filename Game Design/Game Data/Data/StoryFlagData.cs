using System;

/// <summary>
/// StoryFlagData is a class that
/// holds the meta data for the 
/// <c>StoryFlag</c> to be saved
/// and loaded in the game.
/// </summary>
[Serializable]
public class StoryFlagData
{
    //public variables
    public string Id;
    public string Chapter;
    public string Town;
    public string Description;
    public bool Value;

    //Constructor
    public StoryFlagData(StoryFlag storyFlag)
    {
        Id = storyFlag.Id;
        Chapter = storyFlag.Chapter;
        Town = storyFlag.Town;
        Description = storyFlag.Description;
        Value = storyFlag.Value;
    }
}