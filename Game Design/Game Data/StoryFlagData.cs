using System;

[Serializable]
public class StoryFlagData
{
    public string Id;
    public string Chapter;
    public string Town;
    public string Description;
    public bool Value;

    public StoryFlagData(StoryFlag storyFlag)
    {
        Id = storyFlag.Id;
        Chapter = storyFlag.Chapter;
        Town = storyFlag.Town;
        Description = storyFlag.Description;
        Value = storyFlag.Value;
    }
}