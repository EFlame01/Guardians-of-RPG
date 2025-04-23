
/// <summary>
/// QuestMaker is a class that parses through
/// the data to create <c>Quest</c> objects for
/// the <c>Player</c> class and the story.
/// </summary>
public class QuestMaker : Singleton<QuestMaker>
{
    private readonly string _questDataPath = "/database/quests.csv";

    /// <summary>
    /// Finds quest data and creates the Quest object
    /// based on the id and file path.
    /// </summary>
    /// <param name="id">the primary key for specified data</param>
    /// <param name="saveData">variable that decides which data path to use</param>
    /// <returns>the quest object</returns>
    public Quest GetQuestByID(string id)
    {
        if(name == null) 
            return null;
        
        Quest quest;
        string[] questAttributes;

        DataEncoder.Instance.DecodeFile(_questDataPath);
        questAttributes = DataEncoder.Instance.GetRowOfData(id).Split(',');
        DataEncoder.ClearData();

        quest = new Quest
        (
            questAttributes[0],
            questAttributes[1],
            questAttributes[2],
            questAttributes[3],
            questAttributes[4],
            questAttributes[5] == "1"
        );

        return quest;
    }
}