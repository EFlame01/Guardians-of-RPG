
/// <summary>
/// Ability is a class that holds a list of
/// <c>Effect</c> objects that a character will
/// be able to perform during battle.
/// </summary>
public class Ability
{
    public string Name {get; private set;}
    public string Description {get; private set;}
    public Effect[] Effects {get; private set;}
    public string[] WhenToUse {get; private set;}

    //Constructor
    public Ability(string name, string description, Effect[] effects, string[] whenToUse)
    {
        Name = name;
        Description = description;
        Effects = effects;
        WhenToUse = whenToUse;
    }
}