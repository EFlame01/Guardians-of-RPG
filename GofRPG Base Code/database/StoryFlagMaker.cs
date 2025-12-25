using System.Collections.Generic;

/// <summary>
/// StoryFlagMaker is a class that parses through
/// the data to create <c>StoryFlag</c> objects for
/// the story.
/// </summary>
public class StoryFlagMaker : Singleton<StoryFlagMaker>
{
    private int STORY_FLAG_INDEX = 18;

    /// <summary>
    /// Finds a story flag data and creates the 
    /// <c>StoryFlag</c> object based on the <paramref name="id"/> and file path.
    /// </summary>
    /// <param name="id">the primary key for specified data</param>
    /// <param name="useSaveData">variable that decides which data path to use</param>
    /// <returns>the story flag.</returns>
    public StoryFlag GetStoryFlag(string id)
    {
        if (name == null)
            return null;

        string[] flagAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STORY_FLAG_INDEX], id).Split(',');

        if (flagAttributes == null)
            return null;

        return new StoryFlag
        (
            flagAttributes[0],
            flagAttributes[1],
            flagAttributes[2],
            flagAttributes[3],
            flagAttributes[4] == "1"
        );
    }

    public StoryFlag[] GetAllStoryFlags()
    {
        List<StoryFlag> storyFlags = new();
        string[] flags = DataRetriever.Instance.SplitDataBasedOnRow(DataRetriever.Instance.Database[STORY_FLAG_INDEX]);
        string[] flagAttributes;

        foreach (string flag in flags)
        {
            if (flag.Trim().Length <= 0)
                break;

            flagAttributes = flag.Split(',');
            storyFlags.Add
            (
                new StoryFlag
                (
                    flagAttributes[0],
                    flagAttributes[1],
                    flagAttributes[2],
                    flagAttributes[3],
                    flagAttributes[4] == "1"
                )
            );
        }

        return storyFlags.ToArray();
    }

}