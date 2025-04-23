
public class StoryFlag
{
    public string Id {get; private set;}
    public string Chapter {get; private set;}
    public string Town {get; private set;}
    public string Description {get; private set;}
    public bool Value;

    public StoryFlag(string id, string chapter, string town, string description, bool value)
    {
        Id = id;
        Chapter = chapter;
        Town = town;
        Description = description;
        Value = value;
    }
}