
/// <summary>
/// QuestMaker is a class that parses through
/// the data to create <c>Quest</c> objects for
/// the <c>Player</c> class and the story.
/// </summary>
public class QuestMaker : Singleton<QuestMaker>
{
    private const int QUEST_INDEX = 12;

    /// <summary>
    /// Finds quest data and creates the Quest object
    /// based on the id and file path.
    /// </summary>
    /// <param name="id">the primary key for specified data</param>
    /// <param name="saveData">variable that decides which data path to use</param>
    /// <returns>the quest object</returns>
    public Quest GetQuestByID(string id)
    {
        if (name == null)
            return null;

        string[] questAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[QUEST_INDEX], id).Split(',');

        if (questAttributes == null)
            return null;

        return new Quest
        (
            questAttributes[0],
            questAttributes[1],
            questAttributes[2],
            questAttributes[3],
            questAttributes[4],
            questAttributes[5] == "1"
        );
    }
}