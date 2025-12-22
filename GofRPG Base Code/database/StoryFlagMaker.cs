using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StoryFlagMaker is a class that parses through
/// the data to create <c>StoryFlag</c> objects for
/// the story.
/// </summary>
public class StoryFlagMaker : Singleton<StoryFlagMaker>
{
    private readonly string _storyFlagDataPath1 = "/database/story_flags.csv";

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

        string[] flagAttributes;

        // DataEncoder.Instance.DecodePersistentDataFile(_storyFlagDataPath1);
        // DataEncoder.Instance.GetStreamingAssetsFile(_storyFlagDataPath1);
        StartCoroutine(DataEncoder.Instance.GetStreamingAssetsFileWebGL(_storyFlagDataPath1));
        flagAttributes = DataEncoder.Instance.GetRowOfData(id).Split(',');
        DataEncoder.ClearData();

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
        if (name == null)
            return null;

        List<StoryFlag> storyFlags = new List<StoryFlag>();
        string[] flags;
        string[] flagAttributes;

        // DataEncoder.Instance.DecodePersistentDataFile(_storyFlagDataPath1);
        // DataEncoder.Instance.GetStreamingAssetsFile(_storyFlagDataPath1);
        StartCoroutine(DataEncoder.Instance.GetStreamingAssetsFileWebGL(_storyFlagDataPath1));
        flags = DataEncoder.Instance.GetRowsOfData();
        DataEncoder.ClearData();

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