using System.Collections.Generic;

/// <summary>
/// StoryFlagManager is a class that is 
/// responsible for managing all of the 
/// <c>StoryFlag</c> objects in the game.
/// </summary>
public class StoryFlagManager
{
    public static Dictionary<string, StoryFlag> FlagDictionary { get; private set; }
    public StoryFlagData[] StoryFlagDatas;

    //Constructor
    public StoryFlagManager()
    {
        FlagDictionary = new Dictionary<string, StoryFlag>();
    }

    public void AddAllStoryFlags()
    {
        StoryFlag[] storyFlags = StoryFlagMaker.Instance.GetAllStoryFlags();
        foreach (StoryFlag flag in storyFlags)
            FlagDictionary.Add(flag.Id, flag);
    }

    /// <summary>
    /// Adds a story flag into the dictionary of 
    /// story flags based on the <paramref name="flagID"/>
    /// </summary>
    /// <param name="flagID">the primary key needed to find story flag</param>
    public void AddFlag(string flagID)
    {
        StoryFlag flag = StoryFlagMaker.Instance.GetStoryFlag(flagID);
        if (flag != null && !FlagDictionary.ContainsKey(flagID))
            FlagDictionary.Add(flagID, flag);
    }

    /// <summary>
    /// Updates the story flag in the dictionary to true or false.
    /// based on the <paramref name="id"/> and <paramref name="value"/>.
    /// </summary>
    /// <param name="id">the primary key needed to find story flag</param>
    /// <param name="value">the value needed to update flag</param>
    public void UpdateFlag(string id, bool value)
    {
        if (id == null || id.Length <= 0)
            return;

        if (!FlagDictionary.ContainsKey(id))
            AddFlag(id);
        FlagDictionary[id].Value = value;
    }

    /// <summary>
    /// Saves all the story flag data and places it inside the persistentDataPath.
    /// </summary>
    public void UpdateFlagData()
    {
        List<StoryFlagData> storyFlags = new List<StoryFlagData>();
        foreach (KeyValuePair<string, StoryFlag> storyFlagInfo in FlagDictionary)
        {
            storyFlags.Add(new StoryFlagData(storyFlagInfo.Value));
        }
        StoryFlagDatas = storyFlags.ToArray();
    }

    /// <summary>
    /// Loads all the story flag data in the persistentDataPath
    /// and adds it back to the dictionary.
    /// </summary>
    public void LoadUpdatedFlagData()
    {
        foreach (StoryFlagData data in StoryFlagDatas)
        {
            if (FlagDictionary.ContainsKey(data.Id))
                FlagDictionary[data.Id] = new StoryFlag(data.Id, data.Chapter, data.Town, data.Description, data.Value);
            else
                FlagDictionary.Add
                (
                    data.Id,
                    new StoryFlag
                    (
                        data.Id, data.Chapter, data.Town, data.Description, data.Value
                    )
                );
        }
    }
}